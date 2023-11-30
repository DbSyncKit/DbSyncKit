using Sync.Core.Comparer;
using Sync.Core.DataContract;
using Sync.Core.Enum;
using Sync.DB.Interface;
using Sync.DB.Manager;
using System.Collections.Concurrent;
using System.Reflection;

namespace Sync.Core.Helper
{
    public class DataMetadataComparisonHelper<T> where T : IDataContractComparer
    {
        public static Result<T> GetDifferences(HashSet<T> sourceList, HashSet<T> destinationList, List<string> keyProperties, List<string> excludedProperties)
        {
            List<T> added = new List<T>();
            List<T> deleted = new List<T>();
            ConcurrentBag<(T edit, Dictionary<string, object> updatedProperties)> edited = new ConcurrentBag<(T, Dictionary<string, object>)>();

            var keyComparer = new KeyEqualityComparer<T>(keyProperties, excludedProperties);
            var properties = typeof(T).GetProperties()
                .Where(prop => keyProperties.Contains(prop.Name) && !excludedProperties.Contains(prop.Name));

            //Columns.Add(property.Name);

            // Identify added entries
            added.AddRange(sourceList.Except(destinationList, keyComparer));

            // Identify deleted entries
            deleted.AddRange(destinationList.Except(sourceList, keyComparer));

            // Identify edited entries
            var sourceKeyDictionary = sourceList
                .Except(added)
                .ToDictionary(row => GenerateCompositeKey(row, keyProperties), row => row);

            var destinationKeyDictionary = destinationList
                .Except(deleted)
                .ToDictionary(row => GenerateCompositeKey(row, keyProperties), row => row);

            Parallel.ForEach(sourceKeyDictionary, kvp =>
            {
                var sourceContract = kvp.Value;

                T destinationContract;
                if (destinationKeyDictionary.TryGetValue(GenerateCompositeKey(sourceContract, keyProperties), out destinationContract))
                {
                    var (isEdited, updatedProperties) = GetEdited(sourceContract, destinationContract, excludedProperties);

                    if (isEdited)
                    {
                        edited.Add((sourceContract, updatedProperties));
                    }
                }
            });


            var result = new Result<T>();
            result.Added = added;
            result.Deleted = deleted;
            result.Edited = edited.ToList();
            result.SourceDataCount = sourceList.Count;
            result.DestinaionDataCount = destinationList.Count;
            result.ResultChangeType = DetermineChangeType(result);
            return result;
        }

        private static (bool isEdited, Dictionary<string, object> updatedProperties) GetEdited(T source, T destination, List<string> excludedProperties)
        {
            Dictionary<string, object> updatedProperties = new Dictionary<string, object>();
            bool isEdited = false;
            if (source.Equals(destination))
            {
                return (isEdited, updatedProperties);
            }

            foreach (PropertyInfo prop in TypePropertyCacheManager.GetTypeProperties(typeof(T)).Where(prop => !excludedProperties.Contains(prop.Name)))
            {
                object sourceValue = prop.GetValue(source);
                object destinationValue = prop.GetValue(destination);

                // Compare values
                if (!EqualityComparer<object>.Default.Equals(sourceValue, destinationValue))
                {
                    isEdited = true;
                    updatedProperties[prop.Name] = sourceValue;
                }
            }

            return (isEdited, updatedProperties);
        }

        // Function to generate a composite key based on specified properties
        private static string GenerateCompositeKey(T DataContract, List<string> keyProperties)
        {
            // Concatenate property values based on keyProperties
            var compositeKey = string.Join("_", keyProperties.Select(propName => DataContract.GetType().GetProperty(propName)?.GetValue(DataContract)));
            return compositeKey;
        }

        private static ChangeType DetermineChangeType(Result<T> result)
        {
            // Tuple representing combinations of hasAdded, hasEdited, and hasDeleted
            var key = (result.Added?.Count > 0, result.Edited?.Count > 0, result.Deleted?.Count > 0);

            // Switch based on the combinations
            switch (key)
            {
                case (true, true, true):
                    // Added, Edited, and Deleted present
                    return ChangeType.All;

                case (true, false, false):
                    // Only Added present
                    return ChangeType.Added;

                case (true, true, false):
                    // Added and Edited present
                    return ChangeType.AddedWithEdited;

                case (false, true, false):
                    // Only Edited present
                    return ChangeType.Edited;

                case (false, true, true):
                    // Edited and Deleted present
                    return ChangeType.EditedWithDeleted;

                case (false, false, true):
                    // Only Deleted present
                    return ChangeType.Deleted;

                default:
                    // No significant changes
                    return ChangeType.None;
            }
        }
    }
}

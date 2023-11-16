using System.Text;

using Sync.DB.Interface;

namespace Sync.DB.Utils
{
    /// <summary>
    /// Generic utility class for working with data contract classes.
    /// </summary>
    /// <typeparam name="T">The type of the data contract class.</typeparam>
    public class DataContractUtility<T> : IDataContractComparer where T : IDataContractComparer
    {
        /// <summary>
        /// Calculates the hash code for the object, excluding specified properties.
        /// </summary>
        /// <param name="excludedProperties">List of property names to exclude from hash code calculation.</param>
        /// <returns>The calculated hash code.</returns>
        public int GetHashCode(List<string> excludedProperties)
        {
            // Get all properties of the Entity class
            var properties = typeof(T).GetProperties();

            // Filter out properties that are in the exclusion list
            var filteredProperties = properties
                .Where(prop => !excludedProperties.Contains(prop.Name));

            // Calculate the hash code using XOR
            int hashCode = base.GetHashCode();
            foreach (var prop in filteredProperties)
            {
                var value = prop.GetValue(this);
                if (value != null)
                {
                    hashCode ^= value.GetHashCode();
                }
            }

            return hashCode;
        }

        /// <summary>
        /// Generates a string representation of the object, excluding specified properties.
        /// </summary>
        /// <param name="excludedProperties">List of property names to exclude from the string representation.</param>
        /// <returns>The string representation of the object.</returns>
        public string ToString(List<string> excludedProperties)
        {
            // Get all properties of the Entity class
            var properties = typeof(T).GetProperties();

            // Filter out properties that are in the exclusion list
            var filteredProperties = properties
                .Where(prop => !excludedProperties.Contains(prop.Name));

            // Create a string representation
            StringBuilder stringBuilder = new StringBuilder(base.ToString());
            foreach (var prop in filteredProperties)
            {
                var value = prop.GetValue(this);
                stringBuilder.Append($",\t{prop.Name}: {value}");
            }

            return stringBuilder.ToString();
        }


        /// <summary>
        /// Checks if the current object is equal to another object, considering specified properties.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <param name="listOfProperties">List of properties to consider for equality check.</param>
        /// <param name="isExcluded">If true, specified properties are excluded; otherwise, they are included.</param>
        /// <returns>True if the objects are equal; otherwise, false.</returns>
        public bool Equals(T obj, List<string> listOfProperties, bool isExcluded = true)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var props = GetType().GetProperties();

            if (isExcluded)
                props = props.Where(prop => !listOfProperties.Contains(prop.Name)).ToArray();
            else
                props = props.Where(prop => listOfProperties.Contains(prop.Name)).ToArray();


            foreach (var prop in props)
            {
                if (!EqualityComparer<object?>.Default.Equals(prop.GetValue(this), prop.GetValue(obj)))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets a list of property names representing the columns of the data contract class.
        /// </summary>
        /// <returns>A list of property names.</returns>
        public List<string> GetColumns()
        {
            // Get all properties of the class
            return typeof(T).GetProperties().Select(col => col.Name).ToList();
        }

        /// <summary>
        /// Gets the name of the property with a specified attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="type">The type of the data contract class.</param>
        /// <returns>The name of the property with the specified attribute; null if not found.</returns>
        public string GetPropertyNameWithAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            var property = type.GetProperties()
                .FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(TAttribute)));

            return property?.Name;
        }

        /// <summary>
        /// Gets the value of the property with a specified attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="type">The type of the data contract class.</param>
        /// <returns>The value of the property with the specified attribute; null if not found.</returns>
        public object GetPropertyValueWithAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            var property = type.GetProperties()
                .FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(TAttribute)));

            return property?.GetValue(property?.Name);
        }
    }
}

using DbSyncKit.DB.Utils;
using System.Data;
namespace DbSyncKit.DB.Extensions
{
    /// <summary>
    /// Extension methods for DataRow to simplify retrieving values from columns.
    /// </summary>
    public static class DataRowExtensions
    {
        /// <summary>
        /// Gets the value of a specified column in the DataRow, converted to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to which the column value should be converted.</typeparam>
        /// <param name="row">The DataRow from which to retrieve the value.</param>
        /// <param name="columnName">The name of the column.</param>
        /// <returns>The value of the specified column, converted to the specified type.</returns>
        /// <remarks>
        /// This extension method is intended for use with classes that inherit from <see cref="DataContractUtility{T}"/>.
        /// </remarks>
        public static T GetValue<T>(this DataRow row, string columnName)
        {
            // Check if the DataRow is null
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }

            // Check if the column exists in the DataRow's table
            if (row.Table.Columns.Contains(columnName))
            {
                // Retrieve the value from the specified column
                object value = row[columnName];

                // Check if the value is not DBNull
                if (value != DBNull.Value)
                {
                    // Convert the value to the specified type
                    return (T)Convert.ChangeType(value, typeof(T));
                }
            }

            // Handle the case when the column doesn't exist or the value is DBNull return a default value
            return default(T)!;
        }
    }
}


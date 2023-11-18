using Sync.DB.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sync.DB.Helper
{
    public class QueryHelper
    {

        public void ReplacePlaceholder(ref StringBuilder stringBuilder, string placeholder, string replacement)
        {
            // Use Regex to replace all occurrences of the placeholder
            string pattern = Regex.Escape(placeholder);
            stringBuilder.Replace(pattern, replacement);
        }

        public string GetTableName<T>()
        {
            TableNameAttribute tableNameAttribute = (TableNameAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(TableNameAttribute));

            if (tableNameAttribute != null)
                return tableNameAttribute.TableName;

            return typeof(T).Name;
        }

        public string? GetTableSchema<T>()
        {
            TableSchemaAttribute tableSchemaAttribute = (TableSchemaAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(TableSchemaAttribute));

            if (tableSchemaAttribute != null)
                return tableSchemaAttribute.SchemaName;

            return null;
        }

        public bool GetInsertWithID<T>()
        {
            GenerateInsertWithIDAttribute tableSchemaAttribute = (GenerateInsertWithIDAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(GenerateInsertWithIDAttribute));

            if (tableSchemaAttribute != null)
                return tableSchemaAttribute.GenerateWithID;

            return false;
        }

    }
}

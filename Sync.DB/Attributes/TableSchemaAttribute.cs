using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sync.DB.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableSchemaAttribute : Attribute
    {
        public string SchemaName { get; }

        public TableSchemaAttribute(string schemaName)
        {
            SchemaName = schemaName;
        }
    }
}

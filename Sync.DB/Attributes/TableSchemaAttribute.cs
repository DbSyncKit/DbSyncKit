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

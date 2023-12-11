namespace DbSyncKit.DB.Attributes
{
    /// <summary>
    /// Specifies the schema name of the database table associated with a class.
    /// </summary>
    /// <remarks>
    /// This attribute can be applied to classes to indicate the schema name of the corresponding
    /// database table. It is used to associate a class with a specific schema in database operations.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableSchemaAttribute : Attribute
    {
        /// <summary>
        /// Gets the schema name of the database table.
        /// </summary>
        public string SchemaName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableSchemaAttribute"/> class.
        /// </summary>
        /// <param name="schemaName">The schema name of the database table associated with the class.</param>
        public TableSchemaAttribute(string schemaName)
        {
            SchemaName = schemaName ?? throw new ArgumentNullException(nameof(schemaName));
        }
    }
}

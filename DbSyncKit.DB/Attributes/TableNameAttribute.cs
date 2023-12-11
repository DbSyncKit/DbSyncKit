namespace DbSyncKit.DB.Attributes
{
    /// <summary>
    /// Specifies the name of the database table associated with a class.
    /// </summary>
    /// <remarks>
    /// This attribute can be applied to classes to indicate the name of the corresponding
    /// database table. It is used to associate a class with a specific table in database operations.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the database table.
        /// </summary>
        public string TableName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableNameAttribute"/> class.
        /// </summary>
        /// <param name="tableName">The name of the database table associated with the class.</param>
        public TableNameAttribute(string tableName)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName)); ;
        }
    }
}

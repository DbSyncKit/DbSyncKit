namespace DbSyncKit.DB.Attributes
{
    /// <summary>
    /// Specifies whether the insert query generation for a class should include the ID property or exclude it.
    /// </summary>
    /// <remarks>
    /// This attribute can be applied to classes to indicate whether the ID property's value should be included
    /// in the insert query generation. For example, it can be used to determine whether the ID property should be
    /// part of the generated insert query or excluded. The IncludeIdentityInsert property can contain database-specific
    /// statements that affect identity insert behavior (e.g., SET IDENTITY_INSERT for MSSQL), or similar statements
    /// applicable to other databases like MySQL or PostgreSQL.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public class GenerateInsertWithIDAttribute : Attribute
    {
        /// <summary>
        /// Gets a value indicating whether the insert query generation should include the ID property.
        /// </summary>
        /// <value><c>true</c> if the insert query should include the ID property; otherwise, <c>false</c>.</value>
        public bool GenerateWithID { get; }

        /// <summary>
        /// Gets a value indicating whether to include database-specific SQL statements during insert query generation
        /// that affect identity insert behavior (e.g., SET IDENTITY_INSERT for MSSQL).
        /// </summary>
        /// <value><c>true</c> to include identity insert statements; otherwise, <c>false</c>.</value>
        public bool IncludeIdentityInsert { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateInsertWithIDAttribute"/> class.
        /// </summary>
        /// <param name="generateWithID">Indicates whether the insert query generation should include the ID property.
        /// Default is <c>true</c>.</param>
        /// <param name="includeIdentityInsert">Indicates whether to include identity insert statements in the
        /// insert query generation. Default is <c>true</c>.</param>
        public GenerateInsertWithIDAttribute(bool generateWithID = true, bool includeIdentityInsert = true)
        {
            GenerateWithID = generateWithID;
            IncludeIdentityInsert = includeIdentityInsert;
        }
    }
}
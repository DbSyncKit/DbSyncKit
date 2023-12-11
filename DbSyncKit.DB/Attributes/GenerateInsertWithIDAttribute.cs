namespace DbSyncKit.DB.Attributes
{
    /// <summary>
    /// Specifies whether the insert query generation for a class should include the ID property or exclude it.
    /// </summary>
    /// <remarks>
    /// This attribute can be applied to classes to indicate whether the ID property's value should be included
    /// in the insert query generation. For example, it can be used to determine whether the ID property should be
    /// part of the generated insert query or excluded.
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
        /// Initializes a new instance of the <see cref="GenerateInsertWithIDAttribute"/> class.
        /// </summary>
        /// <param name="generateWithID">Indicates whether the insert query generation should include the ID property.
        /// Default is <c>true</c>.</param>
        public GenerateInsertWithIDAttribute(bool generateWithID = true)
        {
            GenerateWithID = generateWithID;
        }


    }
}

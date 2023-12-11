namespace DbSyncKit.DB.Attributes
{
    /// <summary>
    /// Specifies whether a property should be excluded from certain operations, such as data fetching and query generation.
    /// </summary>
    /// <remarks>
    /// This attribute can be applied to properties within a class to indicate that the marked property should
    /// be excluded from specific operations. For example, it can be used to exclude a property from data fetching
    /// and query generation.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcludedPropertyAttribute : Attribute
    {
        /// <summary>
        /// Gets a value indicating whether the property should be excluded from operations.
        /// </summary>
        /// <value><c>true</c> if the property should be excluded; otherwise, <c>false</c>.</value>
        public bool Excluded { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcludedPropertyAttribute"/> class.
        /// </summary>
        /// <param name="excluded">Indicates whether the property should be excluded. Default is <c>true</c>.</param>
        public ExcludedPropertyAttribute(bool excluded = true)
        {
            Excluded = excluded;
        }
    }
}

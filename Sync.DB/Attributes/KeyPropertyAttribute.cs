namespace DbSyncKit.DB.Attributes
{
    /// <summary>
    /// Specifies whether a property should be considered as a key property.
    /// </summary>
    /// <remarks>
    /// This attribute can be applied to properties within a class to indicate that the marked property
    /// should be treated as a key property. It is typically used to identify properties that uniquely
    /// identify instances of the class in database operations such as insert, update, and delete.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class KeyPropertyAttribute : Attribute
    {
        /// <summary>
        /// Gets a value indicating whether the property should be considered as a key property.
        /// </summary>
        /// <value><c>true</c> if the property should be treated as a key property; otherwise, <c>false</c>.</value>
        public bool KeyProperty { get; }

        /// <summary>
        /// Gets a value indicating whether the property is a primary key.
        /// </summary>
        /// <value><c>true</c> if the property is a primary key; otherwise, <c>false</c>.</value>
        public bool IsPrimaryKey { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyPropertyAttribute"/> class.
        /// </summary>
        /// <param name="keyProperty">Indicates whether the property should be considered as a key property.
        /// Default is <c>true</c>.</param>
        /// <param name="isPrimaryKey">Indicates whether the property is a primary key.
        /// Default is <c>false</c>.</param>
        public KeyPropertyAttribute(bool keyProperty = true, bool isPrimaryKey = false)
        {
            KeyProperty = keyProperty;
            IsPrimaryKey = isPrimaryKey;
        }
    }
}

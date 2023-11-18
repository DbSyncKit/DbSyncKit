namespace Sync.DB.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class KeyPropertyAttribute : Attribute
    {
        public bool KeyPropery { get; }
        // This attribute can be used to mark properties as key properties

        public KeyPropertyAttribute(bool keyProperty = true)
        {
            KeyPropery = keyProperty;
        }

    }
}

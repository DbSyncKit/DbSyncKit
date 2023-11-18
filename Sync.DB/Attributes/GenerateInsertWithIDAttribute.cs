namespace Sync.DB.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GenerateInsertWithIDAttribute : Attribute
    {
        public bool GenerateWithID { get; }

        // if insert querry generation will be with id or without id
        public GenerateInsertWithIDAttribute(bool generateWithID = true)
        {
            GenerateWithID = generateWithID;
        }


    }
}

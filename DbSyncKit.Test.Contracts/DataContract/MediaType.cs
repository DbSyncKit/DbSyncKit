using DbSyncKit.DB.Attributes;
using DbSyncKit.DB.Extensions;
using DbSyncKit.DB.Utils;
using System.Data;

namespace DbSyncKit.Test.SampleContract.DataContract
{
    [TableName("MediaType"), TableSchema("dbo")]
    public class MediaType : DataContractUtility<MediaType>
    {
        #region Declerations
        [KeyPropertyAttribute(isPrimaryKey: true)]
        public int MediaTypeId { get; set; }
        public string Name { get; set; }

        #endregion

        #region Constructor

        public MediaType(DataRow mediaTypeInfo)
        {
            if (mediaTypeInfo == null)
                throw new ArgumentNullException(nameof(mediaTypeInfo));

            MediaTypeId = mediaTypeInfo.GetValue<int>("MediaTypeId");
            if (mediaTypeInfo.IsNull("Name"))
                throw new ArgumentNullException(nameof(Name), "Name cannot be null.");

            Name = mediaTypeInfo.GetValue<string>("Name");
        }

        #endregion
    }
}

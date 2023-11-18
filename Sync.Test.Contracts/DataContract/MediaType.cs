using Sync.DB.Attributes;
using Sync.DB.Extensions;
using Sync.DB.Utils;
using System.Data;

namespace Sync.Test.SampleContract.DataContract
{
    [TableName("MediaType"), TableSchema("dbo")]
    public class MediaType : DataContractUtility<MediaType>
    {
        #region Declerations
        [KeyProperty]
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

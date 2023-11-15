using Sync.DB.Extensions;
using Sync.DB.Utils;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Sync.Test.SampleContract.DataContract
{
    public class MediaType : DataContractUtility<MediaType>
    {
        #region Declerations
        [Key]
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

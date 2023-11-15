using Sync.DB.Extensions;
using Sync.DB.Utils;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Sync.Test.SampleContract.DataContract
{
    public class Artist : DataContractUtility<Artist>
    {
        #region Declerations

        [Key]
        public int ArtistId { get; set; }

        public string Name { get; set; }
        #endregion

        #region Constructor
        public Artist(DataRow ArtistInfo)
        {
            if (ArtistInfo == null)
                throw new ArgumentNullException("entityInfo");

            if (ArtistInfo.IsNull("ArtistId"))
                throw new Exception("ArtistId cannot be null.");
            ArtistId = ArtistInfo.GetValue<int>("ArtistId");

            if (ArtistInfo.IsNull("Name"))
                throw new Exception("Name cannot be null.");
            Name = ArtistInfo.GetValue<string>("Name")!;
        }
        #endregion
    }
}

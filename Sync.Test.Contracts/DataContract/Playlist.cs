using System.ComponentModel.DataAnnotations;
using System.Data;
using Sync.DB.Extensions;
using Sync.DB.Utils;

namespace Sync.Test.SampleContract.DataContract
{
    public class Playlist : DataContractUtility<Playlist>
    {
        #region Declerations
        [Key]
        public int PlaylistId { get; set; }
        public string Name { get; set; }

        #endregion

        #region Constructor

        public Playlist(DataRow playlistInfo)
        {
            if (playlistInfo == null)
                throw new ArgumentNullException(nameof(playlistInfo));

            PlaylistId = playlistInfo.GetValue<int>("PlaylistId");
            if (playlistInfo.IsNull("Name"))
                throw new ArgumentNullException(nameof(Name), "Name cannot be null.");

            Name = playlistInfo.GetValue<string>("Name");
        }

        #endregion
    }
}

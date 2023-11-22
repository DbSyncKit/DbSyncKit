using Sync.DB.Attributes;
using Sync.DB.Extensions;
using Sync.DB.Utils;
using System.Data;

namespace Sync.Test.SampleContract.DataContract
{
    [TableName("PlaylistTrack"), TableSchema("dbo")]
    public class PlaylistTrack : DataContractUtility<PlaylistTrack>
    {
        #region Declerations
        [KeyProperty]
        public int PlaylistId { get; set; }
        [KeyProperty]
        public int TrackId { get; set; }

        #endregion

        #region Constructor

        public PlaylistTrack(DataRow playlistTrackInfo)
        {
            if (playlistTrackInfo == null)
                throw new ArgumentNullException(nameof(playlistTrackInfo));

            PlaylistId = playlistTrackInfo.GetValue<int>("PlaylistId");
            TrackId = playlistTrackInfo.GetValue<int>("TrackId");
        }

        #endregion
    }
}

using DbSyncKit.DB.Attributes;
using DbSyncKit.DB.Extensions;
using DbSyncKit.DB.Utils;
using System.Data;

namespace DbSyncKit.Test.SampleContract.DataContract
{
    [TableName("PlaylistTrack"), TableSchema("dbo")]
    public class PlaylistTrack : DataContractUtility<PlaylistTrack>
    {
        #region Declerations
        [KeyPropertyAttribute(isPrimaryKey: true)]
        public int PlaylistId { get; set; }
        [KeyPropertyAttribute]
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

using DbSyncKit.DB.Attributes;
using DbSyncKit.DB.Extensions;
using DbSyncKit.DB.Interface;
using System.Data;

namespace DbSyncKit.Test.SampleContract.DataContract
{
    [TableName("PlaylistTrack")]
    public class PlaylistTrack : IDataContract
    {
        #region Declerations
        [KeyProperty(isPrimaryKey: true,isComparable:true)]
        public int PlaylistId { get; set; }
        [KeyProperty(isPrimaryKey: true,isComparable: true)]
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

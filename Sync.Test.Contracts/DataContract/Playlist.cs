﻿using DbSyncKit.DB.Attributes;
using DbSyncKit.DB.Extensions;
using DbSyncKit.DB.Utils;
using System.Data;

namespace DbSyncKit.Test.SampleContract.DataContract
{
    [TableName("Playlist"), TableSchema("dbo")]
    public class Playlist : DataContractUtility<Playlist>
    {
        #region Declerations
        [KeyPropertyAttribute(isPrimaryKey: true)]
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

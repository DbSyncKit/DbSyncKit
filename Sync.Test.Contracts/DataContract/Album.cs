using Sync.DB.Attributes;
using Sync.DB.Extensions;
using Sync.DB.Utils;
using System.Data;

namespace Sync.Test.SampleContract.DataContract
{
    [TableName("Album"), TableSchema("dbo")]
    public class Album : DataContractUtility<Album>
    {
        #region Declerations

        [KeyProperty]
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public int ArtistId { get; set; }

        #endregion

        #region Constructor

        public Album(DataRow AlbumInfo)
        {
            if (AlbumInfo == null)
                throw new ArgumentNullException("entityInfo");

            if (AlbumInfo.IsNull("AlbumId"))
                throw new Exception("AlbumId cannot be null.");
            AlbumId = AlbumInfo.GetValue<int>("AlbumId");

            if (AlbumInfo.IsNull("Title"))
                throw new Exception("Title cannot be null.");
            Title = AlbumInfo.GetValue<string>("Title")!;

            if (AlbumInfo.IsNull("ArtistId"))
                throw new Exception("ArtistId cannot be null.");
            ArtistId = AlbumInfo.GetValue<int>("ArtistId")!;
        }

        #endregion
    }
}

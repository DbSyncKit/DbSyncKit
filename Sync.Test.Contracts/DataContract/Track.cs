using Sync.DB.Attributes;
using Sync.DB.Extensions;
using Sync.DB.Utils;
using System.Data;

namespace Sync.Test.SampleContract.DataContract
{
    [TableName("Track"), TableSchema("dbo")]
    public class Track : DataContractUtility<Track>
    {
        #region Decleration

        [KeyPropertyAttribute(isPrimaryKey: true)]
        public int TrackId { get; set; }
        public string Name { get; set; }
        public int AlbumId { get; set; }
        public int MediaTypeId { get; set; }
        public int GenreId { get; set; }
        public string Composer { get; set; }
        public int Milliseconds { get; set; }
        public int Bytes { get; set; }
        public decimal UnitPrice { get; set; }
        #endregion

        #region Constructor

        public Track(DataRow trackInfo)
        {
            if (trackInfo == null)
                throw new ArgumentNullException(nameof(trackInfo));

            TrackId = trackInfo.GetValue<int>("TrackId");

            if (trackInfo.IsNull("Name"))
                throw new ArgumentNullException(nameof(Name), "Name cannot be null.");

            Name = trackInfo.GetValue<string>("Name")!;

            AlbumId = trackInfo.GetValue<int>("AlbumId");
            MediaTypeId = trackInfo.GetValue<int>("MediaTypeId");
            GenreId = trackInfo.GetValue<int>("GenreId");
            Composer = trackInfo.GetValue<string>("Composer")!;
            Milliseconds = trackInfo.GetValue<int>("Milliseconds");
            Bytes = trackInfo.GetValue<int>("Bytes");
            UnitPrice = trackInfo.GetValue<decimal>("UnitPrice");
        }

        #endregion
    }
}

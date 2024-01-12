using DbSyncKit.DB.Attributes;
using DbSyncKit.DB.Extensions;
using DbSyncKit.DB.Interface;
using System.Data;

namespace DbSyncKit.Test.SampleContract.DataContract
{
    [TableName("Artist")]

    public class Artist : IDataContract
    {
        #region Declerations

        [KeyProperty(isPrimaryKey: true)]
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

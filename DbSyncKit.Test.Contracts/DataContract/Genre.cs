using DbSyncKit.DB.Attributes;
using DbSyncKit.DB.Extensions;
using System.Data;

namespace DbSyncKit.Test.SampleContract.DataContract
{
    [TableName("Genre")]
    public class Genre
    {

        #region Decleration
        [KeyProperty(isPrimaryKey: true)]
        public int GenreId { get; set; }
        public string Name { get; set; }

        #endregion

        #region Constructor

        public Genre(DataRow genreData)
        {
            if (genreData == null)
                throw new ArgumentNullException(nameof(genreData));

            GenreId = genreData.GetValue<int>("GenreId");

            if (genreData.IsNull("Name"))
                throw new ArgumentNullException(nameof(Name), "Name cannot be null.");

            Name = genreData.GetValue<string>("Name");
        }

        #endregion
    }
}

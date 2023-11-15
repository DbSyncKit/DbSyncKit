using Sync.DB.Extensions;
using Sync.DB.Utils;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Sync.Test.SampleContract.DataContract
{
    public class Genre : DataContractUtility<Genre>
    {

        #region Decleration
        [Key]
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

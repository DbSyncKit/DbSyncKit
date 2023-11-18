﻿using Sync.DB.Attributes;
using Sync.DB.Extensions;
using Sync.DB.Utils;
using System.Data;

namespace Sync.Test.SampleContract.DataContract
{
    [TableName("Genre"), TableSchema("dbo")]
    public class Genre : DataContractUtility<Genre>
    {

        #region Decleration
        [KeyProperty]
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

using System.Data;

namespace DbSyncKit.MSSQL.DataContract.Generic
{
    public class GenericIndex : GenericTable
    {
        #region Constructor

        public GenericIndex(DataRow indexInfo) : base(indexInfo)
        {
            index_name = indexInfo.Field<string>("index_name")!;
            index_is_unique = indexInfo.Field<bool>("index_is_unique")!;
            index_key_column_names = indexInfo.Field<string>("index_key_column_names")!;
        }

        #endregion

        #region Properties

        public string index_name { get; set; }
        public bool? index_is_unique { get; set; }
        public string index_key_column_names { get; set; }

        #endregion

        #region Override Methods

        public override string ToString()
        {
            return base.ToString() + ",\t" +
                index_name + ",\t" +
                index_is_unique.ToString() + ",\t" +
                index_key_column_names;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            GenericIndex index2 = (GenericIndex)obj;
            return base.Equals(obj) &&
                index_name == index2.index_name &&
                index_is_unique == index2.index_is_unique &&
                index_key_column_names == index2.index_key_column_names;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^
                index_name.GetHashCode() ^
                index_is_unique.GetHashCode() ^
                index_key_column_names.GetHashCode();
        }

        #endregion

    }
}

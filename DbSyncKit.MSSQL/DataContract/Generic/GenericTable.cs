using System.Data;

namespace DbSyncKit.MSSQL.DataContract.Generic
{
    public class GenericTable : Schema
    {
        #region Constructor

        public GenericTable(DataRow tableInfo) : base(tableInfo)
        {
            if (tableInfo.IsNull("table_name"))
                throw new Exception("table_name cannot be null.");
            table_name = tableInfo.Field<string>("table_name")!;

        }

        #endregion

        #region Properties

        public string table_name { get; set; }

        #endregion

        #region Override Methods

        public override string ToString()
        {
            return base.ToString() + ",\t" +
                table_name;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            GenericTable table2 = (GenericTable)obj;
            return base.Equals(obj) &&
                table_name == table2.table_name;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^
                table_name.GetHashCode();
        }

        #endregion

    }
}

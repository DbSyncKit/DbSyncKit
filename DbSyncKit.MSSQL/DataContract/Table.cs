using System.Data;

namespace DbSyncKit.MSSQL.DataContract
{
    public class Table : Generic.GenericTable
    {
        #region Constructor

        public Table(DataRow tableInfo) : base(tableInfo)
        {
            type = tableInfo.Field<string>("type")!;

            if (tableInfo.IsNull("create_date")) throw new Exception("create_date cannot be null");
            create_date = tableInfo.Field<DateTime>("create_date");

            if (tableInfo.IsNull("modify_date")) throw new Exception("modify_date cannot be null");
            modify_date = tableInfo.Field<DateTime>("modify_date");

            if (tableInfo.IsNull("max_column_id_used")) throw new Exception("max_column_id_used cannot be null");
            max_column_id_used = tableInfo.Field<int>("max_column_id_used");

            if (tableInfo.IsNull("uses_ansi_nulls")) throw new Exception("uses_ansi_nulls cannot be null");
            uses_ansi_nulls = tableInfo.Field<bool>("uses_ansi_nulls");

        }

        #endregion

        #region Properties

        public string type { get; set; }
        public DateTime create_date { get; set; }
        public DateTime modify_date { get; set; }
        public int max_column_id_used { get; set; }
        public bool uses_ansi_nulls { get; set; }

        #endregion

        #region Override Methods

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object? table2)
        {
            if (table2 == null || GetType() != table2.GetType())
                return false;

            Table table_SQLServer2 = (Table)table2;
            return base.Equals(table2) &&
                uses_ansi_nulls == table_SQLServer2.uses_ansi_nulls;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^
                uses_ansi_nulls.GetHashCode();
        }

        #endregion
    }
}

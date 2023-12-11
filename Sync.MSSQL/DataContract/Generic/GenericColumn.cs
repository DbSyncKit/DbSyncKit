using System.Data;

namespace DbSyncKit.MSSQL.DataContract.Generic
{
    public class GenericColumn : GenericTable
    {
        #region Constructor

        public GenericColumn(DataRow columnInfo) : base(columnInfo)
        {
            if (columnInfo.IsNull("column_name"))
                throw new Exception("column_name cannot be null.");
            column_name = columnInfo.Field<string>("column_name")!;

            if (columnInfo.IsNull("column_id"))
                throw new Exception("column_id cannot be null.");
            column_ordinal_position = columnInfo.Field<int>("column_id");

            column_is_nullable = columnInfo.Field<bool?>("is_nullable");

            if (columnInfo.IsNull("user_type_name"))
                throw new Exception("user_type_name cannot be null.");
            column_type = columnInfo.Field<string>("user_type_name")!;

        }

        #endregion

        #region Properties

        public int column_ordinal_position { get; set; }
        public string column_name { get; set; }
        public string column_type { get; set; }
        public bool? column_is_nullable { get; set; }

        #endregion

        #region Override Methods

        public override string ToString()
        {
            return base.ToString() + ",\t" +
                column_ordinal_position.ToString() + ",\t" +
                column_name + ",\t" +
                column_type + ",\t" +
                column_is_nullable;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            GenericColumn column2 = (GenericColumn)obj;

            return base.Equals(obj) &&
                column_name == column2.column_name &&
                column_type == column2.column_type &&
                column_ordinal_position == column2.column_ordinal_position &&
                column_is_nullable == column2.column_is_nullable;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^
                column_name.GetHashCode() ^
                column_type.GetHashCode() ^
                column_ordinal_position.GetHashCode() ^
                column_is_nullable.GetHashCode();
        }

        #endregion

    }
}

using System.Data;

namespace DbSyncKit.MSSQL.DataContract
{
    public class ForeignKey : Generic.GenericConstraint
    {
        #region Constructor

        public ForeignKey(DataRow foreignKeyInfo) : base(foreignKeyInfo)
        {
            if (foreignKeyInfo.IsNull("column_names")) throw new Exception("column_names cannot be null.");
            column_names = foreignKeyInfo.Field<string>("column_names")!;

            if (foreignKeyInfo.IsNull("referenced_table_schema_name")) throw new Exception("referenced_table_schema_name cannot be null.");
            referenced_table_schema_name = foreignKeyInfo.Field<string>("referenced_table_schema_name")!;

            if (foreignKeyInfo.IsNull("referenced_table_name")) throw new Exception("referenced_table_name cannot be null.");
            referenced_table_name = foreignKeyInfo.Field<string>("referenced_table_name")!;

            referenced_unique_constraint_schema_name = foreignKeyInfo.Field<string>("referenced_unique_constraint_schema_name");

            referenced_unique_constraint_name = foreignKeyInfo.Field<string>("referenced_unique_constraint_name");

            if (foreignKeyInfo.IsNull("referenced_column_names")) throw new Exception("referenced_column_names cannot be null.");
            referenced_column_names = foreignKeyInfo.Field<string>("referenced_column_names")!;

            if (foreignKeyInfo.IsNull("update_rule")) throw new Exception("update_rule cannot be null.");
            update_rule = foreignKeyInfo.Field<string>("update_rule")!;

            if (foreignKeyInfo.IsNull("delete_rule")) throw new Exception("delete_rule cannot be null.");
            delete_rule = foreignKeyInfo.Field<string>("delete_rule")!;

        }

        #endregion

        #region Properties

        public string column_names { get; set; }
        public string referenced_table_schema_name { get; set; }
        public string referenced_table_name { get; set; }
        public string? referenced_unique_constraint_schema_name { get; set; }
        public string? referenced_unique_constraint_name { get; set; }
        public string referenced_column_names { get; set; }
        public string update_rule { get; set; }
        public string delete_rule { get; set; }

        #endregion

        #region Override Methods

        public override string ToString()
        {
            return base.ToString() + ",\t" + column_names + ",\t" +
                referenced_table_schema_name + ",\t" +
                referenced_table_name + ",\t" +
                referenced_column_names + ",\t" +
                update_rule + "\t" +
                delete_rule;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not ForeignKey)
                return false;

            ForeignKey foreignKey2 = (ForeignKey)obj;
            return base.Equals(obj) &&
                column_names == foreignKey2.column_names &&
                referenced_table_schema_name == foreignKey2.referenced_table_schema_name &&
                referenced_table_name == foreignKey2.referenced_table_name &&
                referenced_column_names == foreignKey2.referenced_column_names &&
                update_rule == foreignKey2.update_rule &&
                delete_rule == foreignKey2.delete_rule;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^
                column_names.GetHashCode() ^
                referenced_table_schema_name.GetHashCode() ^
                referenced_table_name.GetHashCode() ^
                referenced_column_names.GetHashCode() ^
                update_rule.GetHashCode() ^
                delete_rule.GetHashCode();
        }

        #endregion        
    }
}

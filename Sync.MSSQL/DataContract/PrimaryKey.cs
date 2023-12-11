using System.Data;

namespace DbSyncKit.MSSQL.DataContract
{
    public class PrimaryKey : Generic.GenericConstraint
    {
        #region Constructor

        public PrimaryKey(DataRow primaryKeyInfo) : base(primaryKeyInfo)
        {
            if (primaryKeyInfo.IsNull("primary_key_columns")) throw new Exception("primary_key_columns cannot be null.");
            primary_key_columns = primaryKeyInfo.Field<string>("primary_key_columns")!;
        }

        #endregion

        #region Properties

        public string primary_key_columns { get; set; }

        #endregion

        #region Override Methods

        public override string ToString()
        {
            return base.ToString() + "\t" +
                primary_key_columns;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not PrimaryKey)
                return false;

            PrimaryKey primaryKey2 = (PrimaryKey)obj;
            return base.Equals(obj) &&
                primary_key_columns == primaryKey2.primary_key_columns;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^
                primary_key_columns.GetHashCode();
        }

        #endregion
    }
}

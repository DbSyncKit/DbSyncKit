using System.Data;

namespace DbSyncKit.MSSQL.DataContract
{
    public class UniqueKey : Generic.GenericConstraint
    {
        #region Constructor

        public UniqueKey(DataRow uniqueKeyInfo) : base(uniqueKeyInfo)
        {
            if (uniqueKeyInfo.IsNull("column_names"))
                throw new Exception("column_names cannot be null.");
            column_names = uniqueKeyInfo.Field<string>("column_names")!;
        }

        #endregion

        #region Properties

        public string column_names { get; set; }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return base.ToString() + ",\t" +
                column_names;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            UniqueKey uniqueKey2 = (UniqueKey)obj;
            return base.Equals(obj) &&
                column_names == uniqueKey2.column_names;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^
                column_names.GetHashCode();
        }

        #endregion
    }
}

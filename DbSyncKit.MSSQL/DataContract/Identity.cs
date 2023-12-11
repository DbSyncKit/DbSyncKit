using System.Data;

namespace DbSyncKit.MSSQL.DataContract
{
    public class Identity : Generic.GenericColumn
    {
        #region Constructor

        public Identity(DataRow identity_col_info) : base(identity_col_info)
        {
            identity_seed_value = identity_col_info.IsNull("identity_seed_value") ? null : Convert.ToInt64(identity_col_info["identity_seed_value"]);

            identity_increment_value = identity_col_info.IsNull("identity_increment_value") ? null : Convert.ToInt64(identity_col_info["identity_increment_value"]);

            identity_last_value = identity_col_info.IsNull("identity_last_value") ? null : Convert.ToInt64(identity_col_info["identity_last_value"]);

        }

        #endregion

        #region Properties

        public long? identity_seed_value { get; set; }
        public long? identity_increment_value { get; set; }
        public long? identity_last_value { get; set; }

        #endregion

        #region Override Methods

        public override string ToString()
        {
            return base.ToString() + ",\t" +
                identity_seed_value + ",\t" +
                identity_increment_value;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Identity identity2 = (Identity)obj;
            return base.Equals(obj) &&
                identity_seed_value == identity2.identity_seed_value &&
                identity_increment_value == identity2.identity_increment_value;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^
                identity_seed_value.GetHashCode() ^
                identity_increment_value.GetHashCode();
        }

        #endregion
    }
}

using System.Data;

namespace DbSyncKit.MSSQL.DataContract
{
    public class DefaultConstraint : Generic.GenericConstraint
    {
        #region Constructor

        public DefaultConstraint(DataRow defaultConstraintInfo) : base(defaultConstraintInfo)
        {
            if (defaultConstraintInfo.IsNull("column_name"))
                throw new Exception("column_name cannot be null.");
            column_name = defaultConstraintInfo.Field<string>("column_name")!;

            if (defaultConstraintInfo.IsNull("constraint_definition"))
                throw new Exception("constraint_definition cannot be null.");
            constraint_definition = defaultConstraintInfo.Field<string>("constraint_definition")!;

        }

        #endregion

        #region Properties

        public string column_name { get; set; }
        public string constraint_definition { get; set; }

        #endregion

        #region Override Methods

        public override string ToString()
        {
            return base.ToString() + ",\t" +
                column_name + ",\t" +
                constraint_definition;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            DefaultConstraint defaultConstraint2 = (DefaultConstraint)obj;
            return base.Equals(obj) &&
                column_name == defaultConstraint2.column_name &&
                constraint_definition == defaultConstraint2.constraint_definition;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^
                column_name.GetHashCode() ^
                constraint_definition.GetHashCode();
        }

        #endregion
    }
}

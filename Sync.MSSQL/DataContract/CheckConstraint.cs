using System.Data;

namespace DbSyncKit.MSSQL.DataContract
{
    public class CheckConstraint : Generic.GenericConstraint
    {
        #region Constructor

        public CheckConstraint(DataRow checkConstraintInfo) : base(checkConstraintInfo)
        {
            if (checkConstraintInfo == null)
                throw new ArgumentNullException("checkConstraintInfo");

            if (checkConstraintInfo.IsNull("constraint_definition"))
                throw new Exception("constraint_definition cannot be null.");
            constraint_definition = checkConstraintInfo.Field<string>("constraint_definition")!;

        }

        #endregion

        #region Properties

        public string constraint_definition { get; set; }

        #endregion

        #region Override Methods

        public override string ToString()
        {
            return base.ToString() + ",\t" +
                constraint_definition;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            CheckConstraint checkConstraint2 = (CheckConstraint)obj;
            return base.Equals(obj) &&
                constraint_definition == checkConstraint2.constraint_definition;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^
                constraint_definition.GetHashCode();
        }

        #endregion
    }
}

using System.Data;

namespace DbSyncKit.MSSQL.DataContract.Generic
{
    public class GenericConstraint : GenericTable
    {
        #region Constructor

        public GenericConstraint(DataRow constraintInfo) : base(constraintInfo)
        {
            if (constraintInfo == null) throw new ArgumentNullException("constraintInfo cannot be null.");

            if (constraintInfo.IsNull("constraint_schema_name")) throw new Exception("constraint_schema_name cannot be null.");
            constraint_schema_name = constraintInfo.Field<string>("constraint_schema_name")!;

            if (constraintInfo.IsNull("constraint_name")) throw new Exception("constraint_name cannot be null.");
            constraint_name = constraintInfo.Field<string>("constraint_name")!;

            if (constraintInfo.IsNull("constraint_type")) throw new Exception("constraint_type cannot be null.");
            constraint_type = constraintInfo.Field<string>("constraint_type")!;

        }

        #endregion

        #region Properties

        public string constraint_schema_name { get; set; }
        public string constraint_name { get; set; }
        public string constraint_type { get; set; }

        #endregion

        #region Override Methods

        public override string ToString()
        {
            return base.ToString() + ",\t" +
                constraint_schema_name + ",\t" +
                constraint_name + ",\t" +
                constraint_type;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^
                constraint_schema_name.GetHashCode() ^
                constraint_name.GetHashCode() ^
                constraint_type.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            GenericConstraint constraint2 = (GenericConstraint)obj;
            return base.Equals(obj) &&
                constraint_schema_name == constraint2.constraint_schema_name &&
                constraint_name == constraint2.constraint_name &&
                constraint_type == constraint2.constraint_type;
        }

        #endregion

    }
}

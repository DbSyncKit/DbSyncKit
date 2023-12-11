using DbSyncKit.DB.Interface;
using System.Data;

namespace DbSyncKit.MSSQL.DataContract
{
    public class UserDataType : IDataContractComparer
    {
        #region Constructor

        public UserDataType(DataRow dataTypeInfo)
        {
            if (dataTypeInfo == null) throw new ArgumentNullException("dataTypeInfo");

            if (dataTypeInfo.IsNull("name")) throw new Exception("name cannot be null.");
            name = dataTypeInfo.Field<string>("name")!;

            if (dataTypeInfo.IsNull("system_type_id")) throw new Exception("system_type_id cannot be null.");
            system_type_id = dataTypeInfo.Field<byte>("system_type_id");

            if (dataTypeInfo.IsNull("user_type_id")) throw new Exception("user_type_id cannot be null.");
            user_type_id = dataTypeInfo.Field<int>("user_type_id");

            if (dataTypeInfo.IsNull("schema_id")) throw new Exception("schema_id cannot be null.");
            schema_id = dataTypeInfo.Field<int>("schema_id");

            principal_id = dataTypeInfo.Field<int?>("principal_id");

            if (dataTypeInfo.IsNull("max_length")) throw new Exception("max_length cannot be null.");
            max_length = dataTypeInfo.Field<short>("max_length");

            if (dataTypeInfo.IsNull("precision")) throw new Exception("precision cannot be null.");
            precision = dataTypeInfo.Field<byte>("precision");

            if (dataTypeInfo.IsNull("scale")) throw new Exception("scale cannot be null.");
            scale = dataTypeInfo.Field<byte>("scale");

            collation_name = dataTypeInfo.Field<string?>("collation_name");

            is_nullable = dataTypeInfo.Field<bool?>("is_nullable");

            if (dataTypeInfo.IsNull("is_user_defined")) throw new Exception("is_user_defined cannot be null.");
            is_user_defined = dataTypeInfo.Field<bool>("is_user_defined");

            if (dataTypeInfo.IsNull("is_assembly_type")) throw new Exception("is_assembly_type cannot be null.");
            is_assembly_type = dataTypeInfo.Field<bool>("is_assembly_type");

            if (dataTypeInfo.IsNull("default_object_id")) throw new Exception("default_object_id cannot be null.");
            default_object_id = dataTypeInfo.Field<int>("default_object_id");

            if (dataTypeInfo.IsNull("rule_object_id")) throw new Exception("rule_object_id cannot be null.");
            rule_object_id = dataTypeInfo.Field<int>("rule_object_id");

            if (dataTypeInfo.IsNull("is_table_type")) throw new Exception("is_table_type cannot be null.");
            is_table_type = dataTypeInfo.Field<bool>("is_table_type");

        }

        #endregion

        #region Properties

        public string name { get; set; }
        public int system_type_id { get; set; }
        public int user_type_id { get; set; }
        public int schema_id { get; set; }
        public int? principal_id { get; set; }
        public short max_length { get; set; }
        public short precision { get; set; }
        public short scale { get; set; }
        public string? collation_name { get; set; }
        public bool? is_nullable { get; set; }
        public bool is_user_defined { get; set; }
        public bool is_assembly_type { get; set; }
        public int default_object_id { get; set; }
        public int rule_object_id { get; set; }
        public bool is_table_type { get; set; }

        #endregion

        #region Override Methods

        public override string ToString()
        {
            return name.ToString();
        }

        public override bool Equals(object? dataType2)
        {
            if (dataType2 == null || GetType() != dataType2.GetType())
                return false;

            UserDataType userDataType2 = (UserDataType)dataType2;
            return name == userDataType2.name &&
                schema_id == userDataType2.schema_id &&
                system_type_id == userDataType2.system_type_id &&
                user_type_id == userDataType2.user_type_id &&
                max_length == userDataType2.max_length &&
                precision == userDataType2.precision &&
                scale == userDataType2.scale &&
                is_nullable == userDataType2.is_nullable;
        }

        public override int GetHashCode()
        {
            return name.GetHashCode() ^
                schema_id.GetHashCode() ^
                system_type_id.GetHashCode() ^
                user_type_id.GetHashCode() ^
                max_length.GetHashCode() ^
                precision.GetHashCode() ^
                scale.GetHashCode() ^
                is_nullable.GetHashCode();
        }

        #endregion
    }
}

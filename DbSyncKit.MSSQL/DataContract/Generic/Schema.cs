using DbSyncKit.DB.Interface;
using System.Data;

namespace DbSyncKit.MSSQL.DataContract.Generic
{
    public class Schema : IDataContractComparer
    {
        #region Constructor

        public Schema(DataRow schemaInfo)
        {
            if (schemaInfo == null) throw new ArgumentNullException("schemaInfo");

            if (schemaInfo.IsNull("schema_name")) throw new Exception("schema_name cannot be null.");
            schema_name = schemaInfo.Field<string>("schema_name")!;

        }

        #endregion

        #region Properties

        public string schema_name { get; set; }

        #endregion

        #region Overrride Methods

        public override string ToString()
        {
            return schema_name;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Schema schema2 = (Schema)obj;
            return schema_name.Equals(schema2.schema_name);
        }

        public override int GetHashCode()
        {
            return schema_name.GetHashCode();
        }

        #endregion

    }
}

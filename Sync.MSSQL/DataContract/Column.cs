using System.Data;

namespace DbSyncKit.MSSQL.DataContract
{
    public class Column : Generic.GenericColumn
    {
        #region Constructor

        public Column(DataRow columnInfo) : base(columnInfo)
        {
            if (columnInfo.IsNull("system_type_id")) throw new Exception("system_type_id cannot be null.");
            system_type_id = columnInfo.Field<byte>("system_type_id");

            if (columnInfo.IsNull("system_type_name")) throw new Exception("system_type_name cannot be null.");
            system_type_name = columnInfo.Field<string>("system_type_name")!;

            if (columnInfo.IsNull("user_type_id")) throw new Exception("user_type_id cannot be null.");
            user_type_id = columnInfo.Field<int>("user_type_id");

            if (columnInfo.IsNull("max_length")) throw new Exception("max_length cannot be null.");
            max_length = columnInfo.Field<short>("max_length");

            if (columnInfo.IsNull("precision")) throw new Exception("precision cannot be null.");
            precision = columnInfo.Field<byte>("precision");

            if (columnInfo.IsNull("scale")) throw new Exception("scale cannot be null.");
            scale = columnInfo.Field<byte>("scale");

            //if (columnInfo.IsNull("collation_name")) throw new Exception("collation_name cannot be null.");
            collation_name = columnInfo.Field<string>("collation_name");

            //if (columnInfo.IsNull("is_nullable")) throw new Exception("is_nullable cannot be null.");

            if (columnInfo.IsNull("is_ansi_padded")) throw new Exception("is_ansi_padded cannot be null.");
            is_ansi_padded = columnInfo.Field<bool>("is_ansi_padded");

            if (columnInfo.IsNull("is_rowguidcol")) throw new Exception("is_rowguidcol cannot be null.");
            is_rowguidcol = columnInfo.Field<bool>("is_rowguidcol");

            if (columnInfo.IsNull("is_identity")) throw new Exception("is_identity cannot be null.");
            is_identity = columnInfo.Field<bool>("is_identity");

            if (columnInfo.IsNull("is_computed")) throw new Exception("is_computed cannot be null.");
            is_computed = columnInfo.Field<bool>("is_computed");

            if (columnInfo.IsNull("is_filestream")) throw new Exception("is_filestream cannot be null.");
            is_filestream = columnInfo.Field<bool>("is_filestream");

            //if (columnInfo.IsNull("is_replicated")) throw new Exception("is_replicated cannot be null.");
            is_replicated = columnInfo.Field<bool?>("is_replicated");

            //if (columnInfo.IsNull("is_non_sql_subscribed")) throw new Exception("is_non_sql_subscribed cannot be null.");
            is_non_sql_subscribed = columnInfo.Field<bool?>("is_non_sql_subscribed");

            //if (columnInfo.IsNull("is_merge_published")) throw new Exception("is_merge_published cannot be null.");
            is_merge_published = columnInfo.Field<bool?>("is_merge_published");

            //if (columnInfo.IsNull("is_dts_replicated")) throw new Exception("is_dts_replicated cannot be null.");
            is_dts_replicated = columnInfo.Field<bool?>("is_dts_replicated");

            if (columnInfo.IsNull("is_xml_document")) throw new Exception("is_xml_document cannot be null.");
            is_xml_document = columnInfo.Field<bool>("is_xml_document");

            if (columnInfo.IsNull("xml_collection_id")) throw new Exception("xml_collection_id cannot be null.");
            xml_collection_id = columnInfo.Field<int>("xml_collection_id");

            if (columnInfo.IsNull("default_object_id")) throw new Exception("default_object_id cannot be null.");
            default_object_id = columnInfo.Field<int>("default_object_id");

            if (columnInfo.IsNull("rule_object_id")) throw new Exception("rule_object_id cannot be null.");
            rule_object_id = columnInfo.Field<int>("rule_object_id");

            //if (columnInfo.IsNull("is_sparse")) throw new Exception("is_sparse cannot be null.");
            is_sparse = columnInfo.Field<bool?>("is_sparse");

            //if (columnInfo.IsNull("is_column_set")) throw new Exception("is_column_set cannot be null.");
            is_column_set = columnInfo.Field<bool?>("is_column_set");

            //if (columnInfo.IsNull("generated_always_type")) throw new Exception("generated_always_type cannot be null.");
            generated_always_type = columnInfo.Field<byte?>("generated_always_type");

            //if (columnInfo.IsNull("generated_always_type_desc")) throw new Exception("generated_always_type_desc cannot be null.");
            generated_always_type_desc = columnInfo.Field<string?>("generated_always_type_desc");

            //if (columnInfo.IsNull("encryption_type")) throw new Exception("encryption_type cannot be null.");
            encryption_type = columnInfo.Field<int?>("encryption_type");

            //if (columnInfo.IsNull("encryption_type_desc")) throw new Exception("encryption_type_desc cannot be null.");
            encryption_type_desc = columnInfo.Field<string?>("encryption_type_desc");

            //if (columnInfo.IsNull("encryption_algorithm_name")) throw new Exception("encryption_algorithm_name cannot be null.");
            encryption_algorithm_name = columnInfo.Field<string?>("encryption_algorithm_name");

            //if (columnInfo.IsNull("column_encryption_key_id")) throw new Exception("column_encryption_key_id cannot be null.");
            column_encryption_key_id = columnInfo.Field<int?>("column_encryption_key_id");

            //if (columnInfo.IsNull("column_encryption_key_database_name")) throw new Exception("column_encryption_key_database_name cannot be null.");
            column_encryption_key_database_name = columnInfo.Field<string?>("column_encryption_key_database_name");

            //if (columnInfo.IsNull("is_hidden")) throw new Exception("is_hidden cannot be null.");
            is_hidden = columnInfo.Field<bool?>("is_hidden");

            //if (columnInfo.IsNull("is_masked")) throw new Exception("is_masked cannot be null.");
            is_masked = columnInfo.Field<bool?>("is_masked");

        }

        #endregion

        #region Properties

        public int system_type_id { get; set; }
        public string system_type_name { get; set; }
        public int user_type_id { get; set; }
        public int user_type_name { get; set; }
        public short max_length { get; set; }
        public short precision { get; set; }
        public short scale { get; set; }
        public string? collation_name { get; set; }
        public bool is_ansi_padded { get; set; }
        public bool is_rowguidcol { get; set; }
        public bool is_identity { get; set; }
        public bool is_computed { get; set; }
        public bool is_filestream { get; set; }
        public bool? is_replicated { get; set; }
        public bool? is_non_sql_subscribed { get; set; }
        public bool? is_merge_published { get; set; }
        public bool? is_dts_replicated { get; set; }
        public bool is_xml_document { get; set; }
        public int xml_collection_id { get; set; }
        public int default_object_id { get; set; }
        public int rule_object_id { get; set; }
        public bool? is_sparse { get; set; }
        public bool? is_column_set { get; set; }
        public int? generated_always_type { get; set; }
        public string? generated_always_type_desc { get; set; }
        public int? encryption_type { get; set; }
        public string? encryption_type_desc { get; set; }
        public string? encryption_algorithm_name { get; set; }
        public int? column_encryption_key_id { get; set; }
        public string? column_encryption_key_database_name { get; set; }
        public bool? is_hidden { get; set; }
        public bool? is_masked { get; set; }

        #endregion

        #region Override Methods

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}

using DbSyncKit.Templates.Interface;
using HandlebarsDotNet;

namespace DbSyncKit.Templates.MSSQL
{
    public class QueryTemplates: IQueryTemplates
    {
        #region Public Properties
        public HandlebarsTemplate<object, object> SELECT_QUERY => _selectQueryTemplate.Value;
        public HandlebarsTemplate<object, object> INSERT_QUERY => _insertQueryTemplate.Value;
        public HandlebarsTemplate<object, object> UPDATE_QUERY => _updateQueryTemplate.Value;
        public HandlebarsTemplate<object, object> DELETE_QUERY => _deleteQueryTemplate.Value;
        public HandlebarsTemplate<object, object> COMMENT_QUERY => _commentQueryTemplate.Value;

        #endregion

        #region Private Properties

        private static Lazy<HandlebarsTemplate<object, object>> _selectQueryTemplate = new Lazy<HandlebarsTemplate<object, object>>(CreateSelectQueryTemplate);
        private static Lazy<HandlebarsTemplate<object, object>> _insertQueryTemplate = new Lazy<HandlebarsTemplate<object, object>>(CreateInsertQueryTemplate);
        private readonly Lazy<HandlebarsTemplate<object, object>> _updateQueryTemplate = new Lazy<HandlebarsTemplate<object, object>>(CreateUpdateQueryTemplate);
        private readonly Lazy<HandlebarsTemplate<object, object>> _deleteQueryTemplate = new Lazy<HandlebarsTemplate<object, object>>(CreateDeleteQueryTemplate);
        private readonly Lazy<HandlebarsTemplate<object, object>> _commentQueryTemplate = new Lazy<HandlebarsTemplate<object, object>>(CreateCommentQueryTemplate);
        #endregion

        #region Templates

        private static HandlebarsTemplate<object, object> CreateSelectQueryTemplate()
        {

            var str = @" SELECT {{Join Columns}} FROM {{Schema}}.{{TableName}} ";

            return Handlebars.Compile(str);
        }

        private static HandlebarsTemplate<object, object> CreateInsertQueryTemplate()
        {
            var str = @" 
IF NOT EXISTS (SELECT 1 FROM {{Schema}}.{{TableName}} WHERE {{And Where}})
BEGIN
{{#if IsIdentityInsert}}
    SET IDENTITY_INSERT {{Schema}}.{{TableName}} ON
{{/if}}
    INSERT INTO {{Schema}}.{{TableName}} ({{Join Columns}}) VALUES ({{Join Values}})
{{#if IsIdentityInsert}}
    SET IDENTITY_INSERT {{Schema}}.{{TableName}} OFF
{{/if}}
END ";

            return Handlebars.Compile(str);
        }

        private static HandlebarsTemplate<object, object> CreateUpdateQueryTemplate()
        {
            var str = @"
IF EXISTS (SELECT 1 FROM {{Schema}}.{{TableName}} WHERE {{And Where}})
BEGIN
    UPDATE {{Schema}}.{{TableName}} SET {{Join Set}} WHERE {{And Where}}
END";
            return Handlebars.Compile(str);
        }

        private static HandlebarsTemplate<object, object> CreateDeleteQueryTemplate()
        {
            var str = @"
IF EXISTS (SELECT 1 FROM {{Schema}}.{{TableName}} WHERE {{And Where}})
BEGIN
    DELETE FROM {{Schema}}.{{TableName}} WHERE {{And Where}}
END";
            return Handlebars.Compile(str);
        }

        private static HandlebarsTemplate<object, object> CreateCommentQueryTemplate()
        {
            var str = @"
{{#if isMultiLine}}
/* 
{{comment}}
*/
{{else}}
-- {{comment}}
{{/if}}";

            return Handlebars.Compile(str);
        }
        #endregion



    }
}

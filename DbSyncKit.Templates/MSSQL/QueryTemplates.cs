using DbSyncKit.Templates.Interface;
using DotLiquid;

namespace DbSyncKit.Templates.MSSQL
{
    public class QueryTemplates: IQueryTemplates
    {
        #region Public Properties
        public Template SELECT_QUERY => _selectQueryTemplate.Value;
        public Template INSERT_QUERY => _insertQueryTemplate.Value;
        public Template UPDATE_QUERY => _updateQueryTemplate.Value;
        public Template DELETE_QUERY => _deleteQueryTemplate.Value;
        public Template COMMENT_QUERY => _commentQueryTemplate.Value;

        #endregion

        #region Private Properties

        private static readonly Lazy<Template> _selectQueryTemplate = new Lazy<Template>(CreateSelectQueryTemplate);
        private static readonly Lazy<Template> _insertQueryTemplate = new Lazy<Template>(CreateInsertQueryTemplate);
        private static readonly Lazy<Template> _updateQueryTemplate = new Lazy<Template>(CreateUpdateQueryTemplate);
        private static readonly Lazy<Template> _deleteQueryTemplate = new Lazy<Template>(CreateDeleteQueryTemplate);
        private static readonly Lazy<Template> _commentQueryTemplate = new Lazy<Template>(CreateCommentQueryTemplate);
        #endregion

        #region Templates

        private static Template CreateSelectQueryTemplate()
        {
            var str = @" SELECT {{ Columns | join: ', ' }} FROM {{Schema}}.{{TableName}} ";

            return Template.Parse(str);
        }

        private static Template CreateInsertQueryTemplate()
        {
            var str = @" 
IF NOT EXISTS (SELECT 1 FROM {{Schema}}.{{TableName}} WHERE {{ Where | join: ' AND ' }})
BEGIN
{% if IsIdentityInsert %}
    SET IDENTITY_INSERT {{Schema}}.{{TableName}} ON
{% endif %}
    INSERT INTO {{Schema}}.{{TableName}} ({{ Columns | join: ', ' }}) VALUES ({{ Values | join: ', ' }})
{% if IsIdentityInsert %}
    SET IDENTITY_INSERT {{Schema}}.{{TableName}} OFF
{% endif %}
END ";

            return Template.Parse(str);
        }

        private static Template CreateUpdateQueryTemplate()
        {
            var str = @"
IF EXISTS (SELECT 1 FROM {{Schema}}.{{TableName}} WHERE {{ Where | join: ' AND ' }})
BEGIN
    UPDATE {{Schema}}.{{TableName}} SET {{ Set | join: ', ' }} WHERE {{ Where | join: ' AND ' }}
END";
            return Template.Parse(str);
        }

        private static Template CreateDeleteQueryTemplate()
        {
            var str = @"
IF EXISTS (SELECT 1 FROM {{Schema}}.{{TableName}} WHERE {{ Where | join: ' AND ' }})
BEGIN
    DELETE FROM {{Schema}}.{{TableName}} WHERE {{ Where | join: ' AND ' }}
END";
            return Template.Parse(str);
        }

        private static Template CreateCommentQueryTemplate()
        {
            var str = @"
{% if isMultiLine %}
/* 
{{comment}}
*/
{% else %}
-- {{comment}}
{% endif %}";

            return Template.Parse(str);
        }
        #endregion



    }
}

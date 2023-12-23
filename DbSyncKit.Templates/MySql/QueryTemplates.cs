using DbSyncKit.Templates.Interface;
using DotLiquid;

namespace DbSyncKit.Templates.MySql
{
    public class QueryTemplates : IQueryTemplates
    {
        #region Public Properties
        public Template SELECT_QUERY => _selectQueryTemplate.Value;
        public Template INSERT_QUERY => _insertQueryTemplate.Value;
        public Template UPDATE_QUERY => _updateQueryTemplate.Value;
        public Template DELETE_QUERY => _deleteQueryTemplate.Value;
        public Template COMMENT_QUERY => _commentQueryTemplate.Value;

        #endregion

        #region Private Properties

        private static Lazy<Template> _selectQueryTemplate = new Lazy<Template>(CreateSelectQueryTemplate);
        private static Lazy<Template> _insertQueryTemplate = new Lazy<Template>(CreateInsertQueryTemplate);
        private readonly Lazy<Template> _updateQueryTemplate = new Lazy<Template>(CreateUpdateQueryTemplate);
        private readonly Lazy<Template> _deleteQueryTemplate = new Lazy<Template>(CreateDeleteQueryTemplate);
        private readonly Lazy<Template> _commentQueryTemplate = new Lazy<Template>(CreateCommentQueryTemplate);
        #endregion

        #region Templates

        private static Template CreateSelectQueryTemplate()
        {
            var str = @" SELECT {{ Columns | join: ', ' }} FROM {{ TableName }}; ";

            return Template.Parse(str);
        }

        private static Template CreateInsertQueryTemplate()
        {
            var str = @" 
INSERT INTO `{{ TableName }}` ({{ Columns | join: ', ' }}) SELECT {{ Values | join: ', ' }} FROM DUAL WHERE NOT EXISTS ( SELECT 1 FROM `{{ TableName }}` WHERE {{ Where | join: ' AND '  }} )
";

            return Template.Parse(str);
        }

        private static Template CreateUpdateQueryTemplate()
        {
            var str = @"
UPDATE `{{ TableName }}` SET {{ Set | join: ', ' }} WHERE {{ Where | join: ' AND '  }} LIMIT 1; 
";
            return Template.Parse(str);
        }

        private static Template CreateDeleteQueryTemplate()
        {
            var str = @"
DELETE FROM {{ TableName }} WHERE {{ Where | join: ' AND '  }} LIMIT 1;  ";

            return Template.Parse(str);
        }

        private static Template CreateCommentQueryTemplate()
        {
            var str = @"{% unless isMultiLine %} -- {{ comment }} {% else %}
/* 
{{ comment }}
*/
-- {{ comment }} {% endunless %}";

            return Template.Parse(str);
        }
        #endregion



    }
}

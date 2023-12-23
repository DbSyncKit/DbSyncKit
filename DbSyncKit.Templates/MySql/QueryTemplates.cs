using DbSyncKit.Templates.Interface;
using Fluid;

namespace DbSyncKit.Templates.MySql
{
    public class QueryTemplates : IQueryTemplates
    {
        #region Public Properties
        public IFluidTemplate SELECT_QUERY => _selectQueryTemplate.Value;
        public IFluidTemplate INSERT_QUERY => _insertQueryTemplate.Value;
        public IFluidTemplate UPDATE_QUERY => _updateQueryTemplate.Value;
        public IFluidTemplate DELETE_QUERY => _deleteQueryTemplate.Value;
        public IFluidTemplate COMMENT_QUERY => _commentQueryTemplate.Value;

        #endregion

        #region Private Properties

        private static Lazy<IFluidTemplate> _selectQueryTemplate = new Lazy<IFluidTemplate>(CreateSelectQueryTemplate);
        private static Lazy<IFluidTemplate> _insertQueryTemplate = new Lazy<IFluidTemplate>(CreateInsertQueryTemplate);
        private readonly Lazy<IFluidTemplate> _updateQueryTemplate = new Lazy<IFluidTemplate>(CreateUpdateQueryTemplate);
        private readonly Lazy<IFluidTemplate> _deleteQueryTemplate = new Lazy<IFluidTemplate>(CreateDeleteQueryTemplate);
        private readonly Lazy<IFluidTemplate> _commentQueryTemplate = new Lazy<IFluidTemplate>(CreateCommentQueryTemplate);
        private static readonly FluidParser parser = new FluidParser();
        #endregion

        #region Templates

        private static IFluidTemplate CreateSelectQueryTemplate()
        {
            var str = @" SELECT {{ Columns | join: ', ' }} FROM {{ TableName }}; ";

            return parser.Parse(str);
        }

        private static IFluidTemplate CreateInsertQueryTemplate()
        {
            var str = @" 
INSERT INTO `{{ TableName }}` ({{ Columns | join: ', ' }}) SELECT {{ Values | join: ', ' }} FROM DUAL WHERE NOT EXISTS ( SELECT 1 FROM `{{ TableName }}` WHERE {{ Where | join: ' AND '  }} )
";

            return parser.Parse(str);
        }

        private static IFluidTemplate CreateUpdateQueryTemplate()
        {
            var str = @"
UPDATE `{{ TableName }}` SET {{ Set | join: ', ' }} WHERE {{ Where | join: ' AND '  }} LIMIT 1; 
";
            return parser.Parse(str);
        }

        private static IFluidTemplate CreateDeleteQueryTemplate()
        {
            var str = @"
DELETE FROM {{ TableName }} WHERE {{ Where | join: ' AND '  }} LIMIT 1;  ";

            return parser.Parse(str);
        }

        private static IFluidTemplate CreateCommentQueryTemplate()
        {
            var str = @"{% unless isMultiLine %} -- {{ comment }} {% else %}
/* 
{{ comment }}
*/
-- {{ comment }} {% endunless %}";

            return parser.Parse(str);
        }
        #endregion



    }
}

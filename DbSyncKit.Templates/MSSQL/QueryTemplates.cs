using DbSyncKit.Templates.Interface;
using Fluid;

namespace DbSyncKit.Templates.MSSQL
{
    public class QueryTemplates: IQueryTemplates
    {
        #region Public Properties
        public IFluidTemplate SELECT_QUERY => _selectQueryTemplate.Value;
        public IFluidTemplate INSERT_QUERY => _insertQueryTemplate.Value;
        public IFluidTemplate UPDATE_QUERY => _updateQueryTemplate.Value;
        public IFluidTemplate DELETE_QUERY => _deleteQueryTemplate.Value;
        public IFluidTemplate COMMENT_QUERY => _commentQueryTemplate.Value;

        #endregion

        #region Private Properties

        private static readonly Lazy<IFluidTemplate> _selectQueryTemplate = new Lazy<IFluidTemplate>(CreateSelectQueryTemplate);
        private static readonly Lazy<IFluidTemplate> _insertQueryTemplate = new Lazy<IFluidTemplate>(CreateInsertQueryTemplate);
        private static readonly Lazy<IFluidTemplate> _updateQueryTemplate = new Lazy<IFluidTemplate>(CreateUpdateQueryTemplate);
        private static readonly Lazy<IFluidTemplate> _deleteQueryTemplate = new Lazy<IFluidTemplate>(CreateDeleteQueryTemplate);
        private static readonly Lazy<IFluidTemplate> _commentQueryTemplate = new Lazy<IFluidTemplate>(CreateCommentQueryTemplate);
        private static readonly FluidParser parser = new FluidParser();
        #endregion

        #region Templates

        private static IFluidTemplate CreateSelectQueryTemplate()
        {
            var str = @" SELECT {{ Columns | join: ', ' }} FROM {{Schema}}.{{TableName}} ";

            return parser.Parse(str);
        }

        private static IFluidTemplate CreateInsertQueryTemplate()
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

            return parser.Parse(str);
        }

        private static IFluidTemplate CreateUpdateQueryTemplate()
        {
            var str = @"
IF EXISTS (SELECT 1 FROM {{Schema}}.{{TableName}} WHERE {{ Where | join: ' AND ' }})
BEGIN
    UPDATE {{Schema}}.{{TableName}} SET {{ Set | join: ', ' }} WHERE {{ Where | join: ' AND ' }}
END";
            return parser.Parse(str);
        }

        private static IFluidTemplate CreateDeleteQueryTemplate()
        {
            var str = @"
IF EXISTS (SELECT 1 FROM {{Schema}}.{{TableName}} WHERE {{ Where | join: ' AND ' }})
BEGIN
    DELETE FROM {{Schema}}.{{TableName}} WHERE {{ Where | join: ' AND ' }}
END";
            return parser.Parse(str);
        }

        private static IFluidTemplate CreateCommentQueryTemplate()
        {
            var str = @"
{% if isMultiLine %}
/* 
{{comment}}
*/
{% else %}
-- {{comment}}
{% endif %}";

            return parser.Parse(str);
        }
        #endregion



    }
}

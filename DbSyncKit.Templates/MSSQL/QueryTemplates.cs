using DbSyncKit.Templates.Interface;
using Fluid;

namespace DbSyncKit.Templates.MSSQL
{
    /// <summary>
    /// Implementation of <see cref="IQueryTemplates"/> for MSSQL database, providing templates for various SQL queries.
    /// </summary>
    public class QueryTemplates: IQueryTemplates
    {
        #region Public Properties

        /// <summary>
        /// Gets the template for a SELECT template.
        /// </summary>
        public IFluidTemplate SelectTemplate => _selectQueryTemplate.Value;

        /// <summary>
        /// Gets the template for an INSERT template.
        /// </summary>
        public IFluidTemplate InsertTemplate => _insertQueryTemplate.Value;

        /// <summary>
        /// Gets the template for an UPDATE template.
        /// </summary>
        public IFluidTemplate UpdateTemplate => _updateQueryTemplate.Value;

        /// <summary>
        /// Gets the template for a DELETE template.
        /// </summary>
        public IFluidTemplate DeleteTemplate => _deleteQueryTemplate.Value;

        /// <summary>
        /// Gets the template for a COMMENT template.
        /// </summary>
        public IFluidTemplate CommentTemplate => _commentQueryTemplate.Value;


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
        /// <summary>
        /// Creates a template for a SELECT query.
        /// </summary>
        private static IFluidTemplate CreateSelectQueryTemplate()
        {
            var str = @" SELECT {{ Columns | join: ', ' }} FROM {{Schema}}.{{TableName}} ";

            return parser.Parse(str);
        }

        /// <summary>
        /// Creates a template for an INSERT query.
        /// </summary>
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

        /// <summary>
        /// Creates a template for an UPDATE query.
        /// </summary>
        private static IFluidTemplate CreateUpdateQueryTemplate()
        {
            var str = @"
IF EXISTS (SELECT 1 FROM {{Schema}}.{{TableName}} WHERE {{ Where | join: ' AND ' }})
BEGIN
    UPDATE {{Schema}}.{{TableName}} SET {{ Set | join: ', ' }} WHERE {{ Where | join: ' AND ' }}
END";
            return parser.Parse(str);
        }

        /// <summary>
        /// Creates a template for a DELETE query.
        /// </summary>
        private static IFluidTemplate CreateDeleteQueryTemplate()
        {
            var str = @"
IF EXISTS (SELECT 1 FROM {{Schema}}.{{TableName}} WHERE {{ Where | join: ' AND ' }})
BEGIN
    DELETE FROM {{Schema}}.{{TableName}} WHERE {{ Where | join: ' AND ' }}
END";
            return parser.Parse(str);
        }

        /// <summary>
        /// Creates a template for a COMMENT query.
        /// </summary>
        private static IFluidTemplate CreateCommentQueryTemplate()
        {
            var str = @"{% unless isMultiLine %} -- {{ Comment }} {% else %}
/* 
{{ Comment }}
*/
{% endunless %}";

            return parser.Parse(str);
        }
        #endregion



    }
}

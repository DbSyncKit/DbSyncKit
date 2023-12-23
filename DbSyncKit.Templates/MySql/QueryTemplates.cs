using DbSyncKit.Templates.Interface;
using Fluid;

namespace DbSyncKit.Templates.MySql
{
    /// <summary>
    /// Implementation of <see cref="IQueryTemplates"/> for MySQL database, providing templates for various SQL queries.
    /// </summary>
    public class QueryTemplates : IQueryTemplates
    {
        #region Public Properties
        /// <summary>
        /// Gets the template for a SELECT query.
        /// </summary>
        public IFluidTemplate SELECT_QUERY => _selectQueryTemplate.Value;

        /// <summary>
        /// Gets the template for an INSERT query.
        /// </summary>
        public IFluidTemplate INSERT_QUERY => _insertQueryTemplate.Value;

        /// <summary>
        /// Gets the template for an UPDATE query.
        /// </summary>
        public IFluidTemplate UPDATE_QUERY => _updateQueryTemplate.Value;

        /// <summary>
        /// Gets the template for a DELETE query.
        /// </summary>
        public IFluidTemplate DELETE_QUERY => _deleteQueryTemplate.Value;

        /// <summary>
        /// Gets the template for a COMMENT query.
        /// </summary>
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
        /// <summary>
        /// Creates a template for a SELECT query.
        /// </summary>
        private static IFluidTemplate CreateSelectQueryTemplate()
        {
            var str = @" SELECT {{ Columns | join: ', ' }} FROM {{ TableName }}; ";

            return parser.Parse(str);
        }

        /// <summary>
        /// Creates a template for an INSERT query.
        /// </summary>
        private static IFluidTemplate CreateInsertQueryTemplate()
        {
            var str = @" 
INSERT INTO `{{ TableName }}` ({{ Columns | join: ', ' }}) SELECT {{ Values | join: ', ' }} FROM DUAL WHERE NOT EXISTS ( SELECT 1 FROM `{{ TableName }}` WHERE {{ Where | join: ' AND '  }} )
";

            return parser.Parse(str);
        }

        /// <summary>
        /// Creates a template for an UPDATE query.
        /// </summary>
        private static IFluidTemplate CreateUpdateQueryTemplate()
        {
            var str = @"
UPDATE `{{ TableName }}` SET {{ Set | join: ', ' }} WHERE {{ Where | join: ' AND '  }} LIMIT 1; 
";
            return parser.Parse(str);
        }

        /// <summary>
        /// Creates a template for a DELETE query.
        /// </summary>
        private static IFluidTemplate CreateDeleteQueryTemplate()
        {
            var str = @"
DELETE FROM {{ TableName }} WHERE {{ Where | join: ' AND '  }} LIMIT 1;  ";

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

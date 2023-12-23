using Fluid;

namespace DbSyncKit.Templates.Interface
{
    /// <summary>
    /// Interface defining templates for different types of SQL queries.
    /// </summary>
    public interface IQueryTemplates
    {
        /// <summary>
        /// Gets the template for a SELECT query.
        /// </summary>
        IFluidTemplate SELECT_QUERY { get; }

        /// <summary>
        /// Gets the template for an INSERT query.
        /// </summary>
        IFluidTemplate INSERT_QUERY { get; }

        /// <summary>
        /// Gets the template for an UPDATE query.
        /// </summary>
        IFluidTemplate UPDATE_QUERY { get; }

        /// <summary>
        /// Gets the template for a DELETE query.
        /// </summary>
        IFluidTemplate DELETE_QUERY { get; }

        /// <summary>
        /// Gets the template for a COMMENT query.
        /// </summary>
        IFluidTemplate COMMENT_QUERY { get; }
    }
}

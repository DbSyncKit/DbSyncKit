using Fluid;

namespace DbSyncKit.Templates.Interface
{
    /// <summary>
    /// Interface defining templates for different types of SQL queries.
    /// </summary>
    public interface IQueryTemplates
    {
        /// <summary>
        /// Gets the template for a SELECT template.
        /// </summary>
        IFluidTemplate SelectTemplate { get; }

        /// <summary>
        /// Gets the template for an INSERT template.
        /// </summary>
        IFluidTemplate InsertTemplate { get; }

        /// <summary>
        /// Gets the template for an UPDATE template.
        /// </summary>
        IFluidTemplate UpdateTemplate { get; }

        /// <summary>
        /// Gets the template for a DELETE template.
        /// </summary>
        IFluidTemplate DeleteTemplate { get; }

        /// <summary>
        /// Gets the template for a COMMENT template.
        /// </summary>
        IFluidTemplate CommentTemplate { get; }
    }
}

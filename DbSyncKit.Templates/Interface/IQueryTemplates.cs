using Fluid;

namespace DbSyncKit.Templates.Interface
{
    public interface IQueryTemplates
    {
        IFluidTemplate SELECT_QUERY { get; }
        IFluidTemplate INSERT_QUERY { get; }
        IFluidTemplate UPDATE_QUERY { get; }
        IFluidTemplate DELETE_QUERY { get; }
        IFluidTemplate COMMENT_QUERY { get; }
    }
}

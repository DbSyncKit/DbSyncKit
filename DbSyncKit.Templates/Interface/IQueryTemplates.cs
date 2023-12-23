using DotLiquid;

namespace DbSyncKit.Templates.Interface
{
    public interface IQueryTemplates
    {
        Template SELECT_QUERY { get; }
        Template INSERT_QUERY { get; }
        Template UPDATE_QUERY { get; }
        Template DELETE_QUERY { get; }
        Template COMMENT_QUERY { get; }
    }
}

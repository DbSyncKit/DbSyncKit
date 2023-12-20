using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandlebarsDotNet;

namespace DbSyncKit.Templates.Interface
{
    public interface IQueryTemplates
    {
        HandlebarsTemplate<object, object> SELECT_QUERY { get; }
        HandlebarsTemplate<object, object> INSERT_QUERY { get; }
        HandlebarsTemplate<object, object> UPDATE_QUERY { get; }
        HandlebarsTemplate<object, object> DELETE_QUERY { get; }
        HandlebarsTemplate<object, object> COMMENT_QUERY { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HandlebarsDotNet;

namespace DbSyncKit.Templates.Helpers
{
    public static class HandlebarHelpers
    {
        public static void Register()
        {
            RegisterJoin();
        }

        private static void RegisterJoin()
        {
            Handlebars.RegisterHelper("Join", (writer, context, parameters) =>
            {
                var list = parameters[0] as List<string>;
                if (list != null && list.Count > 0)
                {
                    var joinedColumns = string.Join(", ", list);
                    writer.WriteSafeString(joinedColumns);
                }
            });

            Handlebars.RegisterHelper("And", (writer, context, parameters) =>
            {
                var list = parameters[0] as List<string>;
                if (list != null && list.Count > 0)
                {
                    var joinedColumns = string.Join(" AND ", list);
                    writer.WriteSafeString(joinedColumns);
                }
            });
        }
    }
}

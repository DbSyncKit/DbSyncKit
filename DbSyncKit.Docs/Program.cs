using NUnit.Framework;
using Statiq.Markdown;

namespace DbSyncKit.ConsoleApp
{
    public class Program
    {
        public static async Task<int> Main(string[] args) => 
            await Bootstrapper
            .Factory
            .CreateDocs(args)
            .DeployToNetlify(
                Config.FromSetting<string>("NetlifySiteId"),
                Config.FromSetting<string>("NetlifyAccessToken")
            )
            .AddSetting(WebKeys.AdditionalSearchResultFields, new List<string> { Keys.Excerpt })
            .RunAsync();
    }
}
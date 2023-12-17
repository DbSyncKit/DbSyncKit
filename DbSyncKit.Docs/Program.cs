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
            .AddSetting(WebKeys.NetlifyRedirects, true)
            .AddSourceFiles("../../DbSyncKit.Core/**/{!.git,!bin,!obj,!packages,!*.Tests,}/**/*.cs")
            .RunAsync();
    }
}
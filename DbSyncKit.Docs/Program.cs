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
            .AddProjectFiles("../DbSyncKit.Core/**/*.{cs,xml}")
            .RunAsync();
    }
}
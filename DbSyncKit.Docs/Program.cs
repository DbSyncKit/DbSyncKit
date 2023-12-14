using NUnit.Framework;
using Statiq.Markdown;

namespace DbSyncKit.ConsoleApp
{
    internal class Program
    {
        public static async Task<int> Main(string[] args) 
        {
            var enviroment = System.Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(enviroment).FullName;

            return await Bootstrapper.Factory
            .CreateDocs(args)
            .RunAsync();
        }
    }
}
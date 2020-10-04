using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using csproj_sorter.Models;
using csproj_sorter.Services;

using System.CommandLine;
using System.CommandLine.Invocation;
using System;
using Microsoft.Extensions.Options;

namespace csproj_sorter
{
    public class Program
    {
        public static int Main(string[] args)
        {
            // setup services
            var serviceCollection = AppServices.Configure();

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var appSettings = serviceProvider.GetService<IOptions<AppSettings>>();

            // Create a root command with some options
            var rootCommand = new RootCommand
            {
                new Option<string>("--filename", "The csproj file that should be sorted")
            };

            rootCommand.Description = appSettings.Value.Description;

            // Note that the parameters of the handler method are matched according to the names of the options
            rootCommand.Handler = CommandHandler.Create<string>((string fileName) => {
                serviceProvider.GetService<App>().Run(fileName);
            });

            return rootCommand.InvokeAsync(args).Result;
        }

        private static void ConfigureConsole(AppSettings settings)
        {
            System.Console.Title = settings.ConsoleTitle;
        }
    }
}

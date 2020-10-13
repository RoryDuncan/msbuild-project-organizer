using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CSProjOrganizer.Models;
using CSProjOrganizer.Services;

using System.CommandLine;
using System.CommandLine.Invocation;
using System;
using Microsoft.Extensions.Options;

namespace CSProjOrganizer
{
    public class Program
    {
        public static int Main(string[] args)
        {

            // Create a root command with some options
            var rootCommand = new RootCommand
            {
                new Option<string>("--filename", "The csproj file that should be sorted"),
                new Option<string>("--output", "The output file path, if desired to be separate from the input"),
                new Option<string>("--config", "The path to a configuration file"),
            };

            System.Console.Title = AppSettings.ConsoleTitle;
            rootCommand.Description = AppSettings.Description;

            // Note that the parameters of the handler method are matched according to the names of the options
            rootCommand.Handler = CommandHandler.Create<string, string, string>((string filename, string output, string config) => {
                Program.AppStart(filename, output, config);
            });

            return rootCommand.InvokeAsync(args).Result;
        }

        private static void AppStart(string filename, string output = null, string config = null)
        {
            // setup services
            var serviceCollection = AppServices.Configure(config);

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var appSettings = serviceProvider.GetService<IOptions<AppSettings>>();
            serviceProvider.GetService<App>().Run(filename, output);
        }
    }
}

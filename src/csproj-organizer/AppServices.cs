using System;
using System.IO;
using System.Text.Json;
using CSProjOrganizer.Interfaces;
using CSProjOrganizer.Models;
using CSProjOrganizer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CSProjOrganizer
{
    public class AppServices
    {
        public static IServiceProvider Configure(string configFile = null)
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            // add logging
            serviceCollection.AddSingleton(new LoggerFactory()
                .AddConsole()
                .AddDebug());

            serviceCollection.AddLogging();
            serviceCollection.AddOptions();

            AppServices.AddSortConfiguration(serviceCollection, configFile);
            AppServices.AddServices(serviceCollection);
            serviceCollection.AddTransient<App>();

            // create service provider
            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }

        private static void AddSortConfiguration(IServiceCollection serviceCollection, string configFile)
        {
            SortConfiguration sortConfig = null;

            // if a configuration was passed we set our SortConfiguration model to use it
            // otherwise we use defaults
            if (configFile != null)
            {
                if (!File.Exists(configFile))
                {
                    throw new ArgumentException("Config file does not exist.");
                }

                // these statement may throw exceptions, but we just let 'em happen
                // since they'll have better feedback than us marshalling them
                string json = File.ReadAllText(configFile);
                var options = new JsonSerializerOptions()
                {
                    AllowTrailingCommas = true,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    IgnoreReadOnlyProperties = true,
                };

                sortConfig = JsonSerializer.Deserialize<SortConfiguration>(json, options);
            }
            else
            {
                sortConfig = SortConfiguration.CreateWithDefaults();
            }

            serviceCollection.AddSingleton<SortConfiguration>(sortConfig);
        }

        private static void AddServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IXmlService, XmlService>();
            serviceCollection.AddSingleton<IGroupingService, GroupingService>();
        }
    }
}
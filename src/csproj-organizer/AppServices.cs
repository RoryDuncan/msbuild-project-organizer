using System.IO;
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
        public static IServiceCollection Configure(string configFile = null)
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            // add logging
            serviceCollection.AddSingleton(new LoggerFactory()
                .AddConsole()
                .AddDebug());
            serviceCollection.AddLogging();
            serviceCollection.AddOptions();

            if (configFile != null)
            {
                // build configuration
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(configFile, false)
                    .Build();

                serviceCollection.Configure<SortConfiguration>(configuration.GetSection("Configuration"));
            }

            AppServices.AddServices(serviceCollection);

            // add app
            serviceCollection.AddTransient<App>();

            return serviceCollection;
        }

        private static void AddServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IXmlService, XmlService>();
            serviceCollection.AddSingleton<IGroupingService, GroupingService>();

        }
    }
}
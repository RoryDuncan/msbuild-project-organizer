using System.IO;
using csproj_sorter.Models;
using csproj_sorter.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace csproj_sorter
{
    public class AppServices
    {
        public static IServiceCollection Configure()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            
            // add logging
            serviceCollection.AddSingleton(new LoggerFactory()
                .AddConsole()
                .AddDebug());
            serviceCollection.AddLogging(); 

            // build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("app-settings.json", false)
                .Build();

            serviceCollection.AddOptions();
            serviceCollection.Configure<AppSettings>(configuration.GetSection("Configuration"));

            AppServices.AddServices(serviceCollection);

            // add app
            serviceCollection.AddTransient<App>();

            return serviceCollection;
        }

        private static void AddServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ITestService, TestService>();
            serviceCollection.AddSingleton<IXmlService, XmlService>();
            serviceCollection.AddSingleton<IGroupingService, GroupingService>();

        }
    }
}
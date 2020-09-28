using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using csproj_sorter.Models;
using csproj_sorter.Services;
using System.IO;

namespace csproj_sorter
{
    public class App
    {
        private readonly ITestService _testService;
        private readonly ILogger<App> _logger;
        private readonly AppSettings _config;
        private SortSettings Settings { get; set; }

        public App(ITestService testService,
            IOptions<AppSettings> config,
            ILogger<App> logger)
        {
            _testService = testService;
            _logger = logger;
            _config = config.Value;
        }

        public void ConfigureSettings(string configFileName, string directory)
        {
            string filename = configFileName ?? _config.DefaultConfigFileName;
            // todo: read json as SortSettings class
        }

        public void Run(string target, FileInfo config)
        {

            if (string.IsNullOrWhiteSpace(target)) 
            {
                _logger.LogError($"A --target was not provided. Run this application with a path to a .csproj file as the --target.");
                System.Console.ReadKey();
                return;
            }

            _logger.LogInformation($"This is a console application for {_config.ConsoleTitle}");
            _testService.Run();
            System.Console.ReadKey();
        }

    }
}

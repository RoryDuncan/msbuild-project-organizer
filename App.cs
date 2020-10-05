using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using csproj_sorter.Models;
using csproj_sorter.Services;
using System.IO;
using System.Xml.Linq;
using csproj_sorter.Enums;

namespace csproj_sorter
{
    public class App
    {
        private readonly ITestService _testService;
        private readonly ILogger<App> _logger;
        private readonly AppSettings _config;
        private SortSettings Settings { get; set; }
        private readonly IXmlService _xmlService;
        private readonly IGroupingService _groupingService;

        public App(ITestService testService,
            IOptions<AppSettings> config,
            ILogger<App> logger,
            IXmlService xmlService,
            IGroupingService groupingService)
        {
            _testService = testService;
            _logger = logger;
            _config = config.Value;
            _xmlService = xmlService;
            _groupingService = groupingService;
        }

        public void ConfigureSettings(string configFileName, string directory)
        {
            string filename = configFileName ?? _config.DefaultConfigFileName;
            // todo: read json as SortSettings class
        }

        public void Run(string target)
        {
            _logger.LogInformation($"params: {target}");
            if (string.IsNullOrWhiteSpace(target)) 
            {
                _logger.LogError($"A --filename was not provided. Run this application with the name of a .csproj file as the --filename arg.");
                return;
            }

            _logger.LogInformation($"Sorting '{target}'...");

            XDocument document = _xmlService.GetXML(target);
            _groupingService.Group(document, GroupBy.NodeType);
            

            _testService.Run();
        }

    }
}

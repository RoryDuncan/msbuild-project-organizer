using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CSProjOrganizer.Models;
using System.Xml.Linq;
using CSProjOrganizer.Interfaces;

namespace CSProjOrganizer
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

        public void Run(string input)
        {
            if (!IsValid(input))
            {
                return;
            }

            _logger.LogInformation($"Sorting '{input}'...");

            // load the document
            XDocument document = _xmlService.GetDocument(input);

            // sort the items
            bool wasModified = _groupingService.Group(document);

            if (wasModified) {
                _logger.LogInformation("Sort complete.");
                _xmlService.SaveDocument($"{input}.sorted", document);
            }

            _logger.LogInformation("Done.");

            // _testService.Run();
        }

        private bool IsValid(string input)
        {
            _logger.LogInformation($"params: {input}");
            if (string.IsNullOrWhiteSpace(input))
            {
                _logger.LogError($"A --filename was not provided. Run this application with the name of a .csproj file as the --filename arg.");
                return false;
            }

            return true;
        }

    }
}

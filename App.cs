using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using csproj_sorter.Models;
using csproj_sorter.Services;
using System.IO;
using System.Xml.Linq;
using csproj_sorter.Enums;
using System.Xml;
using System.Text;

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

        public void Run(string input)
        {
            _logger.LogInformation($"params: {input}");
            if (string.IsNullOrWhiteSpace(input)) 
            {
                _logger.LogError($"A --filename was not provided. Run this application with the name of a .csproj file as the --filename arg.");
                return;
            }

            _logger.LogInformation($"Sorting '{input}'...");

            XDocument document = _xmlService.GetXML(input);
            _groupingService.Group(document, GroupBy.NodeType);
            _logger.LogInformation("Sort complete.");

            SaveDocument($"{input}.sorted", document);

            // _testService.Run();
        }

        public void SaveDocument(string fileName, XDocument document)
        {
            _logger.LogInformation($"Saving Document as {fileName}.");

            var settings = new XmlWriterSettings()
            {
                Indent = true,
            };

            using (XmlWriter writer = XmlWriter.Create(fileName, settings))
            {
                document.Save(writer);
            }
            _logger.LogInformation("Done.");
        }
    }
}

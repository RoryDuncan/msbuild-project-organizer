using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CSProjOrganizer.Models;
using System.Xml.Linq;
using CSProjOrganizer.Interfaces;

namespace CSProjOrganizer
{
    public class App
    {
        private readonly ILogger<App> _logger;
        private readonly SortConfiguration _config;
        private readonly IXmlService _xmlService;
        private readonly IGroupingService _groupingService;

        public App(SortConfiguration config,
                   ILogger<App> logger,
                   IXmlService xmlService,
                   IGroupingService groupingService)
        {
            _logger = logger;
            _config = config;
            _xmlService = xmlService;
            _groupingService = groupingService;
        }

        public void Run(string input, string output)
        {
            if (!IsValid(input))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(output))
            {
                output = input;
            }

            _logger.LogInformation($"Sorting '{input}'...");

            // load the document
            XDocument document = _xmlService.GetDocument(input);


            if (_config.IsDefault)
            {
                _logger.LogInformation("Using default configuration");
            }

            if (_config.SortOptions is null)
            {
                _logger.LogInformation("Using default sort options");
            }

            var sortOptions = _config.SortOptions ?? new SortOptions()
            {
                GroupByNodeType = true,
                GroupByFileType = true,
                SortItemsWithinItemGroups = true,
                RemoveEmptyItemGroups = true,
            };

            // sort the items
            bool wasModified = _groupingService.Group(document, sortOptions);

            if (wasModified)
            {
                _logger.LogInformation("Sort complete.");
                _xmlService.SaveDocument(output, document);
            }
            else
            {
                _logger.LogInformation($"No modifications were necessary for '{input}'.");
            }

            _logger.LogInformation("Done.");
        }

        private bool IsValid(string input)
        {
            _logger.LogInformation($"params: {input}");
            if (string.IsNullOrWhiteSpace(input))
            {
                _logger.LogError($"A --input was not provided. Run this application with the name of a .csproj file as the --input arg.");
                return false;
            }

            return true;
        }

    }
}

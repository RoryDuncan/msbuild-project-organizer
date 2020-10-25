using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CSProjOrganizer.Models;
using System.Xml.Linq;
using CSProjOrganizer.Interfaces;

namespace CSProjOrganizer
{
    /// <summary>
    /// A class for Organizing Project files (.csproj)
    /// </summary>
    public class ProjectOrganizer
    {
        private readonly ILogger<ProjectOrganizer> _logger;
        private readonly SortConfiguration _config;
        private readonly IXmlService _xmlService;
        private readonly IGroupingService _groupingService;

        /// <summary>
        /// App constructor
        /// </summary>
        /// <param name="config"></param>
        /// <param name="logger"></param>
        /// <param name="xmlService"></param>
        /// <param name="groupingService"></param>
        public ProjectOrganizer(SortConfiguration config,
                   ILogger<ProjectOrganizer> logger,
                   IXmlService xmlService,
                   IGroupingService groupingService)
        {
            _logger = logger;
            _config = config;
            _xmlService = xmlService;
            _groupingService = groupingService;
        }

        /// <summary>
        /// Reads, organizes, and saves a csproj file
        /// </summary>
        /// <param name="input">The file to read and organize</param>
        /// <param name="output">A file where the results should be saved — defaults to <see param="input" /></param>
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

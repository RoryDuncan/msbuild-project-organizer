using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MSBuildProjectOrganizer.Models;
using System.Xml.Linq;
using MSBuildProjectOrganizer.Interfaces;

namespace MSBuildProjectOrganizer
{
    /// <summary>
    /// An implementation of IProjectOrganizer
    /// </summary>
    public class ProjectOrganizer : IProjectOrganizer
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

        /// <inheritdoc />
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

            var parts = input.Split(@"\");
            string fileName = parts[parts.Length - 1];

            _logger.LogInformation($"Sorting '{fileName}'...");

            // load the document
            XDocument document = _xmlService.GetDocument(input);


            if (_config.IsDefault)
            {
                _logger.LogDebug("Using default configuration");
            }

            if (_config.SortOptions is null)
            {
                _logger.LogDebug("Using default sort options");
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
                _logger.LogWarning($"No sorting was necessary.");
            }
        }

        private bool IsValid(string input)
        {
            _logger.LogDebug($"params: {input}");
            if (string.IsNullOrWhiteSpace(input))
            {
                _logger.LogError($"A --input was not provided. Run this application with the name of a .csproj file as the --input arg.");
                return false;
            }

            return true;
        }

    }
}

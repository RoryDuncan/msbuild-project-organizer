using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace CSProjOrganizer
{
    /// <summary>
    /// A class for Organizng all Projects within a Solution
    /// </summary>
    public class SolutionOrganizer
    {
        private readonly ILogger<SolutionOrganizer> _logger;
        private readonly IProjectOrganizer _projectOrganizer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="projectOrganizer"></param>
        public SolutionOrganizer(ILogger<SolutionOrganizer> logger, IProjectOrganizer projectOrganizer)
        {
            _logger = logger;
            _projectOrganizer = projectOrganizer;
        }

        /// <summary>
        /// Searches for a solution file, parses it, and then performs a sort and organize on each project within it
        /// </summary>
        public void Run()
        {
            
        }

        private static string FindSolutionFile()
        {
            return null;
        }

        private static IEnumerable<string> ParseSolutionFile(string solutionFile)
        {
            var result = new List<string>();

            return result;
        }
    }
}
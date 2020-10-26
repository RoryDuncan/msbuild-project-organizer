using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly string solutionGlob = "*.sln";

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
        public void Run(string solutionFile = null)
        {
            if (string.IsNullOrWhiteSpace(solutionFile))
            {
                string cwd = Directory.GetCurrentDirectory();
                var solutionFiles = Directory.EnumerateFiles(cwd, solutionGlob);

                if (solutionFiles.Count() > 1)
                {
                    throw new System.Exception("Multiple solution files were found in the current directory. Please use the --solution argument to specify the intended solution.");
                }

                if (solutionFiles.Count() == 0)
                {
                    throw new FileNotFoundException("No solution files found in the current directory.");
                }

                solutionFile = solutionFiles.First();
            }

            _logger.LogInformation(solutionFile);
        }

        private string FindSolutionFile(string path = null)
        {


            return null;
        }

        private IEnumerable<string> ParseSolutionFile(string solutionFile)
        {
            var result = new List<string>();

            return result;
        }
    }
}
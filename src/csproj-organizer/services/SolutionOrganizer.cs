using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Construction;
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
            solutionFile = this.GetSolutionFile(solutionFile);

            _logger.LogDebug($"Solution file found: {solutionFile}");

            SolutionFile solution = SolutionFile.Parse(solutionFile);

            List<ProjectInSolution> projects = solution.ProjectsInOrder.ToList();

            projects.ForEach( project => {
                using (var scope = _logger.BeginScope(project.ProjectName))
                {
                    _logger.LogInformation($"Sorting {project.ProjectName}: Sorting... ");
                    _projectOrganizer.Run(project.AbsolutePath, null);
                    _logger.LogInformation($"Sorting {project.ProjectName}: Done. ");
                }
            });

            _logger.LogInformation($"All projects sorted.");
        }

        private string GetSolutionFile(string solutionFile)
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

            return solutionFile;
        }
    }
}
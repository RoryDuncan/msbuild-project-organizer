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
            if (!this.TryGetSolutionFilePath(solutionFile, out string error))
            {
                _logger.LogWarning(error);
                return;
            }


            _logger.LogDebug($"Solution file found: {solutionFile}");

            SolutionFile solution = GetSolutionFromPath(solutionFile);

            List<ProjectInSolution> projects = solution.ProjectsInOrder.ToList();

            projects.ForEach( project => {
                using (var scope = _logger.BeginScope(project.ProjectName))
                {
                    try
                    {
                        _logger.LogInformation($"Sorting {project.ProjectName}: Sorting... ");
                        if (project.AbsolutePath.EndsWith(".csproj"))
                        {
                            _logger.LogInformation($"Sorting {project.ProjectName}: Sorting... ");
                            _projectOrganizer.Run(project.AbsolutePath, null);
                            _logger.LogInformation($"Sorting {project.ProjectName}: Done. ");
                        }
                        else
                        {
                            _logger.LogInformation($"{project.ProjectName} does not have a project file.");
                        }

                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"Unable to sort {project.ProjectName}: {ex.Message}");
                    }
                }
            });

            _logger.LogInformation($"All projects sorted.");
        }

        private bool TryGetSolutionFilePath(string solutionFile, out string error)
        {
            error = null;
            if (string.IsNullOrWhiteSpace(solutionFile))
            {
                string cwd = Directory.GetCurrentDirectory();
                var solutionFiles = Directory.EnumerateFiles(cwd, solutionGlob);

                if (solutionFiles.Count() > 1)
                {
                    error = "Multiple solution files were found in the current directory. Please use the --sln argument to specify the intended solution.";
                    return false;
                }

                if (solutionFiles.Count() == 0)
                {
                    error = "No solution files found in the current directory.";
                    return false;
                }

                solutionFile = solutionFiles.First();
            }

            return true;
        }

        private SolutionFile GetSolutionFromPath(string filePath)
        {
            // need an absolute path otherwise SolutionFile.Parse will
            // throw an "unexpectedly not a rooted path" exception
            string absolutePath = Path.GetFullPath(filePath);
            return SolutionFile.Parse(absolutePath);
        }
    }
}
using System;
using Microsoft.Extensions.DependencyInjection;

namespace CSProjOrganizer
{
    /// <summary>
    /// Sorts and organizes a .csproj file
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Sorts and organizes messy csproj files
        /// </summary>
        /// <param name="scan">Scan for a .sln file in the current directory, and organize all associated .csproj files. Other arguments have no effect when using scan</param>
        /// <param name="input">The csproj file that should be sorted</param>
        /// <param name="output">The output file path, if the result should be saved to a new file</param>
        /// <param name="config">The path to a configuration file</param>
        /// <param name="sln">The path to a specific solution file. This is only necessary if you have multiple .sln files in the current directory.</param>
        public static void Main(bool scan = false, string input = null, string output = null, string config = null, string sln = null)
        {
            if (!scan && input is null && output is null && config is null) 
            {
                Console.WriteLine("Use --help to see options.");
                return;
            }

            if (scan)
            {
                Program.OrganizeSolution(sln);
            }
            else
            {
                Program.OrganizeCSProj(input, output, config);
            }
        }

        private static void OrganizeCSProj(string input, string output = null, string config = null)
        {
            // setup services
            var serviceProvider = AppServices.Configure(config);

            serviceProvider.GetService<ProjectOrganizer>().Run(input, output);
        }

        private static void OrganizeSolution(string solutionFile = null)
        {
            // setup services
            var serviceProvider = AppServices.Configure(null);

            serviceProvider.GetService<SolutionOrganizer>().Run(solutionFile);
        }
    }
}

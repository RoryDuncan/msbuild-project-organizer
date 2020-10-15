using System.Collections.Generic;

namespace CSProjOrganizer.Models
{
    public class AppSettings
    {
        /// <summary>
        /// Sets header title in console window
        /// </summary>
        public static readonly string ConsoleTitle = "CSProjOrganizer";

        /// <summary>
        /// The default name of the file configuration to look for when running the application
        /// </summary>
        public static readonly string DefaultConfigFileName = "csproj.config.defaults.json";

        public static readonly string Description = "Sorts and organizes a .csproj file";

    }
}

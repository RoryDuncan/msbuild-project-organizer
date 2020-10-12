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
        public static readonly string DefaultConfigFileName = "csproj.config.json";

        public static readonly string Description = "Sorts and organizes a .csproj file";


        #region sort / grouping configuration


        /// <summary>
        /// The label given to items that don't have a file extension and aren't glob patterns
        /// </summary>
        /// <value>"Misc"</value>
        public string UnknownFileExtensionLabel { get; set; } = "Misc";
        /// <summary>
        /// The name of Item with a Glob pattern filepath and no file extension
        /// e.g.: "/images/**/*", NOT "/images/**/*.png"
        /// </summary>
        /// <value>"Glob Pattern"</value>
        public string GlobPatternLabel { get; set; } = "Glob Pattern";
        /// <summary>
        /// Items that represent files. AKA Items that are likely to have an "Include" attribute
        /// </summary>
        /// <value></value>
        public IEnumerable<string> FileTypeItems { get; set; } = new List<string>();
        /// <summary>
        /// Items that can be grouped together into a single ItemGroup.
        /// Useful for grouping different images types together and such
        /// </summary>
        public Dictionary<string, IEnumerable<string>> FileTypeGroupings { get; set; }

        /// <summary>
        /// Labels for ItemGroups, displaying as comments before that group
        /// </summary>
        public Dictionary<string, IEnumerable<string>> ItemGroupGroupings { get; set; }


        #endregion
    }
}

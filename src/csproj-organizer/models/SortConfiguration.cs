using System.Collections.Generic;

namespace CSProjOrganizer.Models
{
    public class SortConfiguration
    {
        public readonly bool IsDefault;
        public SortOptions SortOptions { get; set; }

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


        public SortConfiguration()
        {

        }

        public SortConfiguration(bool isDefault)
        {
            IsDefault = isDefault;
        }

        public static SortConfiguration CreateWithDefaults()
        {

            var FileTypeItems = new List<string>()
            {
                "Content",
                "Compile",
                "TypeScriptCompile",
                "EmbeddedResource",
            };

            var FileTypeGroupings = new Dictionary<string, IEnumerable<string>>();

            FileTypeGroupings.Add("Images", new string[] {
                ".png", ".jpg", ".jpeg", ".gif", ".tiff", ".ico", ".icon", "webp", ".svg", ".bmp",
            });

            FileTypeGroupings.Add("Stylesheets", new string[] {
                ".css", ".less",
            });

            FileTypeGroupings.Add("Razor Files", new string[] {
                ".cshtml"
            });

            var ItemGroupGroupings = new Dictionary<string, IEnumerable<string>>();

            ItemGroupGroupings.Add("Published Files", new string[]{
                "Content"
            });
            ItemGroupGroupings.Add("Typescript and Type Definitions", new string[]{
                "TypeScriptCompile"
            });
            ItemGroupGroupings.Add("Non-Build Files", new string[]{
                "None"
            });
            ItemGroupGroupings.Add("Assembly References", new string[]{
                "Reference"
            });
            ItemGroupGroupings.Add("Project References", new string[]{
                "ProjectReference"
            });
            ItemGroupGroupings.Add("NuGet Package References", new string[]{
                "PackageReference"
            });
            ItemGroupGroupings.Add("Folders", new string[]{
                "Folder"
            });
            ItemGroupGroupings.Add("Compiled C# Files", new string[]{
                "Compile", "CSFile"
            });


            var defaults = new SortConfiguration(isDefault: true)
            {
                FileTypeItems = FileTypeItems,
                FileTypeGroupings = FileTypeGroupings,
                ItemGroupGroupings = ItemGroupGroupings,
            };

            return defaults;
        }
    }
}
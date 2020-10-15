namespace CSProjOrganizer.Models
{
    public class SortOptions
    {
        /// <summary>
        /// Whether to create and consolidate ItemGroups for each distinct Item type (Content, Compile, None, etc).
        /// </summary>
        public bool GroupByNodeType { get; set; }
        /// <summary>
        /// Whether to create ItemGroups for each distinct file extension
        /// </summary>
        public bool GroupByFileType { get; set; }
        /// <summary>
        /// Whether to remove empty ItemGroups in the input csproj. This utility doesn't create empty ItemGroups
        /// </summary>
        public bool RemoveEmptyItemGroups { get; set; }
        /// <summary>
        /// Whether all ItemGroups should have their Items alpha-numerically sorted
        /// Is performed Last.
        /// </summary>
        public bool SortItemsWithinItemGroups { get; set; }
    }
}

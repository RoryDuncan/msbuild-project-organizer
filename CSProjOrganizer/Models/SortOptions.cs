namespace CSProjOrganizer.Models
{
    public class SortOptions
    {
        public bool GroupByNodeType { get; set; } = true;
        public bool GroupByFileType { get; set; } = true;
        public bool RemoveEmptyItemGroups { get; set; } = true;
        public bool SortItemsWithinItemGroups { get; set; } = true;

        public static SortOptions CreateEmpty()
        {
            return new SortOptions()
            {
                GroupByNodeType = false,
                GroupByFileType = false,
                RemoveEmptyItemGroups = false,
                SortItemsWithinItemGroups = false,
            };
        }
    }
}

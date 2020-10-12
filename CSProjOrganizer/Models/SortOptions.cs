namespace CSProjOrganizer.Models
{
    public class SortOptions
    {
        public bool GroupByNodeType { get; set; }
        public bool GroupByFileType { get; set; }
        public bool RemoveEmptyItemGroups { get; set; }
        public bool SortItemsWithinItemGroups { get; set; }

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

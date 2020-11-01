using System.Xml.Linq;
using MSBuildProjectOrganizer.Models;

namespace MSBuildProjectOrganizer.Interfaces
{
    /// <summary>
    /// A service for grouping the nodes within an XML document
    /// </summary>
    public interface IGroupingService
    {
        /// <summary>
        /// Performs grouping on an XML document
        /// </summary>
        /// <param name="document"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        bool Group(XDocument document, SortOptions options);
    }
}
using System.Xml.Linq;
using CSProjOrganizer.Models;

namespace CSProjOrganizer.Interfaces
{
    public interface IGroupingService
    {
        bool Group(XDocument document, SortOptions options);
    }
}
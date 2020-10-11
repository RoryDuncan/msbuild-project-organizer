using System.Xml.Linq;

namespace CSProjOrganizer.Interfaces
{
    public interface IGroupingService
    {
        bool Group(XDocument document);
    }
}
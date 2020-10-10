using System.Xml.Linq;

namespace csproj_sorter.Interfaces
{
    public interface IGroupingService
    {
        bool Group(XDocument document);
    }
}
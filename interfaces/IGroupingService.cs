using System.Xml.Linq;
using csproj_sorter.Enums;

namespace csproj_sorter.Interfaces
{
    public interface IGroupingService
    {
        bool Group(XDocument document, GroupBy grouping);
    }
}
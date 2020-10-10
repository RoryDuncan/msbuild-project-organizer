using System.Xml.Linq;

namespace csproj_sorter.Interfaces
{
    public interface IXmlService
    {
        XDocument GetDocument(string filePath);
        void SaveDocument(string input, XDocument document);

    }

}
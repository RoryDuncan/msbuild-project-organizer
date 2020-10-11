using System.Xml.Linq;

namespace CSProjOrganizer.Interfaces
{
    public interface IXmlService
    {
        XDocument GetDocument(string filePath);
        void SaveDocument(string input, XDocument document);

    }

}
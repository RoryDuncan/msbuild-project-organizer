using System.Xml.Linq;

namespace CSProjOrganizer.Interfaces
{
    /// <summary>
    /// Service for reading and writing XML documents
    /// </summary>
    public interface IXmlService
    {
        /// <summary>
        /// Read an xml document by its filepath
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>The file as an XDocument</returns>
        XDocument GetDocument(string filePath);
        /// <summary>
        /// Save an XMLDocument, overwriting or creating a new file based on the filename
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="document"></param>
        void SaveDocument(string filename, XDocument document);

    }

}
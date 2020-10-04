using System.IO;
using System.Xml.Linq;
using csproj_sorter.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace csproj_sorter.Services
{
    public interface IXmlService
    {
        XDocument GetXML(string filePath);

    }

    public class XmlService : IXmlService
    {
        private readonly ILogger<TestService> _logger;

        public XmlService(ILogger<TestService> logger)
        {
            _logger = logger;
        }

        public Stream ReadFile(FileInfo file)
        {
            throw new System.NotImplementedException();
        }

        public XDocument GetXML(string filePath)
        {
            XDocument document = XDocument.Load(filePath);
            return document;
        }
    }
}
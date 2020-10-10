using System.IO;
using System.Xml;
using System.Xml.Linq;
using csproj_sorter.Interfaces;
using csproj_sorter.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace csproj_sorter.Services
{
    public class XmlService : IXmlService
    {
        private readonly ILogger<XmlService> _logger;

        public XmlService(ILogger<XmlService> logger)
        {
            _logger = logger;
        }

        public XDocument GetDocument(string filePath)
        {
            _logger.LogInformation($"Loading Document: {filePath}.");
            XDocument document = XDocument.Load(filePath);
            return document;
        }

        public void SaveDocument(string fileName, XDocument document)
        {
            _logger.LogInformation($"Saving Document as {fileName}.");

            var settings = new XmlWriterSettings()
            {
                Indent = true,
            };

            using (XmlWriter writer = XmlWriter.Create(fileName, settings))
            {
                document.Save(writer);
            }
        }
    }
}
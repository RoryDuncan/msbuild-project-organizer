using System.IO;
using System.Xml;
using System.Xml.Linq;
using CSProjOrganizer.Interfaces;
using CSProjOrganizer.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CSProjOrganizer.Services
{
    /// <summary>
    /// Implementation of IXMLService
    /// </summary>
    public class XmlService : IXmlService
    {
        private readonly ILogger<XmlService> _logger;
        /// <summary>
        /// Create an XmlService
        /// </summary>
        /// <param name="logger"></param>
        public XmlService(ILogger<XmlService> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public XDocument GetDocument(string filePath)
        {
            _logger.LogDebug($"Loading Document: {filePath}.");
            XDocument document = XDocument.Load(filePath);
            return document;
        }

        /// <inheritdoc />
        public void SaveDocument(string fileName, XDocument document)
        {
            _logger.LogDebug($"Saving Document as {fileName}.");

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
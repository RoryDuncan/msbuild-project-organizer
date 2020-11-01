using System.Xml.Linq;
using MSBuildProjectOrganizer.Interfaces;

namespace MSBuildProjectOrganizer.Services
{
    /// <summary>
    /// An implementation of IXmlNameProvider
    /// </summary>
    public class XmlNameProvider : IXmlNameProvider
    {
        private XNamespace _xmlns;

        /// <summary>
        /// Create an XmlNameProvider with the XML namespace context
        /// </summary>
        /// <param name="xmlns">The XML namespace that should be used by all other methods</param>
        public XmlNameProvider(XNamespace xmlns)
        {
            _xmlns = xmlns;
        }

        /// <inheritdoc />
        public XName Get(string name)
        {
            return _xmlns.GetName(name);
        }
        
        /// <inheritdoc />
        public XName ItemGroup => Get("ItemGroup");
        /// <inheritdoc />
        public XName Project => Get("Project");

        /// <inheritdoc />
        public XName Label => XName.Get("Label");
        /// <inheritdoc />
        public XName Include => XName.Get("Include");
        /// <inheritdoc />
        public XName Exclude => XName.Get("Exclude");
        /// <inheritdoc />
        public XName Condition => XName.Get("Condition");
    }
}
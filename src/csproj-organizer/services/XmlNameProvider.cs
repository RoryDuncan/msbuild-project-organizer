using System.Xml.Linq;
using CSProjOrganizer.Interfaces;

namespace CSProjOrganizer.Services
{
    public class XmlNameProvider : IXmlNameProvider
    {
        private XNamespace _xmlns;

        public XmlNameProvider(XNamespace xmlns)
        {
            _xmlns = xmlns;
        }

        public XName Get(string name)
        {
            return _xmlns.GetName(name);
        }

        public XName ItemGroup => Get("ItemGroup");
        public XName Project => Get("Project");

        // Attributes don't need scoping to an XMLNS
        public XName Label => XName.Get("Label");
        public XName Include => XName.Get("Include");
        public XName Exclude => XName.Get("Exclude");
        public XName Condition => XName.Get("Condition");
    }
}
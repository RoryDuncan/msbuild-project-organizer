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

        public XName Include => Get("Exclude");

        public XName Exclude => Get("Exclude");

        public XName ItemGroup => Get("ItemGroup");

        public XName Project => Get("Project");
        public XName Label => Get("Label");
        public XName Condition => Get("Condition");
    }
}
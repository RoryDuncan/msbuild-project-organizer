using System.Xml.Linq;

namespace CSProjOrganizer.Interfaces
{
    public interface IXmlNameProvider
    {
        XName Get(string name);
        XName Project { get; }
        XName ItemGroup { get; }
        XName Include { get; }
        XName Exclude { get; }
        XName Label { get; }
        XName Condition { get; }
    }
}
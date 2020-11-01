using System.Xml.Linq;

namespace MSBuildProjectOrganizer.Interfaces
{
    /// <summary>
    /// A service for resolving the current name of XML nodes and attributes based on the context
    /// </summary>
    public interface IXmlNameProvider
    {
        /// <summary>
        /// Retrieve an XName
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        XName Get(string name);
        /// <summary>
        /// Retrieve the XName of a Project node
        /// </summary>
        /// <value></value>
        XName Project { get; }
        /// <summary>
        /// Retrieve the XName of a ItemGroup node
        /// </summary>
        /// <value></value>
        XName ItemGroup { get; }
        /// <summary>
        /// Retrieve the XName of an Include attribute
        /// </summary>
        /// <value></value>
        XName Include { get; }
        /// <summary>
        /// Retrieve the XName of an Exclude attribute
        /// </summary>
        /// <value></value>
        XName Exclude { get; }
        /// <summary>
        /// Retrieve the XName of a Label attribute
        /// </summary>
        /// <value></value>
        XName Label { get; }
        /// <summary>
        /// Retrieve the XName of a Condition attribute
        /// </summary>
        /// <value></value>
        XName Condition { get; }
    }
}
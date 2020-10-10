using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using csproj_sorter.Interfaces;
using csproj_sorter.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace csproj_sorter.Services
{

    public class GroupingService : IGroupingService
    {
        private readonly string xmlns = "http://schemas.microsoft.com/developer/msbuild/2003";

        private readonly ILogger<TestService> _logger;
        private readonly AppSettings _config;

        public GroupingService(ILogger<TestService> logger,
            IOptions<AppSettings> config)
        {
            _logger = logger;
            _config = config.Value;
        }

        /// <summary>
        /// Groups and sorts the XDocument's ItemGroup. Returns a bool indicating if the document was modified or not.
        /// </summary>
        public bool Group(XDocument document)
        {
            bool wasModified = false;
            
            wasModified = this.GroupByNodeType(document);
            wasModified = this.GroupByFileType(document);

            return wasModified;
        }

        private bool GroupByFileType(XDocument document)
        {
            throw new NotImplementedException();
        }

    /// <summary>
    /// Groups and sorts the XDocument's ItemGroup. Returns a bool indicating if the document was modified or not.
    /// </summary>
    /// <param name="document"></param>
    /// <returns></returns>
        public bool GroupByNodeType(XDocument document)
        {
            var projectRoot = document.Element(Name("Project"));
            if (projectRoot == null) {
                _logger.LogInformation("No <Project> found within document. Nothing to sort.");
                return false;
            }

            var initialGroups = projectRoot.Descendants(Name("ItemGroup"));

            List<XElement> itemGroupChildren = initialGroups.Elements().ToList();
            
            // an empty item group for us to copy from
            XElement emptyItemGroup = new XElement(Name("ItemGroup"));

            // group on the name of the node, like <Content />
            var itemGroups = itemGroupChildren
                .GroupBy(el => el.Name.LocalName)
                .Select(group => {
                    XElement itemGroup = new XElement(emptyItemGroup);

                    foreach (XElement item in group)
                    {
                        itemGroup.Add(item);
                    }

                    return itemGroup;
                })
                .ToList();

            // remove the existing item groups
            initialGroups.Remove();

            // Sort itemgroups by the name of their first child
            itemGroups = itemGroups.OrderBy(group => group.Elements().First().Name.LocalName).ToList();

            // and add them in their new groupings
            foreach (XElement group in itemGroups)
            {
                if (group.HasElements) {
                    string comment = GetComment(group.Elements().First());
                    projectRoot.Add(new XComment($" {comment} "));
                    projectRoot.Add(group);
                }
            }

            return true;
        }

        private XName Name(string name)
        {
            return XName.Get(name, xmlns);
        }

        private string GetComment(XElement element)
        {
            switch (element.Name.LocalName)
            {
                case "ProjectReference":
                    return "Project References";
                case "None":
                    return "Ignored Files";
                case "Content":
                    return "General"; // this group could be further broken up based on file extension
                case "TypeScriptCompiles":
                    return "Typescript";
                default:
                    break;
            }

            return $"{element.Name.LocalName} Includes";
        }
    }
}
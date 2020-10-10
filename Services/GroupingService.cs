using System;
using System.Collections.Generic;
using System.IO;
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

        private readonly ILogger<GroupingService> _logger;
        private readonly AppSettings _config;

        public GroupingService(ILogger<GroupingService> logger,
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
            if (!HasProjectRoot(document))
            {
                _logger.LogInformation("No <Project> found within document. Nothing to sort.");
                return false;
            }

            this.GroupByNodeType(document);
            this.ThenByFileType(document);

            return true;
        }

    /// <summary>
    /// Groups and sorts the XDocument's ItemGroup. Returns a bool indicating if the document was modified or not.
    /// </summary>
    /// <param name="document"></param>
    /// <returns></returns>
        public void GroupByNodeType(XDocument document)
        {
            var projectRoot = document.Element(Name("Project"));

            var initialGroups = projectRoot
                .Descendants(Name("ItemGroup"))
                .Where( group => group.Attribute("Condition") is null); // only operate on ItemGroups without "Condition" attributes

            List<XElement> itemGroupChildren = initialGroups.Elements().ToList();

            // group on the name of the node, like <Content />
            var itemGroups = itemGroupChildren
                .GroupBy(el => el.Name.LocalName)
                .Select(group => {
                    XElement itemGroup = CreateItemGroup();

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
                if (group.HasElements) 
                {
                    // Add a label corrosponding to the itemgroup's contents
                    var label = GetLabel(group.Elements().First());
                    group.SetAttributeValue("Label", label);

                    projectRoot.Add(group);
                }
            }

        }

        public void ThenByFileType(XDocument document)
        {
            var projectRoot = document.Element(Name("Project"));

            // we can assume all item groups are of the same XElement type
            List<XElement> itemGroups = projectRoot.Descendants(Name("ItemGroup")).ToList();

            int initialCount = itemGroups.Count;
            _logger.LogInformation($"There {(initialCount == 1 ? "is" : "are")} {initialCount} <ItemGroup> node{(initialCount == 1 ? string.Empty : "s")}");

            itemGroups
                .Where( itemGroup => this.IsFileType(itemGroup.Elements().First()))
                .ToList()
                .ForEach( itemGroup => {
                    Dictionary<string, XElement> newItemGroups = new Dictionary<string, XElement>();
                    string previousLabel = (string)itemGroup.Attribute("Label") ?? string.Empty;
                    // for each item of the group, check it's file type and add it to an itemgroup of similar filetypes
                    itemGroup.Elements().ToList().ForEach( element => {
                        string fileType = GetFileExtension(element) ?? "none";
                        string label = $"{fileType} files";

                        if (!newItemGroups.ContainsKey(fileType))
                        {
                            _logger.LogInformation($"Creating <ItemGroup Label=\"{label}\">");
                            var group = CreateItemGroup();
                            group.SetAttributeValue(Name("Label"), label);
                            newItemGroups.Add(fileType, CreateItemGroup());
                        }
                        else
                        {
                            _logger.LogInformation($"Adding to <ItemGroup Label=\"{label}\">");
                        }
                        
                        newItemGroups
                            .GetValueOrDefault(fileType)
                            .Add(element);
                    });

                    // if there's only 1 filetype in this ItemGroup, no need to make changes
                    if (newItemGroups.Count > 1)
                    {
                        // add the new filetype-grouped <ItemGroup>'s immediately after this group
                        _logger.LogInformation($"<ItemGroup> split into {newItemGroups.Count} filetypes");
                        newItemGroups
                            .ToList()
                            .ForEach( kvp => itemGroup.AddAfterSelf(kvp.Value));

                        // remove the original <ItemGroup>
                        itemGroup.Remove();
                    }
                });

            // log how it's changed
            int resultingCount = projectRoot.Descendants(Name("ItemGroup")).ToList().Count;
            _logger.LogInformation($"There {(resultingCount == 1 ? "is" : "are")} {(resultingCount == initialCount ? "still" : "now")} {resultingCount} <ItemGroup> node{(resultingCount == 1 ? string.Empty : "s")}");
        }

        private string GetFileExtension(XElement element)
        {
            XAttribute includeAttr = element.Attribute("Include");
            if (includeAttr is null)
            {
                return null;
            }

            return Path.GetExtension(includeAttr.Value);
        }

        private bool IsFileType(XElement element)
        {
            return _config.FileTypeItems.Contains(element.Name.LocalName);
        }

        private XName Name(string name)
        {
            return XName.Get(name, xmlns);
        }

        private bool HasProjectRoot(XDocument document)
        {
            XElement projectRoot = document.Element(Name("Project"));
            return !(projectRoot is null);
        }

        private XElement CreateItemGroup()
        {
            return new XElement(Name("ItemGroup"));
        }

        private string GetLabel(XElement element)
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
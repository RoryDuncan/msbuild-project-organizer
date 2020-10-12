using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using CSProjOrganizer.Interfaces;
using CSProjOrganizer.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace CSProjOrganizer.Services
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
        public bool Group(XDocument document, SortOptions options)
        {
            if (!HasProjectRoot(document))
            {
                _logger.LogInformation("No <Project> found within document. Nothing to sort.");
                return false;
            }

            if (options.GroupByNodeType)
            {
                this.GroupByNodeType(document);
            }

            if (options.GroupByFileType)
            {
                this.GroupByFileType(document);
            }

            if (options.RemoveEmptyItemGroups)
            {
                this.RemoveEmptyItemGroups(document);
            }

            if (options.SortItemsWithinItemGroups)
            {
                this.SortItemGroupItems(document);
            }

            return true;
        }

        /// <summary>
        /// Groups and sorts the XDocument's ItemGroup. Returns a bool indicating if the document was modified or not.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public void GroupByNodeType(XDocument document)
        {
            var (projectRoot, itemGroups) = GetRootAndItemGroups(document);

            // only operate on ItemGroups without "Condition" attributes
            var initialGroups = itemGroups.Where(group => group.Attribute("Condition") is null);

            List<XElement> itemGroupChildren = initialGroups.Elements().ToList();

            // group on the name of the node, like <Content />
            var nodeGroupings = itemGroupChildren
                .GroupBy(el => el.Name.LocalName)
                .Select(group =>
                {
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

            // Sort nodeGroupings by the name of their first child
            nodeGroupings = nodeGroupings.OrderBy(group => group.Elements().First().Name.LocalName).ToList();

            // and add them in their new groupings
            foreach (XElement group in nodeGroupings)
            {
                if (group.HasElements)
                {
                    // Add a label corrosponding to the itemgroup's contents
                    string label = this.GetItemGroupComment(group.Elements().First());

                    var comment = new XComment($" {label} ");
                    projectRoot.Add(comment);
                    projectRoot.Add(group);
                }
            }

        }

        /// <summary>
        /// Groups items within ItemGroups into new ItemGroups based on file type and any custom groupings provided in <see cref="AppSettings" />
        /// </summary>
        public void GroupByFileType(XDocument document)
        {
            var (projectRoot, itemGroups) = GetRootAndItemGroups(document);

            int initialCount = itemGroups.Count;
            _logger.LogInformation($"There {(initialCount == 1 ? "is" : "are")} {initialCount} <ItemGroup> node{(initialCount == 1 ? string.Empty : "s")}");

            itemGroups
                .Where(itemGroup => itemGroup.HasElements && this.IsItemWithFileTypeAttributes(itemGroup.Elements().First()))
                .ToList()
                .ForEach(itemGroup => OrganizeItemGroup(itemGroup));

            // log how it's changed
            int resultingCount = projectRoot.Descendants("ItemGroup").ToList().Count;
            _logger.LogInformation($"There {(resultingCount == 1 ? "is" : "are")} {(resultingCount == initialCount ? "still" : "now")} {resultingCount} <ItemGroup> node{(resultingCount == 1 ? string.Empty : "s")}");
        }

        /// <summary>
        /// Removes Empty ItemGroups from the XDocument
        /// </summary>
        public void RemoveEmptyItemGroups(XDocument document)
        {
            var (projectRoot, itemGroups) = GetRootAndItemGroups(document);

            itemGroups
                .Where(g => !g.HasElements)
                .ToList()
                .ForEach(emptyGroup => emptyGroup.Remove());
        }

        public void SortItemGroupItems(XDocument document)
        {
            var (projectRoot, itemGroups) = GetRootAndItemGroups(document);

            itemGroups.ForEach(group => this.SortItemGroup(group));
        }

        private void SortItemGroup(XElement itemGroup)
        {
            var children = itemGroup.Elements().ToList();

            var sortedItems = itemGroup.Elements()
                .OrderBy(item => this.GetFileExtension(this.GetFilePath(item)))
                .ThenBy(item => this.GetFilePath(item))
                .ToList();

            // replaceAll will remove all attributes, so we need to copy them first
            IEnumerable<XAttribute> attributes = itemGroup.Attributes().ToList();

            // update with our sorted children
            itemGroup.ReplaceAll(sortedItems);

            // and re-apply them
            itemGroup.ReplaceAttributes(attributes);

        }

        private (XElement projectRoot, List<XElement> itemGroups) GetRootAndItemGroups(XDocument document)
        {
            XElement projectRoot = document.Element("Project");
            List<XElement> itemGroups = projectRoot.Descendants("ItemGroup").ToList();

            return (projectRoot, itemGroups);
        }

        private void OrganizeItemGroup(XElement itemGroup)
        {
            Dictionary<string, XElement> newItemGroups = new Dictionary<string, XElement>();
            string previousLabel = (string)itemGroup.Attribute("Label") ?? string.Empty;
            // for each item of the group, check it's file type and add it to an itemgroup of similar filetypes
            itemGroup.Elements().ToList().ForEach(element =>
            {
                string filePath = this.GetFilePath(element) ?? null;
                string fileType = this.GetFileExtension(filePath) ?? null;
                string label = this.GetGroupingLabelOrDefault(fileType, filePath);
                string key = label ?? "__misc__";

                if (!newItemGroups.ContainsKey(key))
                {
                    XElement value = this.CreateItemGroup();

                    if (label != null)
                    {
                        value.SetAttributeValue("Label", label);
                    }

                    newItemGroups.Add(key, value);
                }

                newItemGroups
                    .GetValueOrDefault(key)
                    .Add(element);
            });

            if (newItemGroups.Count > 0)
            {
                // add the new filetype-grouped <ItemGroup>'s immediately after this group

                // add back to document
                newItemGroups
                    .ToList()
                    .ForEach(kvp => itemGroup.AddAfterSelf(kvp.Value));

                // remove the original <ItemGroup>
                itemGroup.Remove();
            }
        }

        /// <summary>
        /// Fetches the label that should be used for an ItemGroup based on a variety of factors.
        /// </summary>
        /// <returns>A string or null</returns>
        private string GetGroupingLabelOrDefault(string fileType, string filePath)
        {
            // when filepath is null, we won't create a label
            if (filePath is null)
            {
                return null;
            }

            // check to see if this is a GLOB pattern
            if (fileType is null && filePath != null && filePath.Contains("*"))
            {
                return _config.GlobPatternLabel;
            }

            if (fileType is null)
            {
                return _config.UnknownFileExtensionLabel;
            }

            if (TryGetGrouping(_config.FileTypeGroupings, fileType, out string label))
            {
                return label;
            }

            return $"{fileType} files";
        }

        /// <summary>
        /// Gets a label of a grouping for a specific filetype, if available. See <see cref="AppSettings.Groupings" />
        /// </summary>
        private bool TryGetGrouping(object configGroup, string value, out string label)
        {
            label = null;
            if (configGroup == null)
            {
                return false;
            }

            var grouping = configGroup as Dictionary<string, IEnumerable<string>>;

            var match = grouping
                .Where(kvp => kvp.Value.Contains(value, StringComparer.InvariantCultureIgnoreCase));

            if (match.Count() == 0)
            {
                return false;
            }

            label = string.Join(", ", match.First().Key);

            return true;
        }

        private string GetFilePath(XElement element)
        {
            XAttribute includeAttr = element.Attribute("Include");
            if (includeAttr is null)
            {
                return null;
            }

            return includeAttr.Value;
        }

        private string GetFileExtension(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return null;
            }

            return Path.GetExtension(filePath);
        }

        private bool IsItemWithFileTypeAttributes(XElement element)
        {
            if (_config.FileTypeItems.Count() > 0)
            {
                return _config.FileTypeItems.Contains(element.Name.LocalName);
            }

            return element.Attribute("Include") != null;
        }

        // in case we need items specifically from the MSBuild namespace
        private XName Name(string name)
        {
            return XName.Get(name, xmlns);
        }

        private bool HasProjectRoot(XDocument document)
        {
            XElement projectRoot = document.Element("Project");
            return !(projectRoot is null);
        }

        private XElement CreateItemGroup()
        {
            return new XElement("ItemGroup");
        }

        private string GetItemGroupComment(XElement element)
        {
            string itemGroupType = element.Name.LocalName;
            if (TryGetGrouping(_config.ItemGroupGroupings, itemGroupType, out string label))
            {
                return label;
            }

            return $"{element.Name.LocalName} Includes";
        }
    }
}
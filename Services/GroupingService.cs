using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using csproj_sorter.Enums;
using csproj_sorter.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace csproj_sorter.Services
{
    public interface IGroupingService
    {
        void Group(XDocument document, GroupBy grouping);
    }

    public class GroupingService : IGroupingService
    {
        private readonly ILogger<TestService> _logger;
        private readonly AppSettings _config;

        public GroupingService(ILogger<TestService> logger,
            IOptions<AppSettings> config)
        {
            _logger = logger;
            _config = config.Value;
        }

        public void Group(XDocument document, GroupBy grouping)
        {
            switch (grouping)
            {
                case GroupBy.FileType:
                    this.GroupByFileType(document);
                    break;
                case GroupBy.NodeType:
                    this.GroupByNodeType(document);
                    break;
            }
        }

        private void GroupByFileType(XDocument document)
        {
            throw new NotImplementedException();
        }

        public void GroupByNodeType(XDocument document)
        {
            var projectRoot = document.Element("Project");
            var initialGroups = projectRoot.Descendants("ItemGroup");
            List<XElement> allItems = initialGroups.Descendants().ToList();
            
            // an empty item group for us to copy from
            XElement emptyItemGroup = new XElement(XName.Get("ItemGroup", "http://schemas.microsoft.com/developer/msbuild/2003"));

            // group on the name of the node, like <Content />
            var itemGroups = allItems
                .GroupBy(el => el.NodeType)
                .Select(group => {
                    XElement itemGroup = new XElement(emptyItemGroup);

                        foreach (XElement item in group)
                        {
                            itemGroup.Add(item);
                        }

                    return itemGroup;
                });

            // remove the existing item groups
            initialGroups.Remove();

            // and add them in their new groupings, base on GroupBy.NodeType
            foreach (XElement group in itemGroups)
            {
                projectRoot.Add(group);
            }

            //document.Save(Console.Out);
            Console.WriteLine(document); 
        }
    }
}
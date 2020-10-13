using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using CSProjOrganizer.Interfaces;
using CSProjOrganizer.Models;
using CSProjOrganizer.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace CSProjOrganizer.Tests
{
    public class GroupingServiceTests
    {
        private readonly IGroupingService _groupingService;

        public GroupingServiceTests()
        {
            var logger = Mock.Of<ILogger<GroupingService>>();
            var config = SortConfiguration.CreateWithDefaults();

            _groupingService = new GroupingService(logger, Options.Create(config));
        }

        [Fact]
        public void HandlesEmptyProject()
        {
            var options = new SortOptions()
            {
                GroupByNodeType = true
            };


            var document = new XDocument(new XElement("Project"));

            bool result = _groupingService.Group(document, options);
        }


        [Fact]
        public void GroupByNodeType()
        {
            var options = new SortOptions()
            {
                GroupByNodeType = true,
            };


            var document = new XDocument(
                new XElement("Project",
                    new XElement("ItemGroup",
                        new XElement("Compile", new XAttribute("Include", "Alpha.cs")),
                        new XElement("Content", new XAttribute("Include", "Beta.html")),
                        new XElement("TypeScriptCompile", new XAttribute("Include", "Charlie.d.ts"))
                    )
                )
            );

            var wasModified = _groupingService.Group(document, options);

            Assert.True(wasModified, "The document wasnt modified");

            var itemGroups = document.Descendants("ItemGroup").ToList();

            // these could be more granular if some fixtures were setup
            Assert.True(itemGroups.Count() == 3, "Each Item was not placed into a separate ItemGroup");
            Assert.True(itemGroups.TrueForAll(itemGroup => itemGroup.Elements().Count() == 1), "Each ItemGroup has one child");
        }

        [Fact]
        public void GroupByFileType()
        {
            var options = new SortOptions()
            {
                GroupByFileType = true,
            };

            var document = new XDocument(
                new XElement("Project",
                    new XElement("ItemGroup",
                        new XElement("Content", new XAttribute("Include", "Alpha.cshtml")),
                        new XElement("Content", new XAttribute("Include", "Beta.html")),
                        new XElement("Content", new XAttribute("Include", "Charlie.js"))
                    )
                )
            );

            var wasModified = _groupingService.Group(document, options);

            Assert.True(wasModified, "The document wasnt modified");

            var itemGroups = document.Descendants("ItemGroup").ToList();

            // these could be more granular if some fixtures were setup
            Assert.True(itemGroups.Count() == 3, "Each Item was not placed into a separate ItemGroup");
            Assert.True(itemGroups.TrueForAll(itemGroup => itemGroup.Elements().Count() == 1), "Each ItemGroup has one child");
        }

        [Fact]
        public void RemoveEmptyItemGroups()
        {
            var options = new SortOptions()
            {
                RemoveEmptyItemGroups = true,
            };

            var document = new XDocument(
                new XElement("Project",
                    new XElement("ItemGroup"),
                    new XElement("ItemGroup",
                        new XElement("Content", new XAttribute("Include", "Alpha.cshtml"))
                    ),
                    new XElement("ItemGroup")
                )
            );

            var wasModified = _groupingService.Group(document, options);

            Assert.True(wasModified, "The document wasnt modified");

            var itemGroups = document.Descendants("ItemGroup").ToList();

            // these could be more granular if some fixtures were setup
            Assert.True(itemGroups.Count() == 1, "Empty ItemGroups were not removed");
            Assert.True(itemGroups.TrueForAll(itemGroup => itemGroup.Elements().Count() > 0), "Empty ItemGroup were not removed");
            Assert.True(itemGroups.First().Elements().First().Name.LocalName == "Content", "Existing Item was removed");
        }

        [Fact]
        public void SortItemsWithinItemGroups()
        {
            var options = new SortOptions()
            {
                SortItemsWithinItemGroups = true,
            };


            List<string> orderedFiles = new List<string>()
            {
                "Alpha.cs",
                "Beta.cs",
                "Charlie.cs",
                "Zeta.cs",
            };

            var document = new XDocument(
                new XElement("Project",
                    new XElement("ItemGroup",
                        new XElement("Compile", new XAttribute("Include", orderedFiles.ElementAt(1))),
                        new XElement("Compile", new XAttribute("Include", orderedFiles.ElementAt(0))),
                        new XElement("Compile", new XAttribute("Include", orderedFiles.ElementAt(3))),
                        new XElement("Compile", new XAttribute("Include", orderedFiles.ElementAt(2)))
                    )
                )
            );

            var wasModified = _groupingService.Group(document, options);

            Assert.True(wasModified, "The document wasnt modified");

            var itemGroups = document.Descendants("ItemGroup").ToList();
            var itemGroup = itemGroups.First();

            Assert.True(itemGroups.Count() == 1, "Multiple ItemGroups were created");
            var items = itemGroup.Elements();

            Assert.True(items.ElementAt(0).Attribute("Include").Value == orderedFiles.ElementAt(0), "ItemGroup wasn't sorted");
            Assert.True(items.ElementAt(1).Attribute("Include").Value == orderedFiles.ElementAt(1), "ItemGroup wasn't sorted");
            Assert.True(items.ElementAt(2).Attribute("Include").Value == orderedFiles.ElementAt(2), "ItemGroup wasn't sorted");
            Assert.True(items.ElementAt(3).Attribute("Include").Value == orderedFiles.ElementAt(3), "ItemGroup wasn't sorted");
        }
    }
}
using System;
using CSProjOrganizer.Interfaces;
using CSProjOrganizer.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CSProjOrganizer.Tests
{
    public class XMLServiceTests
    {

        private readonly IXmlService _xmlService;

        public XMLServiceTests()
        {
            var logger = Mock.Of<ILogger<XmlService>>();
            _xmlService = new XmlService(logger);
        }
        [Fact]
        public void CanLoadFile()
        {

        }
    }
}

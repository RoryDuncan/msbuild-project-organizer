using CSProjOrganizer.Interfaces;
using CSProjOrganizer.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CSProjOrganizer.Tests
{
    public class XmlServiceTests
    {

        private readonly IXmlService _xmlService;
        private string TestFile(string fileName)
        {
            return $"files/{fileName}";
        }

        public XmlServiceTests()
        {
            var logger = Mock.Of<ILogger<XmlService>>();
            _xmlService = new XmlService(logger);
        }
        [Fact]
        public void CanLoadFile()
        {
            _xmlService.GetDocument(TestFile("empty-project.csproj"));
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using CSProjOrganizer.Interfaces;
using CSProjOrganizer.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CSProjOrganizer.Tests
{
  public class XmlServiceTests : IDisposable
  {
    private readonly IXmlService _xmlService;

    private List<string> outputFiles { get; set; } = new List<string>();
    private string TestFile(string fileName)
    {
      return $"files/{fileName}";
    }

    private string OutputFile(string fileName)
    {
      string file = TestFile(fileName);
      outputFiles.Add(file);
      return file;
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

    [Fact]
    public void CanSaveFile()
    {
      XDocument document = new XDocument(new XElement("Message", "Hello World"));
      _xmlService.SaveDocument(OutputFile("saved-file.csproj"), document);
    }

    public void Dispose()
    {
      if (outputFiles.Count > 0)
      {
        Console.WriteLine($"Performing Teardown of {nameof(XmlServiceTests)}");
        foreach (string fileName in outputFiles)
        {
          File.Delete(fileName);
        }
      }

    }
  }
}

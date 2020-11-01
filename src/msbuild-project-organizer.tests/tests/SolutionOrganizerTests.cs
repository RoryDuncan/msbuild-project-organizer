using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using CSProjOrganizer.Interfaces;
using CSProjOrganizer.Models;
using CSProjOrganizer.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CSProjOrganizer.Tests
{
    public class SolutionOrganizerTests
    {

        private readonly SolutionOrganizer _organizer;
        private readonly Mock<ILogger<SolutionOrganizer>> _loggerMock;
        private readonly Mock<IProjectOrganizer> _projectMock;

        private readonly string solutionFile1 = "files/SolutionOrganizerTestsFile1.sln";
        private readonly string solutionFile2 = "files/SolutionOrganizerTestsFile2.sln";
        

        public SolutionOrganizerTests()
        {
            _loggerMock = new Mock<ILogger<SolutionOrganizer>>();
            _projectMock = new Mock<IProjectOrganizer>();
            _organizer = new SolutionOrganizer(_loggerMock.Object, _projectMock.Object);
        }


        [Fact]
        public void HandlesInvalidPath()
        {
            _organizer.Run("dksangklds;nagklasd;mgldasgkasd");

            _loggerMock.Verify(l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.AtLeastOnce());
        }

        [Fact]
        public void HandlesNonExistantFile()
        {
            string cwd = Directory.GetCurrentDirectory();
            Console.WriteLine(cwd);
            _organizer.Run("not-real.sln");

            _loggerMock.Verify(l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.AtLeastOnce());
        }

        [Fact]
        public void HandlesRelativePathWithoutWarnings()
        {
            _organizer.Run(solutionFile1);

            _loggerMock.Verify(l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Never);
        }

        [Fact]
        public void HandlesAbsolutePath()
        {
            string cwd = Directory.GetCurrentDirectory();
            _organizer.Run($"{cwd}/{solutionFile1}");

            _loggerMock.Verify(l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Never);
        }


        [Fact]
        public void HandlesSolutionHavingNonProjectDirectory()
        {
            _organizer.Run(solutionFile2);

            _loggerMock.Verify(l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Exactly(1));
        }

        [Fact]
        public void HandlesRelativePathWithoutErrors()
        {
            _organizer.Run(solutionFile1);

            _loggerMock.Verify(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Never);

            _loggerMock.Verify(l => l.Log(
                LogLevel.Critical,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Never);
        }

        [Fact]
        public void OutputsMessagingForEachProject()
        {
            int projectsInSolution = 3;
            _organizer.Run(solutionFile1);

            _loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.AtLeast(projectsInSolution));
        }
    }
}
using System;
using System.Collections.Generic;
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

        public SolutionOrganizerTests()
        {
            var logger = Mock.Of<ILogger<SolutionOrganizer>>();
            var projectOrganizer = Mock.Of<IProjectOrganizer>();

            _organizer = new SolutionOrganizer(logger, projectOrganizer);
        }
    }
}
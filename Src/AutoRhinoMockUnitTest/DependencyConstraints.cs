﻿using System.Linq;
using Xunit;

namespace Ploeh.AutoFixture.AutoRhinoMock.UnitTest
{
    public class DependencyConstraints
    {
        [Theory]
        [InlineData("FakeItEasy")]
        [InlineData("Moq")]
        [InlineData("xunit")]
        [InlineData("xunit.extensions")]
        public void AutoFixtureDoesNotReference(string assemblyName)
        {
            // Fixture setup
            // Exercise system
            var references = typeof(AutoRhinoMockCustomization).Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.DoesNotContain(references, an => an.Name == assemblyName);
            // Teardown
        }

        [Theory]
        [InlineData("FakeItEasy")]
        [InlineData("Moq")]
        public void AutoFixtureUnitTestsDoNotReference(string assemblyName)
        {
            // Fixture setup
            // Exercise system
            var references = this.GetType().Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.DoesNotContain(references, an => an.Name == assemblyName);
            // Teardown
        }
    }
}
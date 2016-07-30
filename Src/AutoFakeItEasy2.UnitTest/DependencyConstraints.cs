﻿using System.Linq;
using Xunit;
using Xunit.Extensions;

#pragma warning disable 618
namespace Ploeh.AutoFixture.AutoFakeItEasy2.UnitTest
{
  public class DependencyConstraints
    {
        [Theory]
        [InlineData("Rhino.Mocks")]
        [InlineData("Moq")]
        [InlineData("xunit")]
        [InlineData("xunit.extensions")]
        [InlineData("NSubstitute")]
        public void AutoFakeItEasyDoesNotReference(string assemblyName)
        {
            // Fixture setup
            // Exercise system
            var references = typeof(AutoFakeItEasyCustomization).Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.False(references.Any(an => an.Name == assemblyName));
            // Teardown
        }

        [Theory]
        [InlineData("Rhino.Mocks")]
        [InlineData("Moq")]
        [InlineData("NSubstitute")]
        public void AutoFakeItEasyUnitTestsDoNotReference(string assemblyName)
        {
            // Fixture setup
            // Exercise system
            var references = this.GetType().Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.False(references.Any(an => an.Name == assemblyName));
            // Teardown
        }
    }
}
#pragma warning restore 618
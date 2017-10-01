﻿using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.AutoFakeItEasy.UnitTest
{
    public class DependencyConstraints
    {
        [Theory]
        [InlineData("Rhino.Mocks")]
        [InlineData("Moq")]
        [InlineData("xunit")]
        [InlineData("xunit.extensions")]
        public void AutoFakeItEasyDoesNotReference(string assemblyName)
        {
            // Fixture setup
            // Exercise system
            var references = typeof(AutoFakeItEasyCustomization).GetTypeInfo().Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.DoesNotContain(references, an => an.Name == assemblyName);
            // Teardown
        }

        [Theory]
        [InlineData("Rhino.Mocks")]
        [InlineData("Moq")]
        public void AutoFakeItEasyUnitTestsDoNotReference(string assemblyName)
        {
            // Fixture setup
            // Exercise system
            var references = this.GetType().GetTypeInfo().Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.DoesNotContain(references, an => an.Name == assemblyName);
            // Teardown
        }
    }
}

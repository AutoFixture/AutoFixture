using System;
using NUnit.Framework;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.NUnit2.UnitTest
{
    [TestFixture]
    public class CustomizeAttributeTest
    {
        [Test]
        public void TestableSutIsSut()
        {
            // Fixture setup
            // Exercise system
            var sut = new DelegatingCustomizeAttribute();
            // Verify outcome
            Assert.IsInstanceOf<CustomizeAttribute>(sut);
            // Teardown
        }

        [Test]
        public void SutIsAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new DelegatingCustomizeAttribute();
            // Verify outcome
            Assert.IsInstanceOf<Attribute>(sut);
            // Teardown
        }

        [Test]
        public void SutImplementsIParameterCustomizationProviderToBeRecognizedByTestDataProvider()
        {
            // Fixture setup
            // Exercise system 
            // Verify outcome
            Assert.True(typeof(IParameterCustomizationProvider).IsAssignableFrom(typeof(CustomizeAttribute)));
            // Teardown
        }
    }
}

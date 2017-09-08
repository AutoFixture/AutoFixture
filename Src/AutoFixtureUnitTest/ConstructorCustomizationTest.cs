using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class ConstructorCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Fixture setup
            var dummyType = typeof(object);
            var dummyQuery = new DelegatingMethodQuery();
            // Exercise system
            var sut = new ConstructorCustomization(dummyType, dummyQuery);
            // Verify outcome
            Assert.IsAssignableFrom<ICustomization>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullTypeThrows()
        {
            // Fixture setup
            var dummyQuery = new DelegatingMethodQuery();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new ConstructorCustomization(null, dummyQuery));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullQueryThrows()
        {
            // Fixture setup
            var dummyType = typeof(object);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new ConstructorCustomization(dummyType, null));
            // Teardown
        }

        [Fact]
        public void TargetTypeIsCorrect()
        {
            // Fixture setup
            var expectedType = typeof(string);
            var dummyQuery = new DelegatingMethodQuery();
            var sut = new ConstructorCustomization(expectedType, dummyQuery);
            // Exercise system
            Type result = sut.TargetType;
            // Verify outcome
            Assert.Equal(expectedType, result);
            // Teardown
        }

        [Fact]
        public void QueryIsCorrect()
        {
            // Fixture setup
            var dummyType = typeof(object);
            var expectedQuery = new DelegatingMethodQuery();
            var sut = new ConstructorCustomization(dummyType, expectedQuery);
            // Exercise system
            IMethodQuery result = sut.Query;
            // Verify outcome
            Assert.Equal(expectedQuery, result);
            // Teardown
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Fixture setup
            var dummyType = typeof(object);
            var dummyQuery = new DelegatingMethodQuery();
            var sut = new ConstructorCustomization(dummyType, dummyQuery);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
            // Teardown
        }

        [Fact]
        public void CustomizeWithGreedyQueryCorrectlyCustomizesFixture()
        {
            // Fixture setup
            var fixture = new Fixture();

            var type = typeof(MultiUnorderedConstructorType);
            var query = new GreedyConstructorQuery();
            var sut = new ConstructorCustomization(type, query);
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var specimen = fixture.Create<MultiUnorderedConstructorType>();
            Assert.False(string.IsNullOrEmpty(specimen.Text));
            Assert.NotEqual(0, specimen.Number);
            // Teardown
        }

        [Fact]
        public void CustomizeWithModestQueryCorrectlyCustomizesFixture()
        {
            // Fixture setup
            var fixture = new Fixture();

            var type = typeof(MultiUnorderedConstructorType);
            var query = new ModestConstructorQuery();
            var sut = new ConstructorCustomization(type, query);
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var specimen = fixture.Create<MultiUnorderedConstructorType>();
            Assert.True(string.IsNullOrEmpty(specimen.Text));
            Assert.Equal(0, specimen.Number);
            // Teardown
        }
    }
}

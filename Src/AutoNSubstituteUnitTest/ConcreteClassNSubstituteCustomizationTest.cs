using System;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public class ConcreteClassNSubstituteCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            var dummyType = typeof(object);
            var sut = new ConcreteClassNSubstituteCustomization(dummyType);
            Assert.IsAssignableFrom<ICustomization>(sut);
        }

        [Fact]
        public void InitializeWithNullTypeThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ConcreteClassNSubstituteCustomization(null));
        }

        [Fact]
        public void InitializeWithTypeSetsType()
        {
            var expected = typeof(object);
            var sut = new ConcreteClassNSubstituteCustomization(expected);
            Assert.Same(expected, sut.Type);
        }

        [Fact]
        public void CustomizeWithNullFixtureThrows()
        {
            var dummyType = typeof(object);
            var sut = new ConcreteClassNSubstituteCustomization(dummyType);

            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
        }

        [Fact]
        public void CustomizedFixtureCreatesConcreteTypeSubstitute()
        {
            // Fixture setup
            var expectedBaseType = typeof(ConcreteType);
            var fixture = new Fixture();
            var sut = new ConcreteClassNSubstituteCustomization(expectedBaseType);
            // Exercise system 
            sut.Customize(fixture);
            // Verify outcome
            var actualBaseType = fixture.Create<ConcreteType>().GetType().BaseType;
            Assert.Equal(expectedBaseType, actualBaseType);
            // Teardown
        }
    }
}
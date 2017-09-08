using System;
using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class FreezingCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Fixture setup
            var dummyType = typeof(object);
            // Exercise system
            var sut = new FreezingCustomization(dummyType);
            // Verify outcome
            Assert.IsAssignableFrom<ICustomization>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullTargetTypeThrowsArgumentNullException()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new FreezingCustomization(null));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullRegisteredTypeThrowsArgumentNullException()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new FreezingCustomization(typeof(object), null));
            // Teardown
        }

        [Fact]
        public void InitializeWithRegisteredTypeIncompatibleWithTargetTypeThrowsArgumentException()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() =>
                new FreezingCustomization(typeof(int), typeof(string)));
            // Teardown
        }

        [Fact]
        public void TargetTypeIsCorrect()
        {
            // Fixture setup
            var expectedType = typeof(string);
            var sut = new FreezingCustomization(expectedType);
            // Exercise system
            Type result = sut.TargetType;
            // Verify outcome
            Assert.Equal(expectedType, result);
            // Teardown
        }

        [Fact]
        public void RegisteredTypeIsCorrect()
        {
            // Fixture setup
            var targetType = typeof(string);
            var registeredType = typeof(object);
            var sut = new FreezingCustomization(targetType, registeredType);
            // Exercise system
            Type result = sut.RegisteredType;
            // Verify outcome
            Assert.Equal(registeredType, result);
            // Teardown
        }

        [Fact]
        public void TargetTypeAndRegisteredTypeAreCorrect()
        {
            // Fixture setup
            var targetType = typeof(string);
            var registeredType = typeof(object);
            var sut = new FreezingCustomization(targetType, registeredType);
            // Exercise system and verify outcome
            Assert.Equal(targetType, sut.TargetType);
            Assert.Equal(registeredType, sut.RegisteredType);
            // Teardown
        }

        [Fact]
        public void TargetTypeIsTheSameAsRegisteredTypeWhenOnlyTargetTypeIsSpecified()
        {
            // Fixture setup
            var targetType = typeof(string);
            var sut = new FreezingCustomization(targetType);
            // Exercise system and verify outcome
            Assert.Equal(sut.TargetType, sut.RegisteredType);
            // Teardown
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Fixture setup
            var dummyType = typeof(object);
            var sut = new FreezingCustomization(dummyType);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
            // Teardown
        }

        [Fact]
        public void CustomizeCorrectlyCustomizesFixture()
        {
            // Fixture setup
            var targetType = typeof(int);
            var fixture = new Fixture();

            var sut = new FreezingCustomization(targetType);
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var i1 = fixture.Create<int>();
            var i2 = fixture.Create<int>();
            Assert.Equal(i1, i2);
            // Teardown
        }

        [Fact]
        public void CustomizeWithRegisteredTypeCorrectlyCustomizesFixture()
        {
            // Fixture setup
            var targetType = typeof(int);
            var registeredType = typeof(object);
            var fixture = new Fixture();
            var sut = new FreezingCustomization(targetType, registeredType);
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            object i1 = fixture.Create<int>();
            object i2 = fixture.Create<object>();
            Assert.Equal(i1, i2);
            // Teardown
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixture.Xunit.UnitTest
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
        public void InitializeWithNullTypeThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new FreezingCustomization(null));
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
            var i1 = fixture.CreateAnonymous<int>();
            var i2 = fixture.CreateAnonymous<int>();
            Assert.Equal(i1, i2);
            // Teardown
        }
    }
}

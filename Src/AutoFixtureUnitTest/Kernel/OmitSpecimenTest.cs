using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class OmitSpecimenTest
    {
        [Fact]
        public void SutIsEquatable()
        {
            var sut = new OmitSpecimen();
            Assert.IsAssignableFrom<IEquatable<OmitSpecimen>>(sut);
        }

        [Fact]
        public void SutDoesNotEqualNull()
        {
            // Fixture setup
            var sut = new OmitSpecimen();
            // Exercise system
            var actual = BothEquals(sut, null);
            // Verify outcome
            Assert.False(actual.Any(b => b));
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualAnonymousObject()
        {
            // Fixture setup
            var sut = new OmitSpecimen();
            // Exercise system
            var anonymous = new object();
            var actual = sut.Equals(anonymous);
            // Verify outcome
            Assert.False(actual);
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherSut()
        {
            // Fixture setup
            var sut = new OmitSpecimen();
            var other = new OmitSpecimen();
            // Exercise system
            var actual = BothEquals(sut, other);
            // Verify outcome
            Assert.True(actual.All(b => b));
            // Teardown
        }

        [Fact]
        public void GetHashCodeIsStableAcrossInstances()
        {
            // Fixture setup
            var sut = new OmitSpecimen();
            // Exercise system
            var actual = sut.GetHashCode();
            // Verify outcome
            var expected = new OmitSpecimen().GetHashCode();
            Assert.Equal(expected, actual);
            // Teardown
        }

        private static IEnumerable<bool> BothEquals<T>(T sut, T other) where T : IEquatable<T>
        {
            yield return sut.Equals((object)other);
            yield return sut.Equals(other);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    public class EqualityComparerEqualsSelfAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Arrange
            var dummyComposer = new Fixture();
            // Act
            var sut = new EqualityComparerEqualsSelfAssertion(dummyComposer);
            // Assert
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
        }

        [Fact]
        public void ComposerIsCorrect()
        {
            // Arrange
            var expectedComposer = new Fixture();
            var sut = new EqualityComparerEqualsSelfAssertion(expectedComposer);
            // Act
            ISpecimenBuilder result = sut.Builder;
            // Assert
            Assert.Equal(expectedComposer, result);
        }

        [Fact]
        public void ConstructWithNullComposerThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new EqualityComparerEqualsSelfAssertion(null));
        }

        [Fact]
        public void VerifyNullMethodThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new EqualityComparerEqualsSelfAssertion(dummyComposer);
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((MethodInfo)null));
        }

        [Fact]
        public void VerifyNonEqualityComparerDoesNothing()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new EqualityComparerEqualsSelfAssertion(dummyComposer);
            // Act & Assert
            Assert.Null(Record.Exception(() =>
                sut.Verify(typeof(NonEqualityComparer))));
        }

        [Fact]
        public void VerifyWellBehavedEqualityComparerDoesNotThrow()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new EqualityComparerEqualsSelfAssertion(dummyComposer);
            // Act & Assert
            Assert.Null(Record.Exception(() =>
                sut.Verify(typeof(WellBehavedEqualityComparer))));
        }

        [Fact]
        public void VerifyIllBehavedEqualityComparerThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new EqualityComparerEqualsSelfAssertion(dummyComposer);
            // Act & Assert
            Assert.Throws<EqualityComparerImplementationException>(() =>
                sut.Verify(typeof(IllBehavedEqualityComparer)));
        }

#pragma warning disable 659
        private class WellBehavedEqualityComparer : IEqualityComparer<PropertyHolder<int>>
        {
            public bool Equals(PropertyHolder<int> x, PropertyHolder<int> y)
            {
                if (ReferenceEquals(x, y))
                    return true;

                if (ReferenceEquals(x, null))
                    return false;

                if (ReferenceEquals(y, null))
                    return false;

                if (x.GetType() != y.GetType())
                    return false;

                return x.Property == y.Property;
            }

            public int GetHashCode(PropertyHolder<int> obj)
            {
                return obj.Property;
            }
        }

        private class IllBehavedEqualityComparer : IEqualityComparer<PropertyHolder<int>>
        {
            public bool Equals(PropertyHolder<int> x, PropertyHolder<int> y)
            {
                if (ReferenceEquals(x, y))
                    return false;

                return true;
            }

            public int GetHashCode(PropertyHolder<int> obj)
            {
                throw new Exception();
            }
        }
#pragma warning restore 659

        private class NonEqualityComparer
        {
        }
    }
}
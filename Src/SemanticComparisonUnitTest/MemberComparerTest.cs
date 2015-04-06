using Ploeh.TestTypeFoundation;
using System;
using System.Reflection;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.SemanticComparison.UnitTest
{
    public class MemberComparerTest
    {
        [Fact]
        public void SutIsMemberComparer()
        {
            // Fixture setup
            var dummyComparer = new DelegatingEqualityComparer();
            var sut = new MemberComparer(dummyComparer);
            // Exercise system and verify outcome
            Assert.IsAssignableFrom<IMemberComparer>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullEqualityComparerThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new MemberComparer(null));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullPropertySpecificationThrows()
        {
            // Fixture setup
            var dummyComparer = new DelegatingEqualityComparer();
            var dummySpecification = new DelegatingSpecification<FieldInfo>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new MemberComparer(
                    dummyComparer,
                    null,
                    dummySpecification));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullFieldSpecificationThrows()
        {
            // Fixture setup
            var dummyComparer = new DelegatingEqualityComparer();
            var dummySpecification = new DelegatingSpecification<PropertyInfo>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new MemberComparer(
                    dummyComparer,
                    dummySpecification,
                    null));
            // Teardown
        }

        [Fact]
        public void ComparerIsCorrect()
        {
            // Fixture setup
            var expected = new DelegatingEqualityComparer();
            var sut = new MemberComparer(expected);
            // Exercise system
            var result = sut.Comparer;
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void PropertySpecificationIsCorrect()
        {
            // Fixture setup
            var dummyComparer = new DelegatingEqualityComparer();
            var expected = new DelegatingSpecification<PropertyInfo>();
            var dummySpecification = new DelegatingSpecification<FieldInfo>();

            var sut = new MemberComparer(
                dummyComparer,
                expected,
                dummySpecification);
            // Exercise system
            var result = sut.PropertySpecification;
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void FieldSpecificationIsCorrect()
        {
            // Fixture setup
            var dummyComparer = new DelegatingEqualityComparer();
            var expected = new DelegatingSpecification<FieldInfo>();
            var dummySpecification = new DelegatingSpecification<PropertyInfo>();

            var sut = new MemberComparer(
                dummyComparer,
                dummySpecification,
                expected);
            // Exercise system
            var result = sut.FieldSpecification;
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithDefaultSpecificationForPropertyReturnsCorrectResult()
        {
            // Fixture setup
            var property = typeof(PropertyHolder<int>).GetProperty("Property");
            var dummyComparer = new DelegatingEqualityComparer();
            var sut = new MemberComparer(dummyComparer);
            // Exercise system
            var result = sut.IsSatisfiedBy(property);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithDefaultSpecificationForFieldReturnsCorrectResult()
        {
            // Fixture setup
            var field = typeof(FieldHolder<int>).GetProperty("Field");
            var dummyComparer = new DelegatingEqualityComparer();
            var sut = new MemberComparer(dummyComparer);
            // Exercise system
            var result = sut.IsSatisfiedBy(field);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsSatisfiedByForPropertyReturnsCorrectResult(bool expected)
        {
            // Fixture setup
            var property = typeof(PropertyHolder<int>).GetProperty("Property");
            var dummyComparer = new DelegatingEqualityComparer();
            var dummySpecification = new DelegatingSpecification<FieldInfo>();

            var propertySpecificationStub =
                new DelegatingSpecification<PropertyInfo>
                {
                    OnIsSatisfiedBy = x => expected
                };

            var sut = new MemberComparer(
                dummyComparer,
                propertySpecificationStub,
                dummySpecification);
            // Exercise system
            var result = sut.IsSatisfiedBy(property);
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsSatisfiedByForFieldReturnsCorrectResult(bool expected)
        {
            // Fixture setup
            var field = typeof(FieldHolder<int>).GetField("Field");
            var dummyComparer = new DelegatingEqualityComparer();
            var dummySpecification = new DelegatingSpecification<PropertyInfo>();

            var fieldSpecificationStub =
                new DelegatingSpecification<FieldInfo>
                {
                    OnIsSatisfiedBy = x => expected
                };

            var sut = new MemberComparer(
                dummyComparer,
                dummySpecification,
                fieldSpecificationStub);
            // Exercise system
            var result = sut.IsSatisfiedBy(field);
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Theory]
        [InlineData(123, 123, true)]
        [InlineData(123, 321, false)]
        public void EqualsForwardsCorrectCallToComparer(
            object a,
            object b,
            bool expected)
        {
            // Fixture setup
            var comparerStub = new DelegatingEqualityComparer
            {
                OnEquals = (x, y) => x.Equals(y)
            };
            var sut = new MemberComparer(comparerStub);
            // Exercise system
            var result = sut.Equals(a, b);
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Theory]
        [InlineData("a", 1)]
        [InlineData(123, 2)]
        public void GetHashCodeForwardsCorrectCallToComparer(
            object obj,
            int expected)
        {
            // Fixture setup
            var comparerStub = new DelegatingEqualityComparer
            {
                OnGetHashCode = x => x == obj ? expected : 0
            };
            var sut = new MemberComparer(comparerStub);
            // Exercise system
            var result = sut.GetHashCode(obj);
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }
    }
}
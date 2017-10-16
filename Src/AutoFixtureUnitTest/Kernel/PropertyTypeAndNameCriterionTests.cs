using System;
using System.Reflection;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class PropertyTypeAndNameCriterionTests
    {
        [Fact]
        public void SutIsPropertyInfoEquatable()
        {
            var sut = new PropertyTypeAndNameCriterion(
                new DelegatingCriterion<Type>(),
                new DelegatingCriterion<string>());
            Assert.IsAssignableFrom<IEquatable<PropertyInfo>>(sut);
        }

        [Theory]
        [InlineData(false, false, false)]
        [InlineData(true,  false, false)]
        [InlineData(false, true,  false)]
        [InlineData(true,  true,  true )]
        public void EqualsReturnsCorrectResult(
            bool typeResult,
            bool nameResult,
            bool expected)
        {
            var prop = typeof(string).GetProperty("Length");
            var typeCriterion = new DelegatingCriterion<Type>
            {
                OnEquals = t =>
                {
                    Assert.Equal(prop.PropertyType, t);
                    return typeResult;
                }
            };
            var nameCriterion = new DelegatingCriterion<string>
            {
                OnEquals = n =>
                {
                    Assert.Equal(prop.Name, n);
                    return nameResult;
                }
            };
            var sut = 
                new PropertyTypeAndNameCriterion(
                    typeCriterion,
                    nameCriterion);

            var actual = sut.Equals(prop);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SutDoesNotEqualNullPropertyInfo()
        {
            var sut = new PropertyTypeAndNameCriterion(
                new DelegatingCriterion<Type>(),
                new DelegatingCriterion<string>());
            var actual = sut.Equals((PropertyInfo)null);
            Assert.False(actual, "SUT shouldn't equal null property.");
        }

        [Fact]
        public void ConstructWithNullTypeCriterionThrows()
        {
            Assert.Throws<ArgumentNullException>(
                () => new PropertyTypeAndNameCriterion(
                    null,
                    new DelegatingCriterion<string>()));
        }

        [Fact]
        public void ConstructWithNullNameCriterionThrows()
        {
            Assert.Throws<ArgumentNullException>(
                () => new PropertyTypeAndNameCriterion(
                    new DelegatingCriterion<Type>(),
                    null));
        }

        [Fact]
        public void SutEqualsIdenticalValue()
        {
            var typeCriterion = new DelegatingCriterion<Type>();
            var nameCriterion = new DelegatingCriterion<string>();
            var sut =
                new PropertyTypeAndNameCriterion(typeCriterion, nameCriterion);

            var other =
                new PropertyTypeAndNameCriterion(typeCriterion, nameCriterion);
            var actual = sut.Equals(other);

            Assert.True(actual, "Expected structural equality to hold.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData("foo")]
        [InlineData(typeof(Version))]
        public void SutDoesNotEqualAnyObject(object other)
        {
            var sut = new PropertyTypeAndNameCriterion(
                new DelegatingCriterion<Type>(),
                new DelegatingCriterion<string>());
            var actual = sut.Equals(other);
            Assert.False(actual, "SUT should not equal object of other type.");
        }

        [Fact]
        public void SutDoesNotEqualOtherWhenTypeCriterionDiffers()
        {
            var nameCriterion = new DelegatingCriterion<string>();
            var sut = new PropertyTypeAndNameCriterion(
                new DelegatingCriterion<Type>(),
                nameCriterion);

            var other = new PropertyTypeAndNameCriterion(
                new DelegatingCriterion<Type>(),
                nameCriterion);
            var actual = sut.Equals(other);

            Assert.False(
                actual,
                "SUT should not equal other when type criterion differs.");
        }

        [Fact]
        public void SutDoesNotEqualOtherWhenNameCriterionDiffers()
        {
            var typeCriterion = new DelegatingCriterion<Type>();
            var sut = new PropertyTypeAndNameCriterion(
                typeCriterion,
                new DelegatingCriterion<string>());

            var other = new PropertyTypeAndNameCriterion(
                typeCriterion,
                new DelegatingCriterion<string>());
            var actual = sut.Equals(other);

            Assert.False(
                actual,
                "SUT should not equal other when name criterion differs.");
        }

        [Fact]
        public void TypeCriterionIsCorrect()
        {
            var expected = new DelegatingCriterion<Type>();
            var sut = new PropertyTypeAndNameCriterion(
                expected,
                new DelegatingCriterion<string>());

            IEquatable<Type> actual = sut.TypeCriterion;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NameCriterionIsCorrect()
        {
            var expected = new DelegatingCriterion<string>();
            var sut = new PropertyTypeAndNameCriterion(
                new DelegatingCriterion<Type>(),
                expected);

            IEquatable<string> actual = sut.NameCriterion;

            Assert.Equal(expected, actual);
        }
    }
}

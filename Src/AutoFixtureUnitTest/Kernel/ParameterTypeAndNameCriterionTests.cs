using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class ParameterTypeAndNameCriterionTests
    {
        [Fact]
        public void SutIsParameterInfoEquatable()
        {
            var sut = new ParameterTypeAndNameCriterion(
                new DelegatingCriterion<Type>(),
                new DelegatingCriterion<string>());
            Assert.IsAssignableFrom<IEquatable<ParameterInfo>>(sut);
        }

        [Theory]
        [InlineData(false, false, false)]
        [InlineData(true,  false, false)]
        [InlineData(false, true,  false)]
        [InlineData(true,  true,  true)]
        public void EqualsReturnsCorrectResult(
            bool typeResult,
            bool nameResult,
            bool expected)
        {
            var parameter = 
                typeof(string).GetMethod("Contains").GetParameters().First();
            var typeCriterion = new DelegatingCriterion<Type>
            {
                OnEquals = t =>
                {
                    Assert.Equal(parameter.ParameterType, t);
                    return typeResult;
                }
            };
            var nameCriterion = new DelegatingCriterion<string>
            {
                OnEquals = n =>
                {
                    Assert.Equal(parameter.Name, n);
                    return nameResult;
                }
            };
            var sut =
                new ParameterTypeAndNameCriterion(
                    typeCriterion,
                    nameCriterion);

            var actual = sut.Equals(parameter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SutDoesNotEqualNullParameterInfo()
        {
            var sut = new ParameterTypeAndNameCriterion(
                new DelegatingCriterion<Type>(),
                new DelegatingCriterion<string>());
            var actual = sut.Equals((ParameterInfo)null);
            Assert.False(actual, "SUT shouldn't equal null parameter.");
        }

        [Fact]
        public void ConstructWithNullTypeCriterionThrows()
        {
            Assert.Throws<ArgumentNullException>(
                () => new ParameterTypeAndNameCriterion(
                    null,
                    new DelegatingCriterion<string>()));
        }

        [Fact]
        public void ConstructWithNullNameCriterionThrows()
        {

            Assert.Throws<ArgumentNullException>(
                () => new ParameterTypeAndNameCriterion(
                    new DelegatingCriterion<Type>(),
                    null));
        }

        [Fact]
        public void SutEqualsIdenticalValue()
        {
            var typeCriterion = new DelegatingCriterion<Type>();
            var nameCriterion = new DelegatingCriterion<string>();
            var sut =
                new ParameterTypeAndNameCriterion(typeCriterion, nameCriterion);

            var other =
                new ParameterTypeAndNameCriterion(typeCriterion, nameCriterion);
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
            var sut = new ParameterTypeAndNameCriterion(
                new DelegatingCriterion<Type>(),
                new DelegatingCriterion<string>());
            var actual = sut.Equals(other);
            Assert.False(actual, "SUT should not equal object of other type.");
        }

        [Fact]
        public void SutDoesNotEqualOtherWhenTypeCriterionDiffers()
        {
            var nameCriterion = new DelegatingCriterion<string>();
            var sut = new ParameterTypeAndNameCriterion(
                new DelegatingCriterion<Type>(),
                nameCriterion);

            var other = new ParameterTypeAndNameCriterion(
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
            var sut = new ParameterTypeAndNameCriterion(
                typeCriterion,
                new DelegatingCriterion<string>());

            var other = new ParameterTypeAndNameCriterion(
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
            var sut = new ParameterTypeAndNameCriterion(
                expected,
                new DelegatingCriterion<string>());

            IEquatable<Type> actual = sut.TypeCriterion;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NameCriterionIsCorrect()
        {
            var expected = new DelegatingCriterion<string>();
            var sut = new ParameterTypeAndNameCriterion(
                new DelegatingCriterion<Type>(),
                expected);

            IEquatable<string> actual = sut.NameCriterion;

            Assert.Equal(expected, actual);
        }
    }
}

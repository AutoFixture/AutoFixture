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
    }
}

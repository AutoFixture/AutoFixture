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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class FieldTypeAndNameCriterionTests
    {
        [Fact]
        public void SutIsFieldInfoEquatable()
        {
            var sut = new FieldTypeAndNameCriterion(
                new DelegatingCriterion<Type>(),
                new DelegatingCriterion<string>());
            Assert.IsAssignableFrom<IEquatable<FieldInfo>>(sut);
        }

        [Theory]
        [InlineData(false, false, false)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(true, true, true)]
        public void EqualsReturnsCorrectResult(
            bool typeResult,
            bool nameResult,
            bool expected)
        {
            var field = typeof(string).GetField("Empty");
            var typeCriterion = new DelegatingCriterion<Type>
            {
                OnEquals = t =>
                {
                    Assert.Equal(field.FieldType, t);
                    return typeResult;
                }
            };
            var nameCriterion = new DelegatingCriterion<string>
            {
                OnEquals = n =>
                {
                    Assert.Equal(field.Name, n);
                    return nameResult;
                }
            };
            var sut =
                new FieldTypeAndNameCriterion(
                    typeCriterion,
                    nameCriterion);

            var actual = sut.Equals(field);

            Assert.Equal(expected, actual);
        }
    }
}

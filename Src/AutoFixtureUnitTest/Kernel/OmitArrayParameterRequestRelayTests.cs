using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Xunit.Extensions;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class OmitArrayParameterRequestRelayTests
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            var sut = new OmitArrayParameterRequestRelay();
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Theory]
        [InlineData(typeof(object[]))]
        [InlineData(typeof(string[]))]
        [InlineData(typeof(int[]))]
        [InlineData(typeof(Version[]))]
        [InlineData(typeof(SingleParameterType<string>[]))]
        public void CreateWithEnumerableParameterReturnsCorrectResult(
            Type argumentType)
        {
            var parameterInfo =
                typeof(SingleParameterType<>)
                    .MakeGenericType(new[] { argumentType })
                    .GetConstructors()
                    .First()
                    .GetParameters()
                    .First();
            var expected = new object();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    Assert.Equal(
                        new SeededRequest(
                            parameterInfo.ParameterType,
                            parameterInfo.Name),
                        r);
                    return expected;
                }
            };
            var sut = new OmitArrayParameterRequestRelay();

            var actual = sut.Create(parameterInfo, context);

            Assert.Equal(expected, actual);
        }
    }
}

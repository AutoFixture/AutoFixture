using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class OmitEnumerableParameterRequestRelayTests
    {
        [Fact]
        public void SutIsSpecimentBuilder()
        {
            var sut = new OmitEnumerableParameterRequestRelay();
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithEnumerableParameterReturnsCorrectResult()
        {
            var parameterInfo = 
                typeof(SingleParameterType<IEnumerable<string>>)
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
            var sut = new OmitEnumerableParameterRequestRelay();

            var actual = sut.Create(parameterInfo, context);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(false)]
        [InlineData(true)]
        [InlineData(0)]
        [InlineData(42)]
        [InlineData("")]
        [InlineData("Foo")]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        public void CreateReturnsCorrectResultForNonParameterRequest(
            object request)
        {
            var sut = new OmitEnumerableParameterRequestRelay();
            var actual = sut.Create(request, new DelegatingSpecimenContext());
            Assert.Equal(new NoSpecimen(request), actual);
        }
    }
}

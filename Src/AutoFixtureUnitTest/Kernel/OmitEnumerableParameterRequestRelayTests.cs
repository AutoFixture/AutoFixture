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

        [Theory]
        [InlineData(typeof(IEnumerable<object>))]
        [InlineData(typeof(IEnumerable<string>))]
        [InlineData(typeof(IEnumerable<int>))]
        [InlineData(typeof(IEnumerable<Version>))]
        [InlineData(typeof(IEnumerable<SingleParameterType<string>>))]
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

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        [InlineData(typeof(SingleParameterType<string>))]
        public void CreateWithNonEnumerableParameterRequestReturnsCorrectResult(
            Type argumentType)
        {
            var parameterInfo =
                typeof(SingleParameterType<>)
                    .MakeGenericType(new[] { argumentType })
                    .GetConstructors()
                    .First()
                    .GetParameters()
                    .First();
            var sut = new OmitEnumerableParameterRequestRelay();

            var dummyContext = new DelegatingSpecimenContext();
            var actual = sut.Create(parameterInfo, dummyContext);

            var expected = new NoSpecimen(parameterInfo);
            Assert.Equal(expected, actual);
        }
    }
}

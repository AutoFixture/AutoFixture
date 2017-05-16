using System;
using System.Linq;
using System.Reflection;
using Ploeh.Albedo;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class ParameterInfoElementRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            var sut = new ParameterInfoElementRelay();
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestThrows()
        {
            var context = new DelegatingSpecimenContext();
            var sut = new ParameterInfoElementRelay();
            var ex = Assert.Throws<ArgumentNullException>(() =>
                sut.Create(null, context));
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            var request = new object();
            var sut = new ParameterInfoElementRelay();
            var ex = Assert.Throws<ArgumentNullException>(() =>
                sut.Create(request, null));
            Assert.Equal("context", ex.ParamName);
        }

        [Fact]
        public void CreateWithNotParameterInfoElementReturnsNoSpecimen()
        {
            var request = new object();
            var context = new DelegatingSpecimenContext();
            var sut = new ParameterInfoElementRelay();

            var result = sut.Create(request, context);

            Assert.IsType<NoSpecimen>(result);
        }

        [Fact]
        public void CreateWithParameterInfoElementResolvesSpecimen()
        {
            var expected = new object();
            var request = new ParameterInfoElement(
                typeof(object).GetMethod("Equals", BindingFlags.Instance | BindingFlags.Public)
                .GetParameters().First());
            var context = new DelegatingSpecimenContext { OnResolve = r => expected };
            var sut = new ParameterInfoElementRelay();

            var result = sut.Create(request, context);

            Assert.Same(expected, result);
        }
    }
}
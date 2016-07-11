using System;
using Ploeh.Albedo;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class TypeElementRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            var sut = new TypeElementRelay();
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestThrows()
        {
            var context = new DelegatingSpecimenContext();
            var sut = new TypeElementRelay();
            var ex = Assert.Throws<ArgumentNullException>(() =>
                sut.Create(null, context));
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            var request = new object();
            var sut = new TypeElementRelay();
            var ex = Assert.Throws<ArgumentNullException>(() =>
                sut.Create(request, null));
            Assert.Equal("context", ex.ParamName);
        }

        [Fact]
        public void CreateWithNotTypeElementReturnsNoSpecimen()
        {
            var request = new object();
            var context = new DelegatingSpecimenContext();
            var sut = new TypeElementRelay();

            var result = sut.Create(request, context);

            Assert.IsType<NoSpecimen>(result);
        }

        [Fact]
        public void CreateWithTypeElementResolvesSpecimen()
        {
            var expected = new object();
            var request = new TypeElement(typeof(object));
            var context = new DelegatingSpecimenContext { OnResolve = r => expected };
            var sut = new TypeElementRelay();

            var result = sut.Create(request, context);

            Assert.Same(expected, result);
        }
    }
}
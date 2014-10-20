using System;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class MissingParametersSupplyingStaticMethodFactoryTests
    {
        [Fact]
        public void SutIsIMethodFactory()
        {
            var sut = new MissingParametersSupplyingStaticMethodFactory();
            Assert.IsAssignableFrom<IMethodFactory>(sut);
        }

        [Fact]
        public void CreateReturnsCorrectResult()
        {
            Action dummy = delegate { };
            var sut = new MissingParametersSupplyingStaticMethodFactory();

            var result = sut.Create(dummy.Method);

            var expected = new MissingParametersSupplyingMethod(new StaticMethod(dummy.Method));
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CreateWithNullThrows()
        {
            var sut = new MissingParametersSupplyingStaticMethodFactory();
            Assert.Throws<ArgumentNullException>(() => sut.Create(null));
        }
    }
}

using System;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class LateBoundStaticMethodFactoryTests
    {
        [Fact]
        public void SutIsIMethodFactory()
        {
            var sut = new LateBoundStaticMethodFactory();
            Assert.IsAssignableFrom<IMethodFactory>(sut);
        }

        [Fact]
        public void CreateReturnsCorrectResult()
        {
            Action dummy = delegate { };
            var sut = new LateBoundStaticMethodFactory();

            var result = sut.Create(dummy.Method);

            var expected = new LateBoundMethod(new StaticMethod(dummy.Method));
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CreateWithNullThrows()
        {
            var sut = new LateBoundStaticMethodFactory();
            Assert.Throws<ArgumentNullException>(() => sut.Create(null));
        }
    }
}

using System;
using System.Reflection;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
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

            var result = sut.Create(dummy.GetMethodInfo());

            var expected = new MissingParametersSupplyingMethod(new StaticMethod(dummy.GetMethodInfo()));
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

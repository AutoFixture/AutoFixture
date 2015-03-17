using System;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class MissingParametersSupplyingMethodFactoryTests
    {
        [Fact]
        public void SutIsIMethodFactory()
        {
            var owner = new object();
            var sut = new MissingParametersSupplyingMethodFactory(owner);
            Assert.IsAssignableFrom<IMethodFactory>(sut);
        }

        [Fact]
        public void OwnerIsCorrect()
        {
            var owner = new object();
            var sut = new MissingParametersSupplyingMethodFactory(owner);
            Assert.Equal(owner, sut.Owner);
        }

        [Fact]
        public void CreateReturnsCorrectResult()
        {
            Action dummy = delegate { };
            var owner = new object();
            var sut = new MissingParametersSupplyingMethodFactory(owner);

            var result = sut.Create(dummy.Method);

            var expected = new MissingParametersSupplyingMethod(new InstanceMethod(dummy.Method, owner));
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CreateWithNullThrows()
        {
            var owner = new object();
            var sut = new MissingParametersSupplyingMethodFactory(owner);
            Assert.Throws<ArgumentNullException>(() => sut.Create(null));
        }

        [Fact]
        public void InitializeWithNullThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new MissingParametersSupplyingMethodFactory(null));
        }
    }
}
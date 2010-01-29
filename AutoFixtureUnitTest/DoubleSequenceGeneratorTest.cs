using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class DoubleSequenceGeneratorTest
    {
        public DoubleSequenceGeneratorTest()
        {
        }

        [TestMethod]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<DoubleSequenceGenerator, double>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [TestMethod]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<DoubleSequenceGenerator, double>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [TestMethod]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<DoubleSequenceGenerator, double>(sut => sut.CreateAnonymous()).Execute(10);
        }
    }
}

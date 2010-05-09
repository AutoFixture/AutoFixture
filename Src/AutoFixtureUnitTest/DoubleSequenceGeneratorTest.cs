using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class DoubleSequenceGeneratorTest
    {
        public DoubleSequenceGeneratorTest()
        {
        }

        [Fact]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<DoubleSequenceGenerator, double>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [Fact]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<DoubleSequenceGenerator, double>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [Fact]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<DoubleSequenceGenerator, double>(sut => sut.CreateAnonymous()).Execute(10);
        }
    }
}

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class SingleSequenceGeneratorTest
    {
        public SingleSequenceGeneratorTest()
        {
        }

        [Fact]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<SingleSequenceGenerator, float>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [Fact]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<SingleSequenceGenerator, float>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [Fact]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<SingleSequenceGenerator, float>(sut => sut.CreateAnonymous()).Execute(10);
        }
    }
}

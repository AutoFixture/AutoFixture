using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class UInt16SequenceGeneratorTest
    {
        public UInt16SequenceGeneratorTest()
        {
        }

        [Fact]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<UInt16SequenceGenerator, ushort>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [Fact]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<UInt16SequenceGenerator, ushort>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [Fact]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<UInt16SequenceGenerator, ushort>(sut => sut.CreateAnonymous()).Execute(10);
        }
    }
}

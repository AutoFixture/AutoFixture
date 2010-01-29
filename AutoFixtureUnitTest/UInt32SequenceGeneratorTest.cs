using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class UInt32SequenceGeneratorTest
    {
        public UInt32SequenceGeneratorTest()
        {
        }

        [TestMethod]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<UInt32SequenceGenerator, uint>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [TestMethod]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<UInt32SequenceGenerator, uint>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [TestMethod]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<UInt32SequenceGenerator, uint>(sut => sut.CreateAnonymous()).Execute(10);
        }
    }
}

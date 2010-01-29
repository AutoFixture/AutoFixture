using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class BooleanSwitchTest
    {
        public BooleanSwitchTest()
        {
        }

        [TestMethod]
        public void CreateAnonymousWillReturnTrueOnFirstCall()
        {
            // Fixture setup
            BooleanSwitch sut = new BooleanSwitch();
            // Exercise system
            bool result = sut.CreateAnonymous();
            // Verify outcome
            Assert.IsTrue(result, "CreateAnonymous called an uneven number of times");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWillReturnFalseOnSecondCall()
        {
            // Fixture setup
            BooleanSwitch sut = new BooleanSwitch();
            sut.CreateAnonymous();
            // Exercise system
            bool result = sut.CreateAnonymous();
            // Verify outcome
            Assert.IsFalse(result, "CreateAnonymous called an even number of times");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWillReturnTrueOnThirdCall()
        {
            // Fixture setup
            BooleanSwitch sut = new BooleanSwitch();
            sut.CreateAnonymous();
            sut.CreateAnonymous();
            // Exercise system
            bool result = sut.CreateAnonymous();
            // Verify outcome
            Assert.IsTrue(result, "CreateAnonymous called an uneven number of times");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWillReturnFalseOnFourthCall()
        {
            // Fixture setup
            BooleanSwitch sut = new BooleanSwitch();
            sut.CreateAnonymous();
            sut.CreateAnonymous();
            sut.CreateAnonymous();
            // Exercise system
            bool result = sut.CreateAnonymous();
            // Verify outcome
            Assert.IsFalse(result, "CreateAnonymous called an even number of times");
            // Teardown
        }
    }
}

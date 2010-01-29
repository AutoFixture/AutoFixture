using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class MemberInfoNameComparerTest
    {
        public MemberInfoNameComparerTest()
        {
        }

        [TestMethod]
        public void ComparingIdenticalMembersWillReturnTrue()
        {
            // Fixture setup
            MemberInfo mi1 = typeof(DateTime).GetProperty("Ticks");
            MemberInfo mi2 = typeof(DateTime).GetProperty("Ticks");
            MemberInfoNameComparer sut = new MemberInfoNameComparer();
            // Exercise system
            bool result = sut.Equals(mi1, mi2);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void ComparingDifferentMembersWillReturnFalse()
        {
            // Fixture setup
            MemberInfo mi1 = typeof(DateTime).GetProperty("Ticks");
            MemberInfo mi2 = typeof(DateTime).GetProperty("Hour");
            MemberInfoNameComparer sut = new MemberInfoNameComparer();
            // Exercise system
            bool result = sut.Equals(mi1, mi2);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void ComparingNullToMemberWillReturnFalse()
        {
            // Fixture setup
            MemberInfo mi = typeof(DateTime).GetProperty("Ticks");
            MemberInfo nullMemberInfo = null;
            MemberInfoNameComparer sut = new MemberInfoNameComparer();
            // Exercise system
            bool result = sut.Equals(mi, nullMemberInfo);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void ComparingMemberToNullWillReturnFalse()
        {
            // Fixture setup
            MemberInfo nullMemberInfo = null;
            MemberInfo mi = typeof(DateTime).GetProperty("Ticks");
            MemberInfoNameComparer sut = new MemberInfoNameComparer();
            // Exercise system
            bool result = sut.Equals(nullMemberInfo, mi);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void ComparingNullToNullWillReturnTrue()
        {
            // Fixture setup
            var sut = new MemberInfoNameComparer();
            // Exercise system
            bool result = sut.Equals(null, null);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }
    }
}

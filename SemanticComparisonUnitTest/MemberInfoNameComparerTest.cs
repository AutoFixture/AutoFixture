using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Ploeh.SemanticComparison.UnitTest
{
    public class MemberInfoNameComparerTest
    {
        public MemberInfoNameComparerTest()
        {
        }

        [Fact]
        public void ComparingIdenticalMembersWillReturnTrue()
        {
            // Fixture setup
            MemberInfo mi1 = typeof(DateTime).GetProperty("Ticks");
            MemberInfo mi2 = typeof(DateTime).GetProperty("Ticks");
            MemberInfoNameComparer sut = new MemberInfoNameComparer();
            // Exercise system
            bool result = sut.Equals(mi1, mi2);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void ComparingDifferentMembersWillReturnFalse()
        {
            // Fixture setup
            MemberInfo mi1 = typeof(DateTime).GetProperty("Ticks");
            MemberInfo mi2 = typeof(DateTime).GetProperty("Hour");
            MemberInfoNameComparer sut = new MemberInfoNameComparer();
            // Exercise system
            bool result = sut.Equals(mi1, mi2);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void ComparingNullToMemberWillReturnFalse()
        {
            // Fixture setup
            MemberInfo mi = typeof(DateTime).GetProperty("Ticks");
            MemberInfo nullMemberInfo = null;
            MemberInfoNameComparer sut = new MemberInfoNameComparer();
            // Exercise system
            bool result = sut.Equals(mi, nullMemberInfo);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void ComparingMemberToNullWillReturnFalse()
        {
            // Fixture setup
            MemberInfo nullMemberInfo = null;
            MemberInfo mi = typeof(DateTime).GetProperty("Ticks");
            MemberInfoNameComparer sut = new MemberInfoNameComparer();
            // Exercise system
            bool result = sut.Equals(nullMemberInfo, mi);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void ComparingNullToNullWillReturnTrue()
        {
            // Fixture setup
            var sut = new MemberInfoNameComparer();
            // Exercise system
            bool result = sut.Equals(null, null);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }
    }
}

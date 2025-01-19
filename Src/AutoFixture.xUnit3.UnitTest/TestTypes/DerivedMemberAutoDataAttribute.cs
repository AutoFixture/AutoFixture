using System;

namespace AutoFixture.Xunit3.UnitTest.TestTypes
{
    public class DerivedMemberAutoDataAttribute : MemberAutoDataAttribute
    {
        public DerivedMemberAutoDataAttribute(Func<IFixture> fixtureFactory, string memberName, params object[] parameters)
            : base(fixtureFactory, memberName, parameters)
        {
        }

        public DerivedMemberAutoDataAttribute(Func<IFixture> fixtureFactory, Type memberType, string memberName, params object[] parameters)
            : base(fixtureFactory, memberType, memberName, parameters)
        {
        }
    }
}

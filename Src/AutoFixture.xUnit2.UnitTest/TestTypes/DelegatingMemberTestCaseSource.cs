using System;
using AutoFixture.Xunit2.Internal;

namespace AutoFixture.Xunit2.UnitTest.Internal;

public class DelegatingMemberTestCaseSource : MemberTestCaseSource
{
    public DelegatingMemberTestCaseSource(Type type, string name, params object[] arguments)
        : base(type, name, arguments)
    {
    }

    public TestCaseSource GetSource() => this.Source;
}
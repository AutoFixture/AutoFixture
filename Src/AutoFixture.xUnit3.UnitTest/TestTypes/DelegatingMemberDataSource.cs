using System;
using AutoFixture.Xunit3.Internal;

namespace AutoFixture.Xunit3.UnitTest.TestTypes;

public class DelegatingMemberDataSource : MemberDataSource
{
    public DelegatingMemberDataSource(Type type, string name, params object[] arguments)
        : base(type, name, arguments)
    {
    }

    public DataSource GetSource() => this.Source;
}
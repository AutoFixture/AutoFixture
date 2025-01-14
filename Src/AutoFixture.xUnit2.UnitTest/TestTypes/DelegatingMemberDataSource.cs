using System;
using AutoFixture.Xunit2.Internal;

namespace AutoFixture.Xunit2.UnitTest.TestTypes;

public class DelegatingMemberDataSource : MemberDataSource
{
    public DelegatingMemberDataSource(Type type, string name, params object[] arguments)
        : base(type, name, arguments)
    {
    }

    public DataSource GetSource() => this.Source;
}
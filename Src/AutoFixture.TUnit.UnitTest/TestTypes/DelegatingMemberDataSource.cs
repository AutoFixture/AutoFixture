using System;

namespace AutoFixture.TUnit.UnitTest.TestTypes;

public class DelegatingMemberDataSource : MemberDataSource
{
    public DelegatingMemberDataSource(Type type, string name, params object[] arguments)
        : base(type, name, arguments)
    {
    }

    public DataSource GetSource() => this.Source;
}
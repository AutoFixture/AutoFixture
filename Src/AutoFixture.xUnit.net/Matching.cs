using System;

namespace Ploeh.AutoFixture.Xunit
{
    [Flags]
    public enum Matching
    {
        ExactType = 1,
        BaseType = 2,
        ImplementedInterfaces = 4,
        ParameterName = 8,
        PropertyName = 16,
        FieldName = 32,
        MemberName = ParameterName | PropertyName | FieldName
    }
}

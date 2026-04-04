using System;

namespace AutoFixture.Xunit3.Internal;

internal class Argument
{
    public Argument(TestParameter parameter, object value)
    {
        Parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
        Value = value;
    }

    public TestParameter Parameter { get; }

    public object Value { get; }

    public ICustomization GetCustomization() => Parameter.GetCustomization(Value);
}
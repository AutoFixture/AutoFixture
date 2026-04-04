using System.Collections.Generic;
using FakeItEasy.Core;

namespace AutoFixture.AutoFakeItEasy;

internal class MethodCallResult
{
    private List<PositionedValue> _outAndRefValues;
    private readonly object _returnValue;

    public MethodCallResult(object returnValue)
    {
        _returnValue = returnValue;
    }

    public void ApplyToCall(IInterceptedFakeObjectCall fakeObjectCall)
    {
        fakeObjectCall.SetReturnValue(_returnValue);
        if (_outAndRefValues is null) return;

        foreach (var positionedValue in _outAndRefValues)
        {
            fakeObjectCall.SetArgumentValue(positionedValue.Position, positionedValue.Value);
        }
    }

    public void AddOutOrRefValue(int position, object value)
    {
        _outAndRefValues ??= new List<PositionedValue>();
        _outAndRefValues.Add(new PositionedValue(position, value));
    }

    private class PositionedValue
    {
        public PositionedValue(int position, object value)
        {
            Position = position;
            Value = value;
        }

        public int Position { get; }

        public object Value { get; }
    }
}
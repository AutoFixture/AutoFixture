using System.Collections.Generic;
using FakeItEasy.Core;

namespace AutoFixture.AutoFakeItEasy
{
    internal class MethodCallResult
    {
        private List<PositionedValue> outAndRefValues;
        private readonly object returnValue;

        public MethodCallResult(object returnValue)
        {
            this.returnValue = returnValue;
        }

        public void ApplyToCall(IInterceptedFakeObjectCall fakeObjectCall)
        {
            fakeObjectCall.SetReturnValue(this.returnValue);
            if (this.outAndRefValues is null) return;

            foreach (var positionedValue in this.outAndRefValues)
            {
                fakeObjectCall.SetArgumentValue(positionedValue.Position, positionedValue.Value);
            }
        }

        public void AddOutOrRefValue(int position, object value)
        {
            this.outAndRefValues ??= new List<PositionedValue>();
            this.outAndRefValues.Add(new PositionedValue(position, value));
        }

        private class PositionedValue
        {
            public PositionedValue(int position, object value)
            {
                this.Position = position;
                this.Value = value;
            }

            public int Position { get; }

            public object Value { get; }
        }
    }
}
using System.Collections.Generic;

namespace AutoFixture.AutoFakeItEasy
{
    internal class MethodCallResult
    {
        private IList<PositionedValue> outAndRefValues;
        private readonly object returnValue;

        public MethodCallResult(object returnValue)
        {
            this.returnValue = returnValue;
        }

        public void ApplyToCall(FakeObjectCall fakeObjectCall)
        {
            fakeObjectCall.SetReturnValue(this.returnValue);
            if (outAndRefValues == null) return;

            foreach (var positionedValue in this.outAndRefValues)
            {
                fakeObjectCall.SetArgumentValue(positionedValue.Position, positionedValue.Value);
            }
        }

        public void AddOutOrRefValue(int i, object value)
        {
            if (this.outAndRefValues == null)
            {
                this.outAndRefValues = new List<PositionedValue>();
            }

            this.outAndRefValues.Add(new PositionedValue(i, value));
        }

        private class PositionedValue
        {
            public int Position { get; }
            public object Value { get; }

            public PositionedValue(int position, object value)
            {
                this.Position = position;
                this.Value = value;
            }
        }
    }
}
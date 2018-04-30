namespace AutoFixture.AutoFakeItEasy
{
    internal class MethodCallResult
    {
        private readonly object returnValue;

        public MethodCallResult(object returnValue)
        {
            this.returnValue = returnValue;
        }

        public void ApplyToCall(FakeObjectCall fakeObjectCall)
        {
            fakeObjectCall.SetReturnValue(this.returnValue);
        }
    }
}
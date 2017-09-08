using System;

namespace Ploeh.AutoFixtureDocumentationTest.Simple
{
    public class SomeImp : IBadDesign
    {
        private MyClass mc;
        private string message;

        public string Message
        {
            get { return this.message; }
            set
            {
                if (this.mc == null)
                {
                    throw new InvalidOperationException("...");
                }

                this.message = value;
                this.TransformedMessage = this.mc.DoStuff(value);
            }
        }

        public void Initialize(MyClass mc)
        {
            this.mc = mc;
        }

        public string TransformedMessage { get; private set; }
    }
}

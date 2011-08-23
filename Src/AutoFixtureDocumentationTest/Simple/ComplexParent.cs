namespace Ploeh.AutoFixtureDocumentationTest.Simple
{
    public class ComplexParent
    {
        private readonly ComplexChild child;

        public ComplexParent(ComplexChild child)
        {
            this.child = child;
        }

        public ComplexChild Child
        {
            get { return this.child; }
        }
    }
}

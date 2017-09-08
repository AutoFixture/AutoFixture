namespace Ploeh.AutoFixtureDocumentationTest.Simple
{
    public class ComplexParent
    {
        public ComplexParent(ComplexChild child)
        {
            this.Child = child;
        }

        public ComplexChild Child { get; }
    }
}

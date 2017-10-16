namespace AutoFixtureDocumentationTest.Simple
{
    public interface IBadDesign
    {
        string Message { get; set; }

        void Initialize(MyClass mc);
    }
}

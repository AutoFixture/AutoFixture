namespace AutoFixtureDocumentationTest.Simple;

public class ComplexParent
{
    public ComplexParent(ComplexChild child)
    {
        Child = child;
    }

    public ComplexChild Child { get; }
}
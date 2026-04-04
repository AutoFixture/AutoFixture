namespace AutoFixtureDocumentationTest.Simple;

public class ComplexChild
{
    public ComplexChild(string name)
    {
        Name = name;
    }

    public ComplexChild(string name, int number)
    {
        Name = name;
        Number = number;
    }

    public string Name { get; }

    public int Number { get; set; }
}
using System.Linq;

namespace AutoFixtureDocumentationTest.Intermediate;

public class MyClass
{
    private readonly IMyInterface _d;

    public MyClass(IMyInterface mi)
    {
        _d = mi;
    }

    public int CalculateSumOfThings()
    {
        return _d.ThingNumbers.Sum();
    }
}
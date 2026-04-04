using System;

namespace AutoFixtureDocumentationTest.Intermediate;

public class Person
{
    private Person _spouse;

    public DateTime BirthDay { get; set; }

    public string Name { get; set; }

    public Person Spouse
    {
        get => _spouse;
        set
        {
            _spouse = value;
            if (value != null)
            {
                value._spouse = this;
            }
        }
    }
}
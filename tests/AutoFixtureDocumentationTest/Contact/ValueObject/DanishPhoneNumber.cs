namespace AutoFixtureDocumentationTest.Contact.ValueObject;

public class DanishPhoneNumber
{
    public DanishPhoneNumber(int number)
    {
        RawNumber = number;
    }

    public int RawNumber { get; }
}
namespace AutoFixtureDocumentationTest.Contact.Parsing;

public class Contact
{
    public Contact(string name, string phoneNumber)
    {
        Name = name;
        PhoneNumber =
            Contact.ParsePhoneNumber(phoneNumber);
    }

    public string Name { get; set; }

    public int PhoneNumber { get; set; }

    private static int ParsePhoneNumber(string phoneNumber)
    {
        return int.Parse(phoneNumber);
    }
}
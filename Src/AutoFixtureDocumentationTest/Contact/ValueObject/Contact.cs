namespace AutoFixtureDocumentationTest.Contact.ValueObject;

public class Contact
{
    public Contact(string name, DanishPhoneNumber phoneNumber)
    {
        Name = name;
        PhoneNumber = phoneNumber;
    }

    public string Name { get; set; }

    public DanishPhoneNumber PhoneNumber { get; set; }
}
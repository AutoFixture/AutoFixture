namespace AutoFixtureDocumentationTest.Contact.ValidatingValueObject;

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
namespace Ploeh.AutoFixtureDocumentationTest.Contact.ValueObject
{
    public class Contact
    {
        public Contact(string name, DanishPhoneNumber phoneNumber)
        {
            this.Name = name;
            this.PhoneNumber = phoneNumber;
        }

        public string Name { get; set; }

        public DanishPhoneNumber PhoneNumber { get; set; }
    }
}

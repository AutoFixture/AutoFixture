namespace Ploeh.AutoFixtureDocumentationTest.Contact.ValueObject
{
    public class DanishPhoneNumber
    {
        public DanishPhoneNumber(int number)
        {
            this.RawNumber = number;
        }

        public int RawNumber { get; }
    }
}

using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureDocumentationTest.Contact.ValidatingValueObject
{
    public class ContactTest
    {
        public ContactTest()
        {
        }

        [Fact]
        public void CreateWillNotThrow()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            fixture.Register<int>(() => 12345678);
            // Exercise system
            fixture.Create<Contact>();
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [Fact]
        public void CreateWillPopulatePhoneNumber()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            fixture.Register<int, DanishPhoneNumber>(i => 
                new DanishPhoneNumber(i + 112));
            Contact sut = fixture.Create<Contact>();
            // Exercise system
            int result = sut.PhoneNumber.RawNumber;
            // Verify outcome
            Assert.NotEqual<int>(default(int), result);
            // Teardown
        }

        [Fact]
        public void UsingPhoneNumberMinValue()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            fixture.Register<int, DanishPhoneNumber>(i =>
                new DanishPhoneNumber(i + 
                    DanishPhoneNumber.MinValue));
            Contact sut = fixture.Create<Contact>();
            // Exercise system
            int result = sut.PhoneNumber.RawNumber;
            // Verify outcome
            Assert.NotEqual<int>(default(int), result);
            // Teardown
        }
    }
}

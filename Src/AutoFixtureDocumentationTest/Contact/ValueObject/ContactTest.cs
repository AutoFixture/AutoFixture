using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureDocumentationTest.Contact.ValueObject
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
            Contact sut = fixture.Create<Contact>();
            // Exercise system
            int result = sut.PhoneNumber.RawNumber;
            // Verify outcome
            Assert.NotEqual<int>(default(int), result);
            // Teardown
        }
    }
}

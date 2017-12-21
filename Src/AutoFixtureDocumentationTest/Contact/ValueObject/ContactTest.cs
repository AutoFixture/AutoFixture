using AutoFixture;
using Xunit;

namespace AutoFixtureDocumentationTest.Contact.ValueObject
{
    public class ContactTest
    {
        public ContactTest()
        {
        }

        [Fact]
        public void CreateWillNotThrow()
        {
            // Arrange
            Fixture fixture = new Fixture();
            // Act
            fixture.Create<Contact>();
            // Assert (no exception indicates success)
        }

        [Fact]
        public void CreateWillPopulatePhoneNumber()
        {
            // Arrange
            Fixture fixture = new Fixture();
            Contact sut = fixture.Create<Contact>();
            // Act
            int result = sut.PhoneNumber.RawNumber;
            // Assert
            Assert.NotEqual<int>(default(int), result);
        }
    }
}

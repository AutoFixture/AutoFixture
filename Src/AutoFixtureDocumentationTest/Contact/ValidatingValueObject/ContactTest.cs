using AutoFixture;
using Xunit;

namespace AutoFixtureDocumentationTest.Contact.ValidatingValueObject
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
            fixture.Register<int>(() => 12345678);
            // Act
            fixture.Create<Contact>();
            // Assert (no exception indicates success)
        }

        [Fact]
        public void CreateWillPopulatePhoneNumber()
        {
            // Arrange
            Fixture fixture = new Fixture();
            fixture.Register<int, DanishPhoneNumber>(i => 
                new DanishPhoneNumber(i + 112));
            Contact sut = fixture.Create<Contact>();
            // Act
            int result = sut.PhoneNumber.RawNumber;
            // Assert
            Assert.NotEqual<int>(default(int), result);
        }

        [Fact]
        public void UsingPhoneNumberMinValue()
        {
            // Arrange
            Fixture fixture = new Fixture();
            fixture.Register<int, DanishPhoneNumber>(i =>
                new DanishPhoneNumber(i + 
                    DanishPhoneNumber.MinValue));
            Contact sut = fixture.Create<Contact>();
            // Act
            int result = sut.PhoneNumber.RawNumber;
            // Assert
            Assert.NotEqual<int>(default(int), result);
        }
    }
}

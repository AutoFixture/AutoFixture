using AutoFixture;
using Xunit;

namespace AutoFixtureDocumentationTest.Contact.Parsing
{
    public class ContactTest
    {
        public ContactTest()
        {
        }

        [Fact]
        public void CreateWithDefaultStringWillThrow()
        {
            // Arrange
            Fixture fixture = new Fixture();
            // Act & Assert
            Assert.ThrowsAny<ObjectCreationException>(() =>
                fixture.Create<Contact>());
        }

        [Fact]
        public void CreateWithExplicitNumberStringWillSucceed()
        {
            // Arrange
            Fixture fixture = new Fixture();
            fixture.Register<string>(() => "112");
            Contact sut = fixture.Create<Contact>();
            // Act
            int result = sut.PhoneNumber;
            // Assert
            Assert.NotEqual<int>(default(int), result);
        }

        [Fact]
        public void CreateWithAnonymousNumberStringWillSucceed()
        {
            // Arrange
            Fixture fixture = new Fixture();
            fixture.Register<int, string>(i => i.ToString());
            Contact sut = fixture.Create<Contact>();
            // Act
            int result = sut.PhoneNumber;
            // Assert
            Assert.NotEqual<int>(default(int), result);
        }
    }
}

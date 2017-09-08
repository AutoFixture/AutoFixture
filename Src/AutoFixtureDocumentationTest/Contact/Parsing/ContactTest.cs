using System.Reflection;
using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureDocumentationTest.Contact.Parsing
{
    public class ContactTest
    {
        public ContactTest()
        {
        }

        [Fact]
        public void CreateWithDefaultStringWillThrow()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ObjectCreationException>(() =>
                fixture.Create<Contact>());
            // Teardown
        }

        [Fact]
        public void CreateWithExplicitNumberStringWillSucceed()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            fixture.Register<string>(() => "112");
            Contact sut = fixture.Create<Contact>();
            // Exercise system
            int result = sut.PhoneNumber;
            // Verify outcome
            Assert.NotEqual<int>(default(int), result);
            // Teardown
        }

        [Fact]
        public void CreateWithAnonymousNumberStringWillSucceed()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            fixture.Register<int, string>(i => i.ToString());
            Contact sut = fixture.Create<Contact>();
            // Exercise system
            int result = sut.PhoneNumber;
            // Verify outcome
            Assert.NotEqual<int>(default(int), result);
            // Teardown
        }
    }
}

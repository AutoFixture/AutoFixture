using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureDocumentationTest.Intermediate
{
    public class RegisterExample
    {
        public RegisterExample()
        {
        }

        [Fact]
        public void CreatingMyClassIsNotPossibleWithoutRegister()
        {
            try
            {
                Fixture fixture = new Fixture();
                MyClass sut = fixture.Create<MyClass>();
            }
            catch (ObjectCreationException)
            {
            }
        }

        [Fact]
        public void SimpleRegister()
        {
            Fixture fixture = new Fixture();
            fixture.Register<IMyInterface>(() => 
                new FakeMyInterface());
            MyClass sut = fixture.Create<MyClass>();

            Assert.NotNull(sut);
        }

        [Fact]
        public void ManuallyRegisteringWithAnonymousParameter()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            int anonymousNumber = fixture.Create<int>();
            string knownText = "This text is not anonymous";
            fixture.Register<IMyInterface>(() => 
                new FakeMyInterface(anonymousNumber, knownText));
            // Exercise system
            MyClass sut = fixture.Create<MyClass>();
            // Verify outcome
            Assert.NotNull(sut);
            // Teardown
        }

        [Fact]
        public void RegisterWithAnonymousParameter()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            string knownText = "This text is not anonymous";
            fixture.Register<int, string, IMyInterface>((i, s) => 
                new FakeMyInterface(i, knownText));
            // Exercise system
            MyClass sut = fixture.Create<MyClass>();
            // Verify outcome
            Assert.NotNull(sut);
            // Teardown
        }
    }
}

using AutoFixture;
using Xunit;

namespace AutoFixtureDocumentationTest.Intermediate
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
            // Arrange
            Fixture fixture = new Fixture();
            int anonymousNumber = fixture.Create<int>();
            string knownText = "This text is not anonymous";
            fixture.Register<IMyInterface>(() => 
                new FakeMyInterface(anonymousNumber, knownText));
            // Act
            MyClass sut = fixture.Create<MyClass>();
            // Assert
            Assert.NotNull(sut);
        }

        [Fact]
        public void RegisterWithAnonymousParameter()
        {
            // Arrange
            Fixture fixture = new Fixture();
            string knownText = "This text is not anonymous";
            fixture.Register<int, string, IMyInterface>((i, s) => 
                new FakeMyInterface(i, knownText));
            // Act
            MyClass sut = fixture.Create<MyClass>();
            // Assert
            Assert.NotNull(sut);
        }
    }
}

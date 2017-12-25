using AutoFixture;
using Xunit;

namespace AutoFixtureDocumentationTest.Intermediate
{
    public class PersonTest
    {
        public PersonTest()
        {
        }

        [Fact]
        public void CreateAnonymousWillThrow()
        {
            var fixture = new Fixture();
            // var person = fixture.Create<Person>();

            /* The above call to CreateAnonymous will throw a StackOverflowException, which cannot
             * be caught in .NET 2+.
             * To stop the whole unit test suite from crashing, the line has been commented out. To
             * reproduce this behavior, uncomment the line that creates a new anonymous Person. */
        }

        [Fact]
        public void BuildWithoutSpouseWillSucceed()
        {
            var fixture = new Fixture();
            var person = fixture.Build<Person>()
                .Without(p => p.Spouse)
                .Create();

            Assert.NotNull(person);
        }

        [Fact]
        public void SettingSpouseIsPossible()
        {
            // Arrange
            var fixture = new Fixture();
            var person = fixture.Build<Person>().Without(p => p.Spouse).Create();
            var sut = fixture.Build<Person>().Without(p => p.Spouse).Create();
            // Act
            sut.Spouse = person;
            // Assert
            Assert.Equal<Person>(sut, person.Spouse);
        }
    }
}

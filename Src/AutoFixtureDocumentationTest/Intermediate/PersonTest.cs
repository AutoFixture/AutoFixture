using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureDocumentationTest.Intermediate
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
            // Fixture setup
            var fixture = new Fixture();
            var person = fixture.Build<Person>().Without(p => p.Spouse).Create();
            var sut = fixture.Build<Person>().Without(p => p.Spouse).Create();
            // Exercise system
            sut.Spouse = person;
            // Verify outcome
            Assert.Equal<Person>(sut, person.Spouse);
            // Teardown
        }
    }
}

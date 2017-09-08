using System.Linq;
using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureDocumentationTest.Intermediate
{
    public class MyClassTest
    {
        public MyClassTest()
        {
        }

        [Fact]
        public void NumberSumIsCorrect_AutoFixture()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            IMyInterface fake = new FakeMyInterface();
            fixture.Register<IMyInterface>(() => fake);

            var things = fixture.CreateMany<Thing>().ToList();
            things.ForEach(t => fake.AddThing(t));
            int expectedSum = things.Select(t => t.Number).Sum();

            MyClass sut = fixture.Create<MyClass>();
            // Exercise system
            int result = sut.CalculateSumOfThings();
            // Verify outcome
            Assert.Equal<int>(expectedSum, result);
            // Teardown
        }

        [Fact]
        public void NumberSumIsCorrect_DerivedFixture()
        {
            // Fixture setup
            MyClassFixture fixture = new MyClassFixture();
            fixture.AddManyTo(fixture.Things);

            int expectedSum = 
                fixture.Things.Select(t => t.Number).Sum();
            MyClass sut = fixture.Create<MyClass>();
            // Exercise system
            int result = sut.CalculateSumOfThings();
            // Verify outcome
            Assert.Equal<int>(expectedSum, result);
            // Teardown
        }
    }
}

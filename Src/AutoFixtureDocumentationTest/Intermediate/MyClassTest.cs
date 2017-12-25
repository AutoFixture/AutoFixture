using System.Linq;
using AutoFixture;
using Xunit;

namespace AutoFixtureDocumentationTest.Intermediate
{
    public class MyClassTest
    {
        public MyClassTest()
        {
        }

        [Fact]
        public void NumberSumIsCorrect_AutoFixture()
        {
            // Arrange
            Fixture fixture = new Fixture();
            IMyInterface fake = new FakeMyInterface();
            fixture.Register<IMyInterface>(() => fake);

            var things = fixture.CreateMany<Thing>().ToList();
            things.ForEach(t => fake.AddThing(t));
            int expectedSum = things.Select(t => t.Number).Sum();

            MyClass sut = fixture.Create<MyClass>();
            // Act
            int result = sut.CalculateSumOfThings();
            // Assert
            Assert.Equal<int>(expectedSum, result);
        }

        [Fact]
        public void NumberSumIsCorrect_DerivedFixture()
        {
            // Arrange
            MyClassFixture fixture = new MyClassFixture();
            fixture.AddManyTo(fixture.Things);

            int expectedSum = 
                fixture.Things.Select(t => t.Number).Sum();
            MyClass sut = fixture.Create<MyClass>();
            // Act
            int result = sut.CalculateSumOfThings();
            // Assert
            Assert.Equal<int>(expectedSum, result);
        }
    }
}

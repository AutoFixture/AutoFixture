using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureDocumentationTest.Intermediate
{
    [TestClass]
    public class MyClassTest
    {
        public MyClassTest()
        {
        }

        [TestMethod]
        public void NumberSumIsCorrect_AutoFixture()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            IMyInterface fake = new FakeMyInterface();
            fixture.Register<IMyInterface>(() => fake);

            var things = fixture.CreateMany<Thing>().ToList();
            things.ForEach(t => fake.AddThing(t));
            int expectedSum = things.Select(t => t.Number).Sum();

            MyClass sut = fixture.CreateAnonymous<MyClass>();
            // Exercise system
            int result = sut.CalculateSumOfThings();
            // Verify outcome
            Assert.AreEqual<int>(expectedSum, result,
                "Sum of things");
            // Teardown
        }

        [TestMethod]
        public void NumberSumIsCorrect_DerivedFixture()
        {
            // Fixture setup
            MyClassFixture fixture = new MyClassFixture();
            fixture.AddManyTo(fixture.Things);

            int expectedSum = 
                fixture.Things.Select(t => t.Number).Sum();
            MyClass sut = fixture.CreateAnonymous<MyClass>();
            // Exercise system
            int result = sut.CalculateSumOfThings();
            // Verify outcome
            Assert.AreEqual<int>(expectedSum, result,
                "Sum of things");
            // Teardown
        }
    }
}

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using System.Reflection;

namespace Ploeh.AutoFixtureDocumentationTest.Simple
{
    /// <summary>
    /// This class contains code snippets written for documentation
    /// purposes. They are implemented as tests to ensure that they
    /// compile and work as intended.
    /// </summary>
    [TestClass]
    public class DocumentationExample
    {
        public DocumentationExample()
        {
        }

        [TestMethod]
        public void IntroductoryTest()
        {
            // Fixture setup
            Fixture fixture = new Fixture();

            int expectedNumber = fixture.CreateAnonymous<int>();
            MyClass sut = fixture.CreateAnonymous<MyClass>();
            // Exercise system
            int result = sut.Echo(expectedNumber);
            // Verify outcome
            Assert.AreEqual<int>(expectedNumber, result, "Echo");
            // Teardown
        }

        [TestMethod]
        public void ComplexCreation()
        {
            // Fixture setup
            Fixture fixture = new Fixture();

            var anonymousParent =
                fixture.CreateAnonymous<ComplexParent>();
            // Exercise system
            string result = anonymousParent.Child.Name;
            // Verify outcome
            Assert.IsNotNull(result, "CreateAnonymous");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousString()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            var anonymousText = fixture.CreateAnonymous<string>();
            // Exercise system
            Console.WriteLine(anonymousText);
            // Verify outcome
            Assert.IsNotNull(anonymousText);
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousSeededString()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            var anonymousName = fixture.CreateAnonymous("Name");
            // Exercise system
            Console.WriteLine(anonymousName);
            // Verify outcome
            Assert.IsNotNull(anonymousName);
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousInt32()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            // Exercise system
            int anonymousNumber = fixture.CreateAnonymous<int>();
            // Verify outcome
            Assert.AreNotEqual<int>(default(int), anonymousNumber, "CreateAnonymous");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousSeededInt32()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            // Exercise system
            int anonymousNumber = fixture.CreateAnonymous(42);
            // Verify outcome
            Assert.AreNotEqual<int>(default(int), anonymousNumber, "CreateAnonymous");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousDecimal()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            // Exercise system
            decimal anonymousNumber =
                fixture.CreateAnonymous<decimal>();
            // Verify outcome
            Assert.AreNotEqual<decimal>(default(decimal), anonymousNumber, "CreateAnonymous");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousBooleans()
        {
            // Fixture setup
            var expectedResult = new[] { true, false, true };
            Fixture fixture = new Fixture();
            List<bool> result = new List<bool>();
            // Exercise system
            result.Add(fixture.CreateAnonymous<bool>());
            result.Add(fixture.CreateAnonymous(true));
            result.Add(fixture.CreateAnonymous(true));
            // Verify outcome
            CollectionAssert.AreEqual(expectedResult, result, "CreateAnonymous");
            // Teardown
        }

        [TestMethod]
        public void ReplacingStringAlgorithmUsingRegister()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            fixture.Register<string>(() => "ploeh");
            // Exercise system
            string result = fixture.CreateAnonymous<string>();
            // Verify outcome
            Assert.AreEqual<string>("ploeh", result, "CreateAnonymous");
            // Teardown
        }

        [TestMethod]
        public void ReplacingStringAlgorithmUsingTypeMappings()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            fixture.TypeMappings[typeof(string)] = s => "fnaah";
            // Exercise system
            string result = fixture.CreateAnonymous<string>();
            // Verify outcome
            Assert.AreEqual<string>("fnaah", result, "CreateAnonymous");
            // Teardown
        }

        [TestMethod]
        public void ReplacingStringAlgorithmWithSeed()
        {
            Fixture fixture = new Fixture();
            fixture.TypeMappings[typeof(string)] = s =>
                string.Format((string)s, new Random().Next(100));

            string result = fixture.CreateAnonymous("Risk: {0}%");

            Console.WriteLine(result);
        }

        [TestMethod]
        public void CreateManyStrings()
        {
            Fixture fixture = new Fixture();
            var strings = fixture.CreateMany<string>();

            Assert.IsTrue(strings.Count() > 1, "Multiple strings");
        }

        [TestMethod]
        public void CreateManyMyClassInstances()
        {
            Fixture fixture = new Fixture();
            var myInstances = fixture.CreateMany<MyClass>();

            Assert.IsTrue(myInstances.Count() > 1, "Multiple instances");
        }

        [TestMethod]
        public void AddManyInstancesToList()
        {
            Fixture fixture = new Fixture();
            var list = new List<MyClass>();
            fixture.AddManyTo(list);

            Assert.IsTrue(list.Count > 1, "Multiple instances");
        }

        [TestMethod]
        public void AddManyInstancesUsingCustomCreator()
        {
            Fixture fixture = new Fixture();
            var list = new List<int>();
            var r = new Random();
            fixture.AddManyTo(list, () => r.Next());

            Assert.IsTrue(list.Count > 1, "Multiple instances");
        }

        [TestMethod]
        public void CreateAnExplicitNumberOfInstances()
        {
            Fixture fixture = new Fixture();
            fixture.RepeatCount = 10;
            var sequence = fixture.CreateMany<MyClass>();

            Assert.AreEqual<int>(fixture.RepeatCount, sequence.Count(), "Exact number of instances");
        }

        [TestMethod]
        public void AddAnExplicitNumberOfInstancesToList()
        {
            Fixture fixture = new Fixture();
            var list = new List<MyClass>();
            fixture.RepeatCount = 7;
            fixture.AddManyTo(list);

            Assert.AreEqual<int>(fixture.RepeatCount, list.Count, "Exact number of instances");
        }

        [TestMethod]
        public void BuildAndImmediatelyCreateAnonymous()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var mc = fixture.Build<MyClass>().CreateAnonymous();
            // Verify outcome
            Assert.IsNotNull(mc, "Build followed by CreateAnonymous");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousMyClass()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var mc = fixture.CreateAnonymous<MyClass>();
            // Verify outcome
            Assert.IsNotNull(mc, "CreataAnonymous");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousMyClassAndAssignProperty()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var mc = fixture.CreateAnonymous<MyClass>();
            mc.MyText = "Ploeh";
            // Verify outcome
            string result = mc.MyText;
            Assert.AreEqual<string>("Ploeh", result, "MyText");
            // Teardown
        }

        [TestMethod]
        public void BuildAnonymousMyClassAndAssignProperty()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var mc = fixture.Build<MyClass>()
                .With(x => x.MyText, "Ploeh")
                .CreateAnonymous();
            // Verify outcome
            string result = mc.MyText;
            Assert.AreEqual<string>("Ploeh", result, "MyText");
            // Teardown
        }

        [ExpectedException(typeof(TargetInvocationException))]
        [TestMethod]
        public void CreateAnonymousFilterWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var f = fixture.CreateAnonymous<Filter>();
            // Verify outcome (expected exception)
            // Teardown
        }

        [TestMethod]
        public void BuildAnonymousFilterWillSucceed()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            int min = fixture.CreateAnonymous<int>();
            int max = min + 1;
            var f = fixture.Build<Filter>()
                .With(s => s.Max, max)
                .With(s => s.Min, min)
                .CreateAnonymous();
            // Verify outcome
            Assert.AreEqual<int>(min, f.Min, "Min");
            Assert.AreEqual<int>(max, f.Max, "Max");
            // Teardown
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void InvalidUseOfSomeImp()
        {
            // Fixture setup
            var something = new SomeImp();
            // Exercise system
            something.Message = "Ploeh";
            // Verify outcome (expected exception)
            // Teardown
        }

        [TestMethod]
        public void ValidUseOfSomeImp()
        {
            // Fixture setup
            var something = new SomeImp();
            // Exercise system
            something.Initialize(new MyClass());
            something.Message = "Ploeh";
            // Verify outcome
            Assert.AreEqual<string>("Ploeh", something.Message, "Message");
            // Teardown
        }

        [ExpectedException(typeof(TargetInvocationException))]
        [TestMethod]
        public void CreateAnonymousSomeImpWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var imp = fixture.CreateAnonymous<SomeImp>();
            // Verify outcome (expected exception)
            // Teardown
        }

        [TestMethod]
        public void BuildAnonymousSomeImpWillSucceed()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var imp = fixture.Build<SomeImp>()
                .Do(s => s.Initialize(new MyClass()))
                .CreateAnonymous();
            // Verify outcome
            Assert.IsNotNull(imp.TransformedMessage, "Do");
            // Teardown
        }
    }
}

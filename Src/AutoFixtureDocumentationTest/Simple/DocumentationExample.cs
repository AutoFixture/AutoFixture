using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureDocumentationTest.Simple
{
    /// <summary>
    /// This class contains code snippets written for documentation
    /// purposes. They are implemented as tests to ensure that they
    /// compile and work as intended.
    /// </summary>
    public class DocumentationExample
    {
        [Fact]
        public void IntroductoryTest()
        {
            // Fixture setup
            Fixture fixture = new Fixture();

            int expectedNumber = fixture.Create<int>();
            MyClass sut = fixture.Create<MyClass>();
            // Exercise system
            int result = sut.Echo(expectedNumber);
            // Verify outcome
            Assert.Equal<int>(expectedNumber, result);
            // Teardown
        }

        [Fact]
        public void ComplexCreation()
        {
            // Fixture setup
            Fixture fixture = new Fixture();

            var anonymousParent =
                fixture.Create<ComplexParent>();
            // Exercise system
            string result = anonymousParent.Child.Name;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousString()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            var anonymousText = fixture.Create<string>();
            // Exercise system
            TestConsole.WriteLine(anonymousText);
            // Verify outcome
            Assert.NotNull(anonymousText);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousSeededString()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            var anonymousName = fixture.Create("Name");
            // Exercise system
            TestConsole.WriteLine(anonymousName);
            // Verify outcome
            Assert.NotNull(anonymousName);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousInt32()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            // Exercise system
            int anonymousNumber = fixture.Create<int>();
            // Verify outcome
            Assert.NotEqual<int>(default(int), anonymousNumber);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousSeededInt32()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            // Exercise system
            int anonymousNumber = fixture.Create(42);
            // Verify outcome
            Assert.NotEqual<int>(default(int), anonymousNumber);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousDecimal()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            // Exercise system
            decimal anonymousNumber =
                fixture.Create<decimal>();
            // Verify outcome
            Assert.NotEqual<decimal>(default(decimal), anonymousNumber);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousBooleans()
        {
            // Fixture setup
            var expectedResult = new[] { true, false, true };
            Fixture fixture = new Fixture();
            List<bool> result = new List<bool>();
            // Exercise system
            result.Add(fixture.Create<bool>());
            result.Add(fixture.Create(true));
            result.Add(fixture.Create(true));
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void ReplacingStringAlgorithmUsingRegister()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            fixture.Register<string>(() => "ploeh");
            // Exercise system
            string result = fixture.Create<string>();
            // Verify outcome
            Assert.Equal("ploeh", result);
            // Teardown
        }

        [Fact]
        public void ReplacingStringAlgorithmUsingFromSeed()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            fixture.Customize<string>(c => c.FromSeed(s => "fnaah"));
            // Exercise system
            string result = fixture.Create<string>();
            // Verify outcome
            Assert.Equal("fnaah", result);
            // Teardown
        }

        [Fact]
        public void ReplacingStringAlgorithmWithSeed()
        {
            Fixture fixture = new Fixture();
            fixture.Customize<string>(c =>
                c.FromSeed(s =>
                    string.Format(s, new Random().Next(100))));

            string result = fixture.Create("Risk: {0}%");

            TestConsole.WriteLine(result);
        }

        // Addresses this particular question: http://autofixture.codeplex.com/discussions/248586
        [Fact]
        public void ReplacingStringAlgorithmWithSeededFooStrings()
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(new NumericSequenceGenerator());
            fixture.Customize<string>(c =>
                c.FromFactory((int i) => "foo" + i));

            var s1 = fixture.Create<string>();
            var s2 = fixture.Create<string>();

            Assert.Equal("foo1", s1);
            Assert.Equal("foo2", s2);
        }

        [Fact]
        public void ReplacingInt32AlgorithmWithSeededAlgorithm()
        {
            var fixture = new Fixture();
            fixture.Customize<int>(c => c.FromSeed(i => i));

            var result = fixture.Create<int>(42);

            Assert.Equal(42, result);
        }

        [Fact]
        public void ReplaceDateTimeAlgorithmToReturnRisingUniqueDateTimeInstances()
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(new NumericSequenceGenerator());
            fixture.Register((long i) => new DateTime(i));

            var dateTimes = fixture.CreateMany<DateTime>(20);
            Assert.True(dateTimes.Aggregate(new { DateTime = DateTime.MinValue, Flag = true }, (x, dt) => new { DateTime = dt, Flag = x.Flag && x.DateTime < dt }, x => x.Flag));
        }

        [Fact]
        public void CustomizeNumbersToUseASingleRisingSequence()
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(new NumericSequenceGenerator());
            fixture.Register((int i) => (long)i);
            fixture.Register((int i) => (decimal)i);
            fixture.Register((int i) => (float)i);

            Assert.Equal(1, fixture.Create<int>());
            Assert.Equal(2, fixture.Create<long>());
            Assert.Equal(3, fixture.Create<int>());
            Assert.Equal(4, fixture.Create<float>());
            Assert.Equal(5, fixture.Create<decimal>());
            Assert.Equal(6, fixture.Create<decimal>());
            Assert.Equal(7, fixture.Create<int>());
        }

        [Fact]
        public void CreateManyStrings()
        {
            Fixture fixture = new Fixture();
            var strings = fixture.CreateMany<string>();

            Assert.True(strings.Count() > 1, "Multiple strings");
        }

        [Fact]
        public void CreateManyMyClassInstances()
        {
            Fixture fixture = new Fixture();
            var myInstances = fixture.CreateMany<MyClass>();

            Assert.True(myInstances.Count() > 1, "Multiple instances");
        }

        [Fact]
        public void AddManyInstancesToList()
        {
            Fixture fixture = new Fixture();
            var list = new List<MyClass>();
            fixture.AddManyTo(list);

            Assert.True(list.Count > 1, "Multiple instances");
        }

        [Fact]
        public void AddManyInstancesUsingCustomCreator()
        {
            Fixture fixture = new Fixture();
            var list = new List<int>();
            var r = new Random();
            fixture.AddManyTo(list, () => r.Next());

            Assert.True(list.Count > 1, "Multiple instances");
        }

        [Fact]
        public void CreateAnExplicitNumberOfInstances()
        {
            Fixture fixture = new Fixture();
            fixture.RepeatCount = 10;
            var sequence = fixture.CreateMany<MyClass>();

            Assert.Equal<int>(fixture.RepeatCount, sequence.Count());
        }

        [Fact]
        public void AddAnExplicitNumberOfInstancesToList()
        {
            Fixture fixture = new Fixture();
            var list = new List<MyClass>();
            fixture.RepeatCount = 7;
            fixture.AddManyTo(list);

            Assert.Equal<int>(fixture.RepeatCount, list.Count);
        }

        [Fact]
        public void BuildAndImmediatelyCreateAnonymous()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var mc = fixture.Build<MyClass>().Create();
            // Verify outcome
            Assert.NotNull(mc);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousMyClass()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var mc = fixture.Create<MyClass>();
            // Verify outcome
            Assert.NotNull(mc);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousMyClassAndAssignProperty()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var mc = fixture.Create<MyClass>();
            mc.MyText = "Ploeh";
            // Verify outcome
            string result = mc.MyText;
            Assert.Equal("Ploeh", result);
            // Teardown
        }

        [Fact]
        public void BuildAnonymousMyClassAndAssignProperty()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var mc = fixture.Build<MyClass>()
                .With(x => x.MyText, "Ploeh")
                .Create();
            // Verify outcome
            string result = mc.MyText;
            Assert.Equal("Ploeh", result);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousFilterWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Customizations.Add(new NumericSequenceGenerator());
            // Exercise system and verify outcome
            Assert.Throws<ObjectCreationException>(()=>
                fixture.Create<Filter>());
            // Teardown
        }

        [Fact]
        public void BuildAnonymousFilterWillSucceed()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            int min = fixture.Create<int>();
            int max = min + 1;
            var f = fixture.Build<Filter>()
                .With(s => s.Max, max)
                .With(s => s.Min, min)
                .Create();
            // Verify outcome
            Assert.Equal<int>(min, f.Min);
            Assert.Equal<int>(max, f.Max);
            // Teardown
        }

        [Fact]
        public void InvalidUseOfSomeImp()
        {
            // Fixture setup
            var something = new SomeImp();
            // Exercise system and verify outcome
            Assert.Throws<InvalidOperationException>(() =>
                something.Message = "Ploeh");
            // Teardown
        }

        [Fact]
        public void ValidUseOfSomeImp()
        {
            // Fixture setup
            var something = new SomeImp();
            // Exercise system
            something.Initialize(new MyClass());
            something.Message = "Ploeh";
            // Verify outcome
            Assert.Equal("Ploeh", something.Message);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousSomeImpWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ObjectCreationException>(() =>
                fixture.Create<SomeImp>());
            // Teardown
        }

        [Fact]
        public void BuildAnonymousSomeImpWillSucceed()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var imp = fixture.Build<SomeImp>()
                .Do(s => s.Initialize(new MyClass()))
                .Create();
            // Verify outcome
            Assert.NotNull(imp.TransformedMessage);
            // Teardown
        }

        [Fact]
        public void DemonstrateHowInjectWorks()
        {
            var fixture = new Fixture();
            var original = new MyClass();
            fixture.Inject(original);

            var actual1 = fixture.Create<MyClass>();
            var actual2 = fixture.Create<MyClass>();

            // actual1 and actual2 are equal, and equal to original
            Assert.Same(actual1, actual2);
            Assert.Same(original, actual1);
            Assert.Same(original, actual2);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Xunit;

namespace AutoFixtureDocumentationTest.Simple
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
            // Arrange
            Fixture fixture = new Fixture();

            int expectedNumber = fixture.Create<int>();
            MyClass sut = fixture.Create<MyClass>();
            // Act
            int result = sut.Echo(expectedNumber);
            // Assert
            Assert.Equal<int>(expectedNumber, result);
        }

        [Fact]
        public void ComplexCreation()
        {
            // Arrange
            Fixture fixture = new Fixture();

            var anonymousParent =
                fixture.Create<ComplexParent>();
            // Act
            string result = anonymousParent.Child.Name;
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateAnonymousString()
        {
            // Arrange
            Fixture fixture = new Fixture();
            var anonymousText = fixture.Create<string>();
            // Act
            TestConsole.WriteLine(anonymousText);
            // Assert
            Assert.NotNull(anonymousText);
        }

        [Fact]
        public void CreateAnonymousSeededString()
        {
            // Arrange
            Fixture fixture = new Fixture();
            var anonymousName = fixture.Create("Name");
            // Act
            TestConsole.WriteLine(anonymousName);
            // Assert
            Assert.NotNull(anonymousName);
        }

        [Fact]
        public void CreateAnonymousInt32()
        {
            // Arrange
            Fixture fixture = new Fixture();
            // Act
            int anonymousNumber = fixture.Create<int>();
            // Assert
            Assert.NotEqual<int>(default(int), anonymousNumber);
        }

        [Fact]
        public void CreateAnonymousSeededInt32()
        {
            // Arrange
            Fixture fixture = new Fixture();
            // Act
            int anonymousNumber = fixture.Create(42);
            // Assert
            Assert.NotEqual<int>(default(int), anonymousNumber);
        }

        [Fact]
        public void CreateAnonymousDecimal()
        {
            // Arrange
            Fixture fixture = new Fixture();
            // Act
            decimal anonymousNumber =
                fixture.Create<decimal>();
            // Assert
            Assert.NotEqual<decimal>(default(decimal), anonymousNumber);
        }

        [Fact]
        public void CreateAnonymousBooleans()
        {
            // Arrange
            var expectedResult = new[] { true, false, true };
            Fixture fixture = new Fixture();
            List<bool> result = new List<bool>();
            // Act
            result.Add(fixture.Create<bool>());
            result.Add(fixture.Create(true));
            result.Add(fixture.Create(true));
            // Assert
            Assert.True(expectedResult.SequenceEqual(result));
        }

        [Fact]
        public void ReplacingStringAlgorithmUsingRegister()
        {
            // Arrange
            Fixture fixture = new Fixture();
            fixture.Register<string>(() => "ploeh");
            // Act
            string result = fixture.Create<string>();
            // Assert
            Assert.Equal("ploeh", result);
        }

        [Fact]
        public void ReplacingStringAlgorithmUsingFromSeed()
        {
            // Arrange
            Fixture fixture = new Fixture();
            fixture.Customize<string>(c => c.FromSeed(s => "fnaah"));
            // Act
            string result = fixture.Create<string>();
            // Assert
            Assert.Equal("fnaah", result);
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
            // Arrange
            var fixture = new Fixture();
            // Act
            var mc = fixture.Build<MyClass>().Create();
            // Assert
            Assert.NotNull(mc);
        }

        [Fact]
        public void CreateAnonymousMyClass()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var mc = fixture.Create<MyClass>();
            // Assert
            Assert.NotNull(mc);
        }

        [Fact]
        public void CreateAnonymousMyClassAndAssignProperty()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var mc = fixture.Create<MyClass>();
            mc.MyText = "Ploeh";
            // Assert
            string result = mc.MyText;
            Assert.Equal("Ploeh", result);
        }

        [Fact]
        public void BuildAnonymousMyClassAndAssignProperty()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var mc = fixture.Build<MyClass>()
                .With(x => x.MyText, "Ploeh")
                .Create();
            // Assert
            string result = mc.MyText;
            Assert.Equal("Ploeh", result);
        }

        [Fact]
        public void CreateAnonymousFilterWillThrow()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customizations.Add(new NumericSequenceGenerator());
            // Act & Assert
            Assert.ThrowsAny<ObjectCreationException>(()=>
                fixture.Create<Filter>());
        }

        [Fact]
        public void BuildAnonymousFilterWillSucceed()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            int min = fixture.Create<int>();
            int max = min + 1;
            var f = fixture.Build<Filter>()
                .With(s => s.Max, max)
                .With(s => s.Min, min)
                .Create();
            // Assert
            Assert.Equal<int>(min, f.Min);
            Assert.Equal<int>(max, f.Max);
        }

        [Fact]
        public void InvalidUseOfSomeImp()
        {
            // Arrange
            var something = new SomeImp();
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                something.Message = "Ploeh");
        }

        [Fact]
        public void ValidUseOfSomeImp()
        {
            // Arrange
            var something = new SomeImp();
            // Act
            something.Initialize(new MyClass());
            something.Message = "Ploeh";
            // Assert
            Assert.Equal("Ploeh", something.Message);
        }

        [Fact]
        public void CreateAnonymousSomeImpWillThrow()
        {
            // Arrange
            var fixture = new Fixture();
            // Act & Assert
            Assert.ThrowsAny<ObjectCreationException>(() =>
                fixture.Create<SomeImp>());
        }

        [Fact]
        public void BuildAnonymousSomeImpWillSucceed()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var imp = fixture.Build<SomeImp>()
                .Do(s => s.Initialize(new MyClass()))
                .Create();
            // Assert
            Assert.NotNull(imp.TransformedMessage);
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

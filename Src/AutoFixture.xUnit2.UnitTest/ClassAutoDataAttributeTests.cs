using AutoFixture.Kernel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using TestTypeFoundation;
using Xunit;
using Xunit.Sdk;

namespace AutoFixture.Xunit2.UnitTest
{
    public class ClassAutoDataAttributeTests
    {
        [Fact]
        public void CanCreateInstance()
        {
            _ = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
        }

        [Fact]
        public void IsDataAttribute()
        {
            ClassAutoDataAttribute sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));

            Assert.IsAssignableFrom<DataAttribute>(sut);
        }

        [Fact]
        public void GetDataDoesNotThrow()
        {
            ClassAutoDataAttribute sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
            MethodInfo testMethod = this.GetType().GetMethod(nameof(ExampleTestClass.TestMethod));

            _ = sut.GetData(testMethod);
        }

        [Fact]
        public void GetDataReturnsEnumerable()
        {
            ClassAutoDataAttribute sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
            MethodInfo testMethod = this.GetType().GetMethod(nameof(ExampleTestClass.TestMethod));

            IEnumerable<object[]> actual = sut.GetData(testMethod);

            Assert.NotNull(actual);
        }

        [Fact]
        public void GetDataReturnsNonEmptyEnumerable()
        {
            ClassAutoDataAttribute sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
            MethodInfo testMethod = this.GetType().GetMethod(nameof(ExampleTestClass.TestMethod));

            IEnumerable<object[]> actual = sut.GetData(testMethod);

            Assert.NotEmpty(actual);
        }

        [Fact]
        public void GetDataReturnsExpectedTestCaseCount()
        {
            ClassAutoDataAttribute sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
            MethodInfo testMethod = this.GetType().GetMethod(nameof(ExampleTestClass.TestMethod));

            IEnumerable<object[]> actual = sut.GetData(testMethod);

            Assert.Equal(5, actual.Count());
        }

        [Fact]
        public void GetDataThrowsWhenDataSourceNotEnumerable()
        {
            ClassAutoDataAttribute sut = new ClassAutoDataAttribute(typeof(GuardedConstructorHost<object>));
            MethodInfo testMethod = this.GetType().GetMethod(nameof(ExampleTestClass.TestMethod));

            Action act = () => _ = sut.GetData(testMethod).ToList();

            Assert.Throws<MissingMethodException>(act);
        }

        [Fact]
        public void GetDataThrowsForNonMatchingConstructorTypes()
        {
            ClassAutoDataAttribute sut = new ClassAutoDataAttribute(
                typeof(DelegatingTestData),
                new object[] { "myString", 33, null });
            MethodInfo testMethod = this.GetType().GetMethod(nameof(ExampleTestClass.TestMethod));

            Action act = () => _ = sut.GetData(testMethod).ToList();

            Assert.Throws<MissingMethodException>(act);
        }

        [Fact]
        public void GetDataReturnsExpectedTestCases()
        {
            CompositeSpecimenBuilder builder = new CompositeSpecimenBuilder(
                new FixedParameterBuilder<int>("a", 1),
                new FixedParameterBuilder<string>("b", "value"),
                new FixedParameterBuilder<EnumType>("c", EnumType.First),
                new FixedParameterBuilder<Tuple<string, int>>("d", new Tuple<string, int>("value", 1)));
            DerivedClassAutoDataAttribute sut = new DerivedClassAutoDataAttribute(
                () => new DelegatingFixture { OnCreate = (r, c) => builder.Create(r, c) },
                typeof(MixedTypeClassData));
            MethodInfo testMethod = this.GetType().GetMethod(nameof(ExampleTestClass.TestMethod));
            object[][] expected =
            {
                new object[] { 1, "value", EnumType.First, new Tuple<string, int>("value", 1) },
                new object[] { 9, "value", EnumType.First, new Tuple<string, int>("value", 1) },
                new object[] { 12, "test-12", EnumType.First, new Tuple<string, int>("value", 1) },
                new object[] { 223, "test-17", EnumType.Third, new Tuple<string, int>("value", 1) },
                new object[] { -95, "test-92", EnumType.Second, new Tuple<string, int>("myValue", 5) }
            };

            object[][] actual = sut.GetData(testMethod).ToArray();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDataReturnsExpectedTestCasesFromParameterizedSource()
        {
            CompositeSpecimenBuilder builder = new CompositeSpecimenBuilder(
                new FixedParameterBuilder<int>("a", 1),
                new FixedParameterBuilder<string>("b", "value"),
                new FixedParameterBuilder<Tuple<string, int>>("d", new Tuple<string, int>("value", 1)));
            DerivedClassAutoDataAttribute sut = new DerivedClassAutoDataAttribute(
                () => new DelegatingFixture { OnCreate = (r, c) => builder.Create(r, c) },
                typeof(ParameterizedClassData),
                29, "myValue", EnumType.Third);
            MethodInfo testMethod = this.GetType().GetMethod(nameof(ExampleTestClass.TestMethod));
            object[][] expected =
            {
                new object[] { 29, "myValue", EnumType.Third, new Tuple<string, int>("value", 1) },
                new object[] { 29, "myValue", EnumType.Third, new Tuple<string, int>("value", 1) },
            };

            object[][] actual = sut.GetData(testMethod).ToArray();

            Assert.Equal(expected, actual);
        }
    }

    public class ExampleTestClass
    {
        [SuppressMessage("Usage", "xUnit1013:Public method should be marked as test",
            Justification = "This test method is used through reflection.")]
        public void TestMethod(int a, string b, EnumType c, Tuple<string, int> d)
        {
        }
    }

    public class MixedTypeClassData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[0];
            yield return new object[] { 9 };
            yield return new object[] { 12, "test-12" };
            yield return new object[] { 223, "test-17", EnumType.Third };
            yield return new object[] { -95, "test-92", EnumType.Second, new Tuple<string, int>("myValue", 5) };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public class ParameterizedClassData : IEnumerable<object[]>
    {
        private readonly int p1;
        private readonly string p2;
        private readonly EnumType p3;

        public ParameterizedClassData(int p1, string p2, EnumType p3)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { this.p1, this.p2, this.p3 };
            yield return new object[] { this.p1, this.p2, this.p3 };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

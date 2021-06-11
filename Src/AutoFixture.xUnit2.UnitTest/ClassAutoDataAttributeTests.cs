using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;
using Xunit.Sdk;

#if NETCOREAPP1_1
using System.Reflection;
#endif

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
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));

            Assert.IsAssignableFrom<DataAttribute>(sut);
        }

        [Fact]
        public void GetDataDoesNotThrow()
        {
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
            var testMethod = this.GetType().GetMethod(nameof(this.TestMethod));

            _ = sut.GetData(testMethod);
        }

        [Fact]
        public void GetDataReturnsEnumerable()
        {
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
            var testMethod = this.GetType().GetMethod(nameof(this.TestMethod));

            var actual = sut.GetData(testMethod);

            Assert.NotNull(actual);
        }

        [Fact]
        public void GetDataReturnsNonEmptyEnumerable()
        {
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
            var testMethod = this.GetType().GetMethod(nameof(this.TestMethod));

            var actual = sut.GetData(testMethod);

            Assert.NotEmpty(actual);
        }

        [Fact]
        public void GetDataReturnsExpectedTestCaseCount()
        {
            var sut = new ClassAutoDataAttribute(typeof(MixedTypeClassData));
            var testMethod = this.GetType().GetMethod(nameof(this.TestMethod));

            var actual = sut.GetData(testMethod);

            Assert.Equal(5, actual.Count());
        }

        [Fact]
        public void GetDataThrowsWhenDataSourceNotEnumerable()
        {
            var sut = new ClassAutoDataAttribute(typeof(GuardedConstructorHost<object>));
            var testMethod = this.GetType().GetMethod(nameof(this.TestMethod));

            Action act = () => _ = sut.GetData(testMethod).ToList();

            Assert.Throws<MissingMethodException>(act);
        }

        [Fact]
        public void GetDataThrowsForNonMatchingConstructorTypes()
        {
            var sut = new ClassAutoDataAttribute(typeof(ParameterizedClassData), "myString", 33, null);
            var testMethod = this.GetType().GetMethod(nameof(this.TestMethod));

            Action act = () => _ = sut.GetData(testMethod).ToList();

            Assert.Throws<MissingMethodException>(act);
        }

        [Fact]
        public void GetDataReturnsExpectedTestCases()
        {
            var builder = new CompositeSpecimenBuilder(
                new FixedParameterBuilder<int>("a", 1),
                new FixedParameterBuilder<string>("b", "value"),
                new FixedParameterBuilder<EnumType>("c", EnumType.First),
                new FixedParameterBuilder<Tuple<string, int>>("d", new Tuple<string, int>("value", 1)));
            var sut = new DerivedClassAutoDataAttribute(
                () => new DelegatingFixture { OnCreate = (r, c) => builder.Create(r, c) },
                typeof(MixedTypeClassData));
            var testMethod = this.GetType().GetMethod(nameof(this.TestMethod));
            object[][] expected =
            {
                new object[] { 1, "value", EnumType.First, new Tuple<string, int>("value", 1) },
                new object[] { 9, "value", EnumType.First, new Tuple<string, int>("value", 1) },
                new object[] { 12, "test-12", EnumType.First, new Tuple<string, int>("value", 1) },
                new object[] { 223, "test-17", EnumType.Third, new Tuple<string, int>("value", 1) },
                new object[] { -95, "test-92", EnumType.Second, new Tuple<string, int>("myValue", 5) }
            };

            var actual = sut.GetData(testMethod).ToArray();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDataReturnsExpectedTestCasesFromParameterizedSource()
        {
            var builder = new CompositeSpecimenBuilder(
                new FixedParameterBuilder<int>("a", 1),
                new FixedParameterBuilder<string>("b", "value"),
                new FixedParameterBuilder<Tuple<string, int>>("d", new Tuple<string, int>("value", 1)));
            var sut = new DerivedClassAutoDataAttribute(
                () => new DelegatingFixture { OnCreate = (r, c) => builder.Create(r, c) },
                typeof(ParameterizedClassData),
                29, "myValue", EnumType.Third);
            var testMethod = this.GetType().GetMethod(nameof(this.TestMethod));
            object[][] expected =
            {
                new object[] { 29, "myValue", EnumType.Third, new Tuple<string, int>("value", 1) },
                new object[] { 29, "myValue", EnumType.Third, new Tuple<string, int>("value", 1) },
            };

            var actual = sut.GetData(testMethod).ToArray();

            Assert.Equal(expected, actual);
        }

        [SuppressMessage("Usage", "xUnit1013:Public method should be marked as test",
            Justification = "This test method is used through reflection.")]
        public void TestMethod(int a, string b, EnumType c, Tuple<string, int> d)
        {
        }

        private class MixedTypeClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[0];
                yield return new object[] { 9 };
                yield return new object[] { 12, "test-12" };
                yield return new object[] { 223, "test-17", EnumType.Third };
                yield return new object[] { -95, "test-92", EnumType.Second, new Tuple<string, int>("myValue", 5) };
            }

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }

        private class ParameterizedClassData : IEnumerable<object[]>
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

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }
    }
}
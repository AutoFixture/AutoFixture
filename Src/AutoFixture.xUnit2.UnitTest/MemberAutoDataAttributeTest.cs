using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TestTypeFoundation;
using Xunit;
using Xunit.Sdk;

namespace AutoFixture.Xunit2.UnitTest
{
    public class MemberAutoDataAttributeTest
    {
        [Fact]
        public void SutIsDataAttribute()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();

            // Act
            var sut = new MemberAutoDataAttribute(memberName);

            // Assert
            Assert.IsAssignableFrom<DataAttribute>(sut);
        }

        [Fact]
        public void GetDataWithNullMethodThrows()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();
            var sut = new MemberAutoDataAttribute(memberName);

            // Act & assert
            Assert.Throws<ArgumentNullException>(() => sut.GetData(null));
        }

        [Fact]
        public void GetDataShouldReturnSameNumberOfRowsAsMemberDataSource()
        {
            // Arrange
            var method = ((Action<string, string>)this.FooBarTestMethod).GetMethodInfo();

            var sut = new MemberAutoDataAttribute(nameof(FooBarTestCase));

            // Act
            var data = sut.GetData(method);

            // Assert
            Assert.Equal(FooBarTestCase.Count(), data.Count());
        }

        [Fact]
        public void MemberNameIsCorrectWhenConstructedWithDefaultConstructor()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();
            var sut = new MemberAutoDataAttribute(memberName);

            // Act
            var result = sut.MemberName;

            // Assert
            Assert.Equal(memberName, result);
        }

        [Fact]
        public void MemberNameIsCorrectWhenConstructedWithDataAttribute()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();
            Func<DataAttribute> dataAttribute = () => new AutoDataAttribute();
            var sut = new DerivedMemberAutoDataAttribute(dataAttribute, memberName);

            // Act
            var result = sut.MemberName;

            // Assert
            Assert.Equal(memberName, result);
        }

        [Fact]
        public void ParametersAreCorrectWhenConstructedWithDefaultCtor()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();
            var parameters = new object[] { 42, "42" };
            var sut = new MemberAutoDataAttribute(memberName, parameters);

            // Act
            var result = sut.Parameters;

            // Assert
            Assert.Equal(parameters, result);
        }

        [Fact]
        public void ParametersAreCorrectWhenConstructedWithDataAttribute()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();
            var parameters = new object[] { 42, "42" };
            Func<DataAttribute> dataAttribute = () => new AutoDataAttribute();
            var sut = new DerivedMemberAutoDataAttribute(dataAttribute, memberName, parameters);

            // Act
            var result = sut.Parameters;

            // Assert
            Assert.Equal(parameters, result);
        }

        [Fact]
        public void AutoDataAttributeIsCorrectWhenConstructedWithDefaultConstructor()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();
            var sut = new MemberAutoDataAttribute(memberName);

            // Act
            var result = sut.DataAttributeFactory;

            // Assert
            Assert.IsType<Func<DataAttribute>>(result);
        }

        [Fact]
        public void AutoDataAttributeIsCorrectWhenConstructedWithExplicitDataAttribute()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();
            Func<DataAttribute> expected = () => new AutoDataAttribute();
            var sut = new DerivedMemberAutoDataAttribute(expected, memberName);

            // Act
            var result = sut.DataAttributeFactory;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DoesNotActivateFixtureImmediately()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();
            bool wasInvoked = false;
            Func<DataAttribute> autoData = () => new DerivedAutoDataAttribute(() =>
            {
                wasInvoked = true;
                return new Fixture();
            });

            // Act
            var sut = new DerivedMemberAutoDataAttribute(autoData, memberName);

            // Assert
            Assert.False(wasInvoked);
        }

        [Fact]
        public void PreDiscoveryShouldBeDisabled()
        {
            // Arrange
            var expectedDiscovererType = typeof(NoPreDiscoveryDataDiscoverer).GetTypeInfo();
            var discovererAttr = typeof(MemberAutoDataAttribute).GetTypeInfo()
                .CustomAttributes
                .Single(x => x.AttributeType == typeof(DataDiscovererAttribute));

            var expectedType = expectedDiscovererType.FullName;
            var expectedAssembly = expectedDiscovererType.Assembly.GetName().Name;

            // Act
            var actualType = (string)discovererAttr.ConstructorArguments[0].Value;
            var actualAssembly = (string)discovererAttr.ConstructorArguments[1].Value;

            // Assert
            Assert.Equal(expectedType, actualType);
            Assert.Equal(expectedAssembly, actualAssembly);
        }

        [Fact]
        public void ContainsDifferentFrozenInstancesEachRun()
        {
            var memberName = nameof(TestData);
            var methodInfo = this.GetType().GetMethod(
                nameof(this.TestMethodWithComplexType),
                BindingFlags.NonPublic | BindingFlags.Instance);
            var sut = new MemberAutoDataAttribute(memberName);

            var actual = sut.GetData(methodInfo)
                .SelectMany(x => x)
                .OfType<FieldHolder<int>>()
                .Distinct()
                .Count();

            Assert.Equal(3, actual);
        }

        [Fact]
        public void ContainsTestData()
        {
            var memberName = nameof(TestData);
            var methodInfo = this.GetType().GetMethod(
                nameof(this.TestMethodWithComplexType),
                BindingFlags.NonPublic | BindingFlags.Instance);
            var sut = new MemberAutoDataAttribute(memberName);

            var actual = sut.GetData(methodInfo)
                .SelectMany(x => x)
                .OfType<int>();

            Assert.Equal(new[] { 1, 2, 4 }, actual);
        }

        public static TheoryData<int> TestData => new TheoryData<int> { 1, 2, 4 };

        private void TestMethodWithComplexType(int intVal, [Frozen] FieldHolder<int> container)
        {
        }

        public static IEnumerable<object[]> FooBarTestCase => new List<object[]>
        {
            new[] { "foo" },
            new[] { "foo", "bar" },
            new[] { "bar" },
        };

        private void FooBarTestMethod(string s1, string s2)
        {
        }

        private class DerivedMemberAutoDataAttribute : MemberAutoDataAttribute
        {
            public DerivedMemberAutoDataAttribute(
                Func<DataAttribute> autoDataAttribute,
                string memberName,
                params object[] parameters)
                : base(autoDataAttribute, memberName, parameters)
            {
            }
        }

        private class DerivedAutoDataAttribute : AutoDataAttribute
        {
            public DerivedAutoDataAttribute(Func<IFixture> fixtureFactory)
                : base(fixtureFactory)
            {
            }
        }
    }
}

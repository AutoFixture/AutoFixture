using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture.Xunit2.UnitTest.TestTypes;
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
        public void InitializedWithMemberNameAndParameters()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();
            var parameters = new object[] { "value-one", 3, 12.2f };

            // Act
            var sut = new MemberAutoDataAttribute(memberName, parameters);

            // Assert
            Assert.Equal(memberName, sut.MemberName);
            Assert.Equal(parameters, sut.Parameters);
            Assert.Null(sut.MemberType);
            Assert.NotNull(sut.FixtureFactory);
        }

        [Fact]
        public void InitializedWithTypeMemberNameAndParameters()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();
            var parameters = new object[] { "value-one", 3, 12.2f };
            var testType = typeof(MemberAutoDataAttributeTest);

            // Act
            var sut = new MemberAutoDataAttribute(testType, memberName, parameters);

            // Assert
            Assert.Equal(memberName, sut.MemberName);
            Assert.Equal(parameters, sut.Parameters);
            Assert.Equal(testType, sut.MemberType);
            Assert.NotNull(sut.FixtureFactory);
        }

        [Fact]
        public void ThrowsWhenInitializedWithNullMemberName()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => new MemberAutoDataAttribute(null!));
        }

        [Fact]
        public void ThrowsWhenInitializedWithNullParameters()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => new MemberAutoDataAttribute(memberName, default(object[])!));
        }

        [Fact]
        public void DoesNotThrowWhenInitializedWithNullType()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();

            // Act & Assert
            _ = new MemberAutoDataAttribute(default(Type)!, memberName);
        }

        [Fact]
        public void ThrowsWhenTestMethodNull()
        {
            // Arrange
            var sut = new MemberAutoDataAttribute("memberName");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => sut.GetData(null!).ToArray());
        }

        [Fact]
        public void ThrowsWhenMemberNotEnumerable()
        {
            // Arrange
            var memberName = nameof(TestTypeWithMethodData.NonEnumerableMethod);
            var sut = new MemberAutoDataAttribute(memberName);
            var method = TestTypeWithMethodData.GetNonEnumerableMethodInfo();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => sut.GetData(method).ToArray());
            Assert.Contains(memberName, ex.Message);
        }

        [Fact]
        public void ThrowsWhenMemberNotStatic()
        {
            // Arrange
            var memberName = nameof(TestTypeWithMethodData.NonStaticSource);
            var sut = new MemberAutoDataAttribute(memberName);
            var method = TestTypeWithMethodData.GetNonStaticSourceMethodInfo();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => sut.GetData(method).ToArray());
            Assert.Contains(memberName, ex.Message);
        }

        [Fact]
        public void ThrowsWhenMemberDoesNotExist()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();
            var sut = new MemberAutoDataAttribute(typeof(TestTypeWithMethodData), memberName);
            var method = TestTypeWithMethodData.GetMultipleValueTestMethodInfo();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => sut.GetData(method).ToArray());
            Assert.Contains(memberName, ex.Message);
        }

        [Fact]
        public void DoesntActivateFixtureImmediately()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();
            var wasInvoked = false;
            Func<IFixture> autoData = () =>
            {
                wasInvoked = true;
                return new DelegatingFixture();
            };

            // Act
            _ = new DerivedMemberAutoDataAttribute(autoData, memberName);

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

        [Theory]
        [InlineData("CreateWithFrozenAndFavorArrays")]
        [InlineData("CreateWithFavorArraysAndFrozen")]
        [InlineData("CreateWithFrozenAndFavorEnumerables")]
        [InlineData("CreateWithFavorEnumerablesAndFrozen")]
        [InlineData("CreateWithFrozenAndFavorLists")]
        [InlineData("CreateWithFavorListsAndFrozen")]
        [InlineData("CreateWithFrozenAndGreedy")]
        [InlineData("CreateWithGreedyAndFrozen")]
        [InlineData("CreateWithFrozenAndModest")]
        [InlineData("CreateWithModestAndFrozen")]
        [InlineData("CreateWithFrozenAndNoAutoProperties")]
        [InlineData("CreateWithNoAutoPropertiesAndFrozen")]
        public void GetDataOrdersCustomizationAttributes(string methodName)
        {
            // Arrange
            var method = typeof(TypeWithCustomizationAttributes)
                .GetMethod(methodName, new[] { typeof(ConcreteType) });
            var customizationLog = new List<ICustomization>();
            var fixture = new DelegatingFixture();
            fixture.OnCustomize = c =>
            {
                customizationLog.Add(c);
                return fixture;
            };

            var sut = new DerivedMemberAutoDataAttribute(
                () => fixture,
                typeof(TestTypeWithMethodData),
                nameof(TestTypeWithMethodData.TestCasesWithNoValues));

            // Act
            var data = sut.GetData(method).ToArray();

            // Assert
            var composite = Assert.IsAssignableFrom<CompositeCustomization>(customizationLog[0]);
            Assert.IsNotType<FreezeOnMatchCustomization>(composite.Customizations.First());
            Assert.IsType<FreezeOnMatchCustomization>(composite.Customizations.Last());
        }

        [Fact]
        public void GeneratesTestsFromParameterlessMethod()
        {
            const string memberName = nameof(TestTypeWithMethodData.GetSingleStringValueTestCases);
            var sut = new MemberAutoDataAttribute(memberName);
            var testMethod = TestTypeWithMethodData.GetSingleStringValueTestMethodInfo();

            var testCases = sut.GetData(testMethod).ToArray();

            Assert.Collection(testCases,
                testCase => Assert.Equal("value-one", testCase.Single()),
                testCase => Assert.Equal("value-two", testCase.Single()),
                testCase => Assert.Equal("value-three", testCase.Single()));
        }

        [Fact]
        public void GeneratesTestsFromMethodWithParameter()
        {
            const string memberName = nameof(TestTypeWithMethodData.GetStringTestsFromArgument);
            var sut = new MemberAutoDataAttribute(memberName, "testcase");
            var testMethod = TestTypeWithMethodData.GetStringTestsFromArgumentMethodInfo();

            var testCases = sut.GetData(testMethod).ToArray();

            Assert.Collection(testCases,
                testCase => Assert.Equal("testcase-one", testCase.Single()),
                testCase => Assert.Equal("testcase-two", testCase.Single()),
                testCase => Assert.Equal("testcase-three", testCase.Single()));
        }

        [Fact]
        public void GeneratesTestCasesForTestsWithMultipleParameters()
        {
            // Arrange
            const string memberName = nameof(TestTypeWithMethodData.GetMultipleValueTestCases);
            var sut = new MemberAutoDataAttribute(memberName);
            var testMethod = TestTypeWithMethodData.GetMultipleValueTestMethodInfo();

            // Act
            var testCases = sut.GetData(testMethod).ToArray();

            // Assert
            Assert.Collection(testCases,
                testCase =>
                {
                    Assert.Equal(3, testCase.Length);
                    Assert.Equal("value-one", testCase[0]);
                    Assert.Equal(12, testCase[1]);
                    Assert.Equal(23.3m, testCase[2]);
                },
                testCase =>
                {
                    Assert.Equal(3, testCase.Length);
                    Assert.Equal("value-two", testCase[0]);
                    Assert.Equal(38, testCase[1]);
                    Assert.Equal(12.7m, testCase[2]);
                },
                testCase =>
                {
                    Assert.Equal(3, testCase.Length);
                    Assert.Equal("value-three", testCase[0]);
                    Assert.Equal(94, testCase[1]);
                    Assert.Equal(52.21m, testCase[2]);
                });
        }

        [Fact]
        public void GeneratesMissingDataForTestsWithMultipleParameters()
        {
            // Arrange
            const string memberName = nameof(TestTypeWithMethodData.GetSingleStringValueTestCases);
            var sut = new MemberAutoDataAttribute(memberName);
            var testMethod = TestTypeWithMethodData.GetMultipleValueTestMethodInfo();

            // Act
            var testCases = sut.GetData(testMethod).ToArray();

            // Assert
            Assert.Collection(testCases,
                testCase =>
                {
                    Assert.Equal(3, testCase.Length);
                    Assert.Equal("value-one", testCase[0]);
                    Assert.NotEqual(0, testCase[1]);
                    Assert.NotEqual(0, testCase[2]);
                },
                testCase =>
                {
                    Assert.Equal(3, testCase.Length);
                    Assert.Equal("value-two", testCase[0]);
                    Assert.NotEqual(0, testCase[1]);
                    Assert.NotEqual(0, testCase[2]);
                },
                testCase =>
                {
                    Assert.Equal(3, testCase.Length);
                    Assert.Equal("value-three", testCase[0]);
                    Assert.NotEqual(0, testCase[1]);
                    Assert.NotEqual(0, testCase[2]);
                });
        }

        [Fact]
        public void GeneratesTestCasesWithInjectedParameters()
        {
            // Arrange
            const string memberName = nameof(TestTypeWithMethodData.GetTestWithFrozenParameterCases);
            var sut = new MemberAutoDataAttribute(memberName);
            var testMethod = TestTypeWithMethodData.GetTestWithFrozenParameter();

            // Act
            var testCases = sut.GetData(testMethod).ToArray();

            // Assert
            Assert.Collection(testCases,
                testCase =>
                {
                    Assert.Equal(3, testCase.Length);
                    Assert.Equal("value-one", testCase[0]);
                    Assert.Equal("value-two", testCase[1]);
                    Assert.Equal("value-two", testCase[2]);
                },
                testCase =>
                {
                    Assert.Equal(3, testCase.Length);
                    Assert.Equal("value-two", testCase[0]);
                    Assert.Equal("value-three", testCase[1]);
                    Assert.Equal("value-three", testCase[2]);
                },
                testCase =>
                {
                    Assert.Equal(3, testCase.Length);
                    Assert.Equal("value-three", testCase[0]);
                    Assert.Equal("value-one", testCase[1]);
                    Assert.Equal("value-one", testCase[2]);
                });
        }

        [Fact]
        public void AutoGeneratesValuesForFrozenParameters()
        {
            // Arrange
            const string memberName = nameof(TestTypeWithMethodData.GetSingleStringValueTestCases);
            var sut = new MemberAutoDataAttribute(memberName);
            var testMethod = TestTypeWithMethodData.GetTestWithFrozenParameter();

            // Act
            var testCases = sut.GetData(testMethod).ToArray();

            // Assert
            Assert.Collection(testCases,
                testCase =>
                {
                    Assert.Equal(3, testCase.Length);
                    Assert.Equal("value-one", testCase[0]);
                    Assert.NotEmpty(testCase[1].ToString());
                    Assert.Equal(testCase[1], testCase[2]);
                },
                testCase =>
                {
                    Assert.Equal(3, testCase.Length);
                    Assert.Equal("value-two", testCase[0]);
                    Assert.NotEmpty(testCase[1].ToString());
                    Assert.Equal(testCase[1], testCase[2]);
                },
                testCase =>
                {
                    Assert.Equal(3, testCase.Length);
                    Assert.Equal("value-three", testCase[0]);
                    Assert.NotEmpty(testCase[1].ToString());
                    Assert.Equal(testCase[1], testCase[2]);
                });
        }

        [Fact]
        public void SupportsInheritedTestDataMembers()
        {
            // Arrange
            const string memberName = nameof(TestTypeWithMethodData.GetMultipleValueTestCases);
            var sut = new MemberAutoDataAttribute(memberName);
            var testMethod = ChildTestTypeMethodData.GetMultipleValueTestMethodInfo();

            // Act
            var testCases = sut.GetData(testMethod).ToArray();

            // Assert
            Assert.Collection(testCases,
                testCase =>
                {
                    Assert.Equal(3, testCase.Length);
                    Assert.Equal("value-one", testCase[0]);
                    Assert.Equal(12, testCase[1]);
                    Assert.Equal(23.3m, testCase[2]);
                },
                testCase =>
                {
                    Assert.Equal(3, testCase.Length);
                    Assert.Equal("value-two", testCase[0]);
                    Assert.Equal(38, testCase[1]);
                    Assert.Equal(12.7m, testCase[2]);
                },
                testCase =>
                {
                    Assert.Equal(3, testCase.Length);
                    Assert.Equal("value-three", testCase[0]);
                    Assert.Equal(94, testCase[1]);
                    Assert.Equal(52.21m, testCase[2]);
                });
        }
    }
}
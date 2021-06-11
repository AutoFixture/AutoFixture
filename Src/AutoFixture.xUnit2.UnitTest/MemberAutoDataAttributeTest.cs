using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture.Xunit2.UnitTest.TestTypes;
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
            Assert.Throws<ArgumentNullException>(
                () => sut.GetData(null!));
        }

        [Fact]
        public void GeneratesTestsFromParameterlessMethod()
        {
            var memberName = nameof(TestTypeWithMethodData.GetSingleStringValueTestCases);
            var sut = new MemberAutoDataAttribute(memberName);
            var testMethod = TestTypeWithMethodData.GetSingleStringValueTestMethodInfo();

            var testCases = sut.GetData(testMethod);

            Assert.Collection(testCases,
                testCase => Assert.Equal("value-one", testCase.Single()),
                testCase => Assert.Equal("value-two", testCase.Single()),
                testCase => Assert.Equal("value-three", testCase.Single()));
        }

        [Fact]
        public void GeneratesTestsFromMethodWithParameter()
        {
            var memberName = nameof(TestTypeWithMethodData.GetStringTestsFromArgument);
            var sut = new MemberAutoDataAttribute(memberName, "testcase");
            var testMethod = TestTypeWithMethodData.GetStringTestsFromArgumentMethodInfo();

            var testCases = sut.GetData(testMethod);

            Assert.Collection(testCases,
                testCase => Assert.Equal("testcase-one", testCase.Single()),
                testCase => Assert.Equal("testcase-two", testCase.Single()),
                testCase => Assert.Equal("testcase-three", testCase.Single()));
        }

        [Fact]
        public void GeneratesTestCasesForTestsWithMultipleParameters()
        {
            // Arrange
            var memberName = nameof(TestTypeWithMethodData.GetMultipleValueTestCases);
            var sut = new MemberAutoDataAttribute(memberName);
            var testMethod = TestTypeWithMethodData.GetMultipleValueTestMethodInfo();

            // Act
            var testCases = sut.GetData(testMethod);

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
            var memberName = nameof(TestTypeWithMethodData.GetSingleStringValueTestCases);
            var sut = new MemberAutoDataAttribute(memberName);
            var testMethod = TestTypeWithMethodData.GetMultipleValueTestMethodInfo();

            // Act
            var testCases = sut.GetData(testMethod);

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
            var memberName = nameof(TestTypeWithMethodData.GetTestWithFrozenParameterCases);
            var sut = new MemberAutoDataAttribute(memberName);
            var testMethod = TestTypeWithMethodData.GetTestWithFrozenParameter();

            // Act
            var testCases = sut.GetData(testMethod);

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
            var memberName = nameof(TestTypeWithMethodData.GetSingleStringValueTestCases);
            var sut = new MemberAutoDataAttribute(memberName);
            var testMethod = TestTypeWithMethodData.GetTestWithFrozenParameter();

            // Act
            var testCases = sut.GetData(testMethod);

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
    }
}
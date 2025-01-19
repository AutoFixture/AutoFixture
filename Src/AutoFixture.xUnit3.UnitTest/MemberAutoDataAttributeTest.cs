using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit3.UnitTest.TestTypes;
using TestTypeFoundation;
using Xunit;
using Xunit.Sdk;
using Xunit.v3;

namespace AutoFixture.Xunit3.UnitTest
{
    [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local",
        Justification = "Using parameter for precondition checks is acceptable in assertions.")]
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
        public void TreatsNullParametersAsArrayWithNullValue()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();

            // Act
            var actual = new MemberAutoDataAttribute(memberName, null!);

            // Act & Assert
            var value = Assert.Single(actual.Parameters);
            Assert.Null(value);
        }

        [Fact]
        public void DoesNotThrowWhenInitializedWithNullType()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();

            // Act & Assert
            _ = new MemberAutoDataAttribute(null!, memberName);
        }

        [Fact]
        public async Task ThrowsWhenTestMethodNull()
        {
            // Arrange
            var sut = new MemberAutoDataAttribute("memberName");

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => sut.GetData(null!, new DisposalTracker()).AsTask());
        }

        [Fact]
        public async Task ThrowsWhenMemberNotEnumerable()
        {
            // Arrange
            var memberName = nameof(TestTypeWithMethodData.NonEnumerableMethod);
            var sut = new MemberAutoDataAttribute(memberName);
            var method = TestTypeWithMethodData.GetNonEnumerableMethodInfo();

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(
                () => sut.GetData(method!, new DisposalTracker()).AsTask());
            Assert.Contains(memberName, ex.Message);
        }

        [Fact]
        public async Task ThrowsWhenMemberNotStatic()
        {
            // Arrange
            const string memberName = nameof(TestTypeWithMethodData.NonStaticSource);
            var sut = new MemberAutoDataAttribute(memberName);
            var method = TestTypeWithMethodData.GetNonStaticSourceMethodInfo();

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(
                () => sut.GetData(method!, new DisposalTracker()).AsTask());
            Assert.Contains(memberName, ex.Message);
        }

        [Fact]
        public async Task ThrowsWhenMemberDoesNotExist()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();
            var sut = new MemberAutoDataAttribute(typeof(TestTypeWithMethodData), memberName);
            var method = TestTypeWithMethodData.GetMultipleValueTestMethodInfo();

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(
                () => sut.GetData(method!, new DisposalTracker()).AsTask());
            Assert.Contains(memberName, ex.Message);
        }

        [Fact]
        public void DoesntActivateFixtureImmediately()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();
            var wasInvoked = false;

            // Act
            _ = new DerivedMemberAutoDataAttribute(() =>
            {
                wasInvoked = true;
                return new DelegatingFixture();
            }, memberName);

            // Assert
            Assert.False(wasInvoked);
        }

        [Fact]
        public void PreDiscoveryShouldBeDisabled()
        {
            // Arrange
            var sut = new MemberAutoDataAttribute("memberName");

            // Act
            var preDiscoverer = sut.SupportsDiscoveryEnumeration();

            // Assert
            Assert.False(preDiscoverer);
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
        public async Task GetDataOrdersCustomizationAttributes(string methodName)
        {
            // Arrange
            var method = typeof(TypeWithCustomizationAttributes)
                .GetMethod(methodName, new[] { typeof(ConcreteType) })!;
            var customizationLog = new List<ICustomization>();
            var fixture = new DelegatingFixture
            {
                OnCustomize = c => customizationLog.Add(c)
            };

            var sut = new DerivedMemberAutoDataAttribute(
                () => fixture,
                typeof(TestTypeWithMethodData),
                nameof(TestTypeWithMethodData.TestDataWithNoValues));

            // Act
            _ = await sut.GetData(method!, new DisposalTracker());

            // Assert
            var composite = Assert.IsAssignableFrom<CompositeCustomization>(customizationLog[0]);
            Assert.IsNotType<FreezeOnMatchCustomization>(composite.Customizations.First());
            Assert.IsType<FreezeOnMatchCustomization>(composite.Customizations.Last());
        }

        [Fact]
        public async Task GeneratesTestsFromParameterlessMethod()
        {
            // Arrange
            const string memberName = nameof(TestTypeWithMethodData.GetSingleStringValueTestData);
            var sut = new MemberAutoDataAttribute(memberName);
            var testMethod = TestTypeWithMethodData.GetSingleStringValueTestMethodInfo();
            var expected = new[]
            {
                new object[] { "value-one" },
                new object[] { "value-two" },
                new object[] { "value-three" }
            };

            // Act
            var testData = (await sut.GetData(testMethod!, new DisposalTracker()))
                .Select(x => x.GetData()).ToArray();

            // Assert
            Assert.Equal(expected, testData);
        }

        [Fact]
        public async Task GeneratesTestsFromMethodWithParameter()
        {
            // Arrange
            const string memberName = nameof(TestTypeWithMethodData.GetStringTestsFromArgument);
            var sut = new MemberAutoDataAttribute(memberName, "value");
            var testMethod = TestTypeWithMethodData.GetStringTestsFromArgumentMethodInfo();
            var expected = new[]
            {
                new object[] { "value-one" },
                new object[] { "value-two" },
                new object[] { "value-three" }
            };

            // Act
            var testData = (await sut.GetData(testMethod!, new DisposalTracker()))
                .Select(x => x.GetData()).ToArray();

            Assert.Equal(expected, testData);
        }

        [Fact]
        public async Task GeneratesTestDataForTestsWithMultipleParameters()
        {
            // Arrange
            const string memberName = nameof(TestTypeWithMethodData.GetMultipleValueTestData);
            var sut = new MemberAutoDataAttribute(memberName);
            var testMethod = TestTypeWithMethodData.GetMultipleValueTestMethodInfo();
            var expected = new[]
            {
                new object[] { "value-one", 12, 23.3m },
                new object[] { "value-two", 38, 12.7m },
                new object[] { "value-three", 94, 52.21m }
            };

            // Act
            var testData = (await sut.GetData(testMethod!, new DisposalTracker()))
                .Select(x => x.GetData()).ToArray();

            // Assert
            Assert.Equal(expected, testData);
        }

        [Fact]
        public async Task GeneratesMissingDataForTestsWithMultipleParameters()
        {
            // Arrange
            const string memberName = nameof(TestTypeWithMethodData.GetSingleStringValueTestData);
            var sut = new MemberAutoDataAttribute(memberName);
            var testMethod = TestTypeWithMethodData.GetMultipleValueTestMethodInfo();

            // Act
            var testData = (await sut.GetData(testMethod!, new DisposalTracker()))
                .Select(x => x.GetData()).ToArray();

            // Assert
            Assert.Collection(testData,
                arguments =>
                {
                    Assert.Equal(3, arguments.Length);
                    Assert.Equal("value-one", arguments[0]);
                    Assert.NotEqual(0, arguments[1]);
                    Assert.NotEqual(0, arguments[2]);
                },
                arguments =>
                {
                    Assert.Equal(3, arguments.Length);
                    Assert.Equal("value-two", arguments[0]);
                    Assert.NotEqual(0, arguments[1]);
                    Assert.NotEqual(0, arguments[2]);
                },
                arguments =>
                {
                    Assert.Equal(3, arguments.Length);
                    Assert.Equal("value-three", arguments[0]);
                    Assert.NotEqual(0, arguments[1]);
                    Assert.NotEqual(0, arguments[2]);
                });
        }

        [Fact]
        public async Task GeneratesTestDataWithInjectedParameters()
        {
            // Arrange
            const string memberName = nameof(TestTypeWithMethodData.GetDataForTestWithFrozenParameter);
            var sut = new MemberAutoDataAttribute(memberName);
            var testMethod = TestTypeWithMethodData.GetTestWithFrozenParameter();
            var expected = new[]
            {
                new object[] { "value-one", "value-two", "value-two" },
                new object[] { "value-two", "value-three", "value-three" },
                new object[] { "value-three", "value-one", "value-one" }
            };

            // Act
            var testData = (await sut.GetData(testMethod!, new DisposalTracker()))
                .Select(x => x.GetData()).ToArray();

            // Assert
            Assert.Equal(expected, testData);
        }

        [Fact]
        public async Task AutoGeneratesValuesForFrozenParameters()
        {
            // Arrange
            const string memberName = nameof(TestTypeWithMethodData.GetSingleStringValueTestData);
            var sut = new MemberAutoDataAttribute(memberName);
            var testMethod = TestTypeWithMethodData.GetTestWithFrozenParameter();

            // Act
            var testData = (await sut.GetData(testMethod!, new DisposalTracker()))
                .Select(x => x.GetData()).ToArray();

            // Assert
            Assert.Collection(testData,
                arguments =>
                {
                    Assert.Equal(3, arguments.Length);
                    Assert.Equal("value-one", arguments[0]);
                    Assert.NotEmpty(arguments[1].ToString()!);
                    Assert.Equal(arguments[1], arguments[2]);
                },
                arguments =>
                {
                    Assert.Equal(3, arguments.Length);
                    Assert.Equal("value-two", arguments[0]);
                    Assert.NotEmpty(arguments[1].ToString()!);
                    Assert.Equal(arguments[1], arguments[2]);
                },
                arguments =>
                {
                    Assert.Equal(3, arguments.Length);
                    Assert.Equal("value-three", arguments[0]);
                    Assert.NotEmpty(arguments[1].ToString()!);
                    Assert.Equal(arguments[1], arguments[2]);
                });
        }

        [Fact]
        public async Task SupportsInheritedTestDataMembers()
        {
            // Arrange
            const string memberName = nameof(TestTypeWithMethodData.GetMultipleValueTestData);
            var sut = new MemberAutoDataAttribute(memberName);
            var testMethod = ChildTestTypeMethodData.GetMultipleValueTestMethodInfo();
            var expected = new[]
            {
                new object[] { "value-one", 12, 23.3m },
                new object[] { "value-two", 38, 12.7m },
                new object[] { "value-three", 94, 52.21m }
            };

            // Act
            var testData = (await sut.GetData(testMethod!, new DisposalTracker()))
                .Select(x => x.GetData()).ToArray();

            // Assert
            Assert.Equal(expected, testData);
        }

        public static IEnumerable<object[]> TestDataWithNullValues
        {
            get
            {
                yield return new object[] { null, null };
                yield return new object[] { string.Empty, null };
                yield return new object[] { " ", null };
            }
        }

        [Theory]
        [MemberAutoData(nameof(TestDataWithNullValues))]
        public void NullTestDataReturned(string a, string b, PropertyHolder<string> c)
        {
            Assert.True(string.IsNullOrWhiteSpace(a));
            Assert.Null(b);
            Assert.NotNull(c);
        }
    }
}
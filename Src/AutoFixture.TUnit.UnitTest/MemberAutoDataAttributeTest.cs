using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AutoFixture.TUnit.UnitTest.TestTypes;
using TestTypeFoundation;
using TUnit.Assertions.AssertConditions.Throws;

namespace AutoFixture.TUnit.UnitTest
{
    [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local",
        Justification = "Using parameter for precondition checks is acceptable in assertions.")]
    public class MemberAutoDataAttributeTest
    {
        [Test]
        public async Task SutIsDataAttribute()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();

            // Act
            var sut = new MemberAutoDataAttribute(memberName);

            // Assert
            await Assert.That(sut).IsTypeOf<NonTypedDataSourceGeneratorAttribute>();
        }

        [Test]
        public async Task InitializedWithMemberNameAndParameters()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();
            var parameters = new object[] { "value-one", 3, 12.2f };

            // Act
            var sut = new MemberAutoDataAttribute(memberName, parameters);

            // Assert
            await Assert.That(sut.MemberName).IsEqualTo(memberName);
            await Assert.That(sut.Parameters).IsEqualTo(parameters);
            await Assert.That(sut.MemberType).IsNull();
            await Assert.That(sut.FixtureFactory).IsNotNull();
        }

        [Test]
        public async Task InitializedWithTypeMemberNameAndParameters()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();
            var parameters = new object[] { "value-one", 3, 12.2f };
            var testType = typeof(MemberAutoDataAttributeTest);

            // Act
            var sut = new MemberAutoDataAttribute(testType, memberName, parameters);

            // Assert
            await Assert.That(sut.MemberName).IsEqualTo(memberName);
            await Assert.That(sut.Parameters).IsEqualTo(parameters);
            await Assert.That(sut.MemberType).IsEqualTo(testType);
            await Assert.That(sut.FixtureFactory).IsNotNull();
        }

        [Test]
        public async Task ThrowsWhenInitializedWithNullMemberName()
        {
            // Act & Assert
            await Assert.That(() => new MemberAutoDataAttribute(null!)).ThrowsExactly<ArgumentNullException>();
        }

        [Test]
        public async Task TreatsNullParametersAsArrayWithNullValue()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();

            // Act
            var actual = new MemberAutoDataAttribute(memberName, null!);

            // Act & Assert
            var value = await Assert.That(actual.Parameters).HasSingleItem();
            await Assert.That(value).IsNull();
        }

        [Test]
        public void DoesNotThrowWhenInitializedWithNullType()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();

            // Act & Assert
            _ = new MemberAutoDataAttribute(null!, memberName);
        }

        [Test]
        public async Task ThrowsWhenTestMethodNull()
        {
            // Arrange
            var sut = new MemberAutoDataAttribute("memberName");

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => sut.GetData(null!, new DisposalTracker()).AsTask());
        }

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
        public async Task DoesntActivateFixtureImmediately()
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
            await Assert.That(wasInvoked).IsFalse();
        }

        [Test]
        public void PreDiscoveryShouldBeDisabled()
        {
            // Arrange
            var sut = new MemberAutoDataAttribute("memberName");

            // Act
            var preDiscoverer = sut.SupportsDiscoveryEnumeration();

            // Assert
            Assert.That(preDiscoverer).IsFalse();
        }

        [Test]
        [Arguments("CreateWithFrozenAndFavorArrays")]
        [Arguments("CreateWithFavorArraysAndFrozen")]
        [Arguments("CreateWithFrozenAndFavorEnumerables")]
        [Arguments("CreateWithFavorEnumerablesAndFrozen")]
        [Arguments("CreateWithFrozenAndFavorLists")]
        [Arguments("CreateWithFavorListsAndFrozen")]
        [Arguments("CreateWithFrozenAndGreedy")]
        [Arguments("CreateWithGreedyAndFrozen")]
        [Arguments("CreateWithFrozenAndModest")]
        [Arguments("CreateWithModestAndFrozen")]
        [Arguments("CreateWithFrozenAndNoAutoProperties")]
        [Arguments("CreateWithNoAutoPropertiesAndFrozen")]
        public async Task GetDataOrdersCustomizationAttributes(string methodName)
        {
            // Arrange
            var method = typeof(TypeWithCustomizationAttributes)
                .GetMethod(methodName, [typeof(ConcreteType)])!;
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
            var composite = await Assert.That(customizationLog[0]).IsTypeOf<CompositeCustomization>();
            Assert.IsNotType<FreezeOnMatchCustomization>(composite.Customizations.First());
            Assert.IsType<FreezeOnMatchCustomization>(composite.Customizations.Last());
        }

        [Test]
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
            Assert.That(testData).IsEqualTo(expected);
        }

        [Test]
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

            Assert.That(testData).IsEqualTo(expected);
        }

        [Test]
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
            Assert.That(testData).IsEqualTo(expected);
        }

        [Test]
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
                    Assert.That(arguments.Length).IsEqualTo(3);
                    Assert.That(arguments[0]).IsEqualTo("value-one");
                    Assert.That(arguments[1]).IsNotEqualTo(0);
                    Assert.That(arguments[2]).IsNotEqualTo(0);
                },
                arguments =>
                {
                    Assert.That(arguments.Length).IsEqualTo(3);
                    Assert.That(arguments[0]).IsEqualTo("value-two");
                    Assert.That(arguments[1]).IsNotEqualTo(0);
                    Assert.That(arguments[2]).IsNotEqualTo(0);
                },
                arguments =>
                {
                    Assert.That(arguments.Length).IsEqualTo(3);
                    Assert.That(arguments[0]).IsEqualTo("value-three");
                    Assert.That(arguments[1]).IsNotEqualTo(0);
                    Assert.That(arguments[2]).IsNotEqualTo(0);
                });
        }

        [Test]
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
            Assert.That(testData).IsEqualTo(expected);
        }

        [Test]
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
                    Assert.That(arguments.Length).IsEqualTo(3);
                    Assert.That(arguments[0]).IsEqualTo("value-one");
                    Assert.That(arguments[1].ToString()!).IsNotEmpty();
                    Assert.That(arguments[2]).IsEqualTo(arguments[1]);
                },
                arguments =>
                {
                    Assert.That(arguments.Length).IsEqualTo(3);
                    Assert.That(arguments[0]).IsEqualTo("value-two");
                    Assert.That(arguments[1].ToString()!).IsNotEmpty();
                    Assert.That(arguments[2]).IsEqualTo(arguments[1]);
                },
                arguments =>
                {
                    Assert.That(arguments.Length).IsEqualTo(3);
                    Assert.That(arguments[0]).IsEqualTo("value-three");
                    Assert.That(arguments[1].ToString()!).IsNotEmpty();
                    Assert.That(arguments[2]).IsEqualTo(arguments[1]);
                });
        }

        [Test]
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
            Assert.That(testData).IsEqualTo(expected);
        }

        public static IEnumerable<object[]> TestDataWithNullValues
        {
            get
            {
                yield return [null, null];
                yield return [string.Empty, null];
                yield return [" ", null];
            }
        }

        [Test]
        [MemberAutoData(nameof(TestDataWithNullValues))]
        public async Task NullTestDataReturned(string a, string b, PropertyHolder<string> c)
        {
            await Assert.That(string.IsNullOrWhiteSpace(a)).IsTrue();
            await Assert.That(b).IsNull();
            await Assert.That(c).IsNotNull();
        }
    }
}
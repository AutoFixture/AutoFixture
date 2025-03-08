using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.TUnit.Internal;
using AutoFixture.TUnit.UnitTest.TestTypes;
using TUnit.Assertions.AssertConditions.Throws;

namespace AutoFixture.TUnit.UnitTest.Internal;

public class MemberDataSourceTests
{
    public static object NonTestDataField = new();
    public static object NonTestDataProperty => new();
    public static object NonTestDataMethod() => new();
    public static IEnumerable<object[]> EmptyTestDataField = Array.Empty<object[]>();
    public static IEnumerable<object[]> EmptyTestData => Array.Empty<object[]>();
    public static IEnumerable<object[]> GetEmptyTestData() => Array.Empty<object[]>();

    [Test]
    public async Task SutIsTestDataSource()
    {
        // Arrange & Act
        var sut = new MemberDataSource(
            typeof(MemberDataSourceTests),
            nameof(GetEmptyTestData));

        // Assert
        await Assert.That(sut).IsTypeOf<IDataSource>();
    }

    [Test]
    public async Task ThrowsWhenTypeIsNull()
    {
        // Arrange
        var method = nameof(GetEmptyTestData);

        // Act & Assert
        await Assert.That(() => new MemberDataSource(null!, method)).ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task ThrowsWhenNameIsNull()
    {
        // Arrange
        var type = typeof(MemberDataSourceTests);

        // Act & Assert
        await Assert.That(() => new MemberDataSource(type, null!)).ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task ThrowsWhenArgumentsIsNull()
    {
        // Arrange
        var type = typeof(MemberDataSourceTests);
        var method = nameof(GetEmptyTestData);

        // Act & Assert
        await Assert.That(() => new MemberDataSource(type, method, null!)).ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task InitializesTypeProperty()
    {
        // Arrange
        var type = typeof(MemberDataSourceTests);
        var method = nameof(GetEmptyTestData);

        // Act
        var sut = new MemberDataSource(type, method);

        // Assert
        await Assert.That(sut.Type).IsEqualTo(type);
        await Assert.That(sut.Name).IsEqualTo(method);
        await Assert.That(sut.Arguments).IsEmpty();
    }

    [Test]
    [Arguments(nameof(EmptyTestDataField), typeof(FieldDataSource))]
    [Arguments(nameof(EmptyTestData), typeof(PropertyDataSource))]
    [Arguments(nameof(GetEmptyTestData), typeof(MethodDataSource))]
    public async Task InitializesSourceProperty(string memberName, Type expectedInnerSourceType)
    {
        // Arrange
        var type = typeof(MemberDataSourceTests);

        // Act
        var sut = new DelegatingMemberDataSource(type, memberName);

        // Assert
        await Assert.That(sut.GetSource()).IsTypeOf(expectedInnerSourceType);
    }

    [Test]
    public async Task ThrowsWhenSourceDoesNotExist()
    {
        // Arrange
        var type = typeof(MemberDataSourceTests);

        // Act & Assert
        await Assert.That(() => _ = new DelegatingMemberDataSource(type, "NonExistentMember")).ThrowsExactly<ArgumentException>();
    }

    [Test]
    [Arguments(nameof(NonTestDataField))]
    [Arguments(nameof(NonTestDataProperty))]
    [Arguments(nameof(NonTestDataMethod))]
    public async Task ThrowsWhenSourceDoesNotReturnTestData(string memberName)
    {
        // Arrange
        var type = typeof(MemberDataSourceTests);

        // Act & Assert
        await Assert.That(() => _ = new DelegatingMemberDataSource(type, memberName)).ThrowsExactly<ArgumentException>();
    }
}
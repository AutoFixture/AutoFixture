using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.TUnit.Internal;
using AutoFixture.TUnit.UnitTest.TestTypes;

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
    public void SutIsTestDataSource()
    {
        // Arrange & Act
        var sut = new MemberDataSource(
            typeof(MemberDataSourceTests),
            nameof(GetEmptyTestData));

        // Assert
        Assert.IsAssignableFrom<IDataSource>(sut);
    }

    [Test]
    public void ThrowsWhenTypeIsNull()
    {
        // Arrange
        var method = nameof(GetEmptyTestData);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => new MemberDataSource(null!, method));
    }

    [Test]
    public void ThrowsWhenNameIsNull()
    {
        // Arrange
        var type = typeof(MemberDataSourceTests);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => new MemberDataSource(type, null!));
    }

    [Test]
    public void ThrowsWhenArgumentsIsNull()
    {
        // Arrange
        var type = typeof(MemberDataSourceTests);
        var method = nameof(GetEmptyTestData);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => new MemberDataSource(type, method, null!));
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
        Assert.That(sut.Arguments).IsEmpty();
    }

    [Test]
    [Arguments(nameof(EmptyTestDataField), typeof(FieldDataSource))]
    [Arguments(nameof(EmptyTestData), typeof(PropertyDataSource))]
    [Arguments(nameof(GetEmptyTestData), typeof(MethodDataSource))]
    public void InitializesSourceProperty(string memberName, Type expectedInnerSourceType)
    {
        // Arrange
        var type = typeof(MemberDataSourceTests);

        // Act
        var sut = new DelegatingMemberDataSource(type, memberName);

        // Assert
        Assert.IsType(expectedInnerSourceType, sut.GetSource());
    }

    [Test]
    public void ThrowsWhenSourceDoesNotExist()
    {
        // Arrange
        var type = typeof(MemberDataSourceTests);

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => _ = new DelegatingMemberDataSource(type, "NonExistentMember"));
    }

    [Test]
    [Arguments(nameof(NonTestDataField))]
    [Arguments(nameof(NonTestDataProperty))]
    [Arguments(nameof(NonTestDataMethod))]
    public void ThrowsWhenSourceDoesNotReturnTestData(string memberName)
    {
        // Arrange
        var type = typeof(MemberDataSourceTests);

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => _ = new DelegatingMemberDataSource(type, memberName));
    }
}
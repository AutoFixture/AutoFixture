using System;
using System.Collections.Generic;
using AutoFixture.Xunit3.Internal;
using AutoFixture.Xunit3.UnitTest.TestTypes;
using Xunit;

namespace AutoFixture.Xunit3.UnitTest.Internal;

public class MemberDataSourceTests
{
    public static object NonTestDataField = new();
    public static object NonTestDataProperty => new();
    public static object NonTestDataMethod() => new();
    public static IEnumerable<object[]> EmptyTestDataField = Array.Empty<object[]>();
    public static IEnumerable<object[]> EmptyTestData => Array.Empty<object[]>();
    public static IEnumerable<object[]> GetEmptyTestData() => Array.Empty<object[]>();

    [Fact]
    public void SutIsTestDataSource()
    {
        // Arrange & Act
        var sut = new MemberDataSource(
            typeof(MemberDataSourceTests),
            nameof(GetEmptyTestData));

        // Assert
        Assert.IsAssignableFrom<IDataSource>(sut);
    }

    [Fact]
    public void ThrowsWhenTypeIsNull()
    {
        // Arrange
        var method = nameof(GetEmptyTestData);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => new MemberDataSource(null!, method));
    }

    [Fact]
    public void ThrowsWhenNameIsNull()
    {
        // Arrange
        var type = typeof(MemberDataSourceTests);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => new MemberDataSource(type, null!));
    }

    [Fact]
    public void ThrowsWhenArgumentsIsNull()
    {
        // Arrange
        var type = typeof(MemberDataSourceTests);
        var method = nameof(GetEmptyTestData);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => new MemberDataSource(type, method, null!));
    }

    [Fact]
    public void InitializesTypeProperty()
    {
        // Arrange
        var type = typeof(MemberDataSourceTests);
        var method = nameof(GetEmptyTestData);

        // Act
        var sut = new MemberDataSource(type, method);

        // Assert
        Assert.Equal(type, sut.Type);
        Assert.Equal(method, sut.Name);
        Assert.Empty(sut.Arguments);
    }

    [Theory]
    [InlineData(nameof(EmptyTestDataField), typeof(FieldDataSource))]
    [InlineData(nameof(EmptyTestData), typeof(PropertyDataSource))]
    [InlineData(nameof(GetEmptyTestData), typeof(MethodDataSource))]
    public void InitializesSourceProperty(string memberName, Type expectedInnerSourceType)
    {
        // Arrange
        var type = typeof(MemberDataSourceTests);

        // Act
        var sut = new DelegatingMemberDataSource(type, memberName);

        // Assert
        Assert.IsType(expectedInnerSourceType, sut.GetSource());
    }

    [Fact]
    public void ThrowsWhenSourceDoesNotExist()
    {
        // Arrange
        var type = typeof(MemberDataSourceTests);

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => _ = new DelegatingMemberDataSource(type, "NonExistentMember"));
    }

    [Theory]
    [InlineData(nameof(NonTestDataField))]
    [InlineData(nameof(NonTestDataProperty))]
    [InlineData(nameof(NonTestDataMethod))]
    public void ThrowsWhenSourceDoesNotReturnTestData(string memberName)
    {
        // Arrange
        var type = typeof(MemberDataSourceTests);

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => _ = new DelegatingMemberDataSource(type, memberName));
    }
}
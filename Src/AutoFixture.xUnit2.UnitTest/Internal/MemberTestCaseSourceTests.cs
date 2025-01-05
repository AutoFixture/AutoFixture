using System;
using System.Collections.Generic;
using AutoFixture.Xunit2.Internal;
using Xunit;

namespace AutoFixture.Xunit2.UnitTest.Internal;

public class MemberTestCaseSourceTests
{
    public static object NonTestDataField = new();
    public static object NonTestDataProperty => new();
    public static object NonTestDataMethod() => new();
    public static IEnumerable<object[]> EmptyTestDataField = Array.Empty<object[]>();
    public static IEnumerable<object[]> EmptyTestData => Array.Empty<object[]>();
    public static IEnumerable<object[]> GetEmptyTestData() => Array.Empty<object[]>();

    [Fact]
    public void SutIsTestCaseSource()
    {
        // Arrange & Act
        var sut = new MemberTestCaseSource(
            typeof(MemberTestCaseSourceTests),
            nameof(GetEmptyTestData));

        // Assert
        Assert.IsAssignableFrom<ITestCaseSource>(sut);
    }

    [Fact]
    public void ThrowsWhenTypeIsNull()
    {
        // Arrange
        var method = nameof(GetEmptyTestData);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => new MemberTestCaseSource(null!, method));
    }

    [Fact]
    public void ThrowsWhenNameIsNull()
    {
        // Arrange
        var type = typeof(MemberTestCaseSourceTests);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => new MemberTestCaseSource(type, null!));
    }

    [Fact]
    public void ThrowsWhenArgumentsIsNull()
    {
        // Arrange
        var type = typeof(MemberTestCaseSourceTests);
        var method = nameof(GetEmptyTestData);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => new MemberTestCaseSource(type, method, null!));
    }

    [Fact]
    public void InitializesTypeProperty()
    {
        // Arrange
        var type = typeof(MemberTestCaseSourceTests);
        var method = nameof(GetEmptyTestData);

        // Act
        var sut = new MemberTestCaseSource(type, method);

        // Assert
        Assert.Equal(type, sut.Type);
        Assert.Equal(method, sut.Name);
        Assert.Empty(sut.Arguments);
    }

    [Theory]
    [InlineData(nameof(EmptyTestDataField), typeof(FieldTestCaseSource))]
    [InlineData(nameof(EmptyTestData), typeof(PropertyTestCaseSource))]
    [InlineData(nameof(GetEmptyTestData), typeof(MethodTestCaseSource))]
    public void InitializesSourceProperty(string memberName, Type expectedInnerSourceType)
    {
        // Arrange
        var type = typeof(MemberTestCaseSourceTests);

        // Act
        var sut = new DelegatingMemberTestCaseSource(type, memberName);

        // Assert
        Assert.IsType(expectedInnerSourceType, sut.GetSource());
    }

    [Fact]
    public void ThrowsWhenSourceDoesNotExist()
    {
        // Arrange
        var type = typeof(MemberTestCaseSourceTests);

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => _ = new DelegatingMemberTestCaseSource(type, "NonExistentMember"));
    }

    [Theory]
    [InlineData(nameof(NonTestDataField))]
    [InlineData(nameof(NonTestDataProperty))]
    [InlineData(nameof(NonTestDataMethod))]
    public void ThrowsWhenSourceDoesNotReturnTestData(string memberName)
    {
        // Arrange
        var type = typeof(MemberTestCaseSourceTests);

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => _ = new DelegatingMemberTestCaseSource(type, memberName));
    }
}
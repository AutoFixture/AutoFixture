using System.Reflection;
using AutoFixture.NUnit2.Addins.Builders;
using NUnit.Framework;

namespace AutoFixture.NUnit2.UnitTest;

[TestFixture]
public class AutoDataProviderTest
{
    private readonly MethodInfo _method;

    public AutoDataProviderTest()
    {
        _method = typeof(FakeAutoDataFixture).GetMethod("DoSomething");
    }

    [Test]
    public void HasTestCasesForAutoDataProvider()
    {
        // Arrange
        // Act
        var sut = new AutoDataProvider();
        var actual = sut.HasTestCasesFor(_method);
        // Assert
        Assert.True(actual);
    }

    [Test]
    public void GetTestCasesForAutoDataBuilderReturnsCorrectly()
    {
        // Arrange
        // Act
        var sut = new AutoDataProvider();
        var actual = sut.GetTestCasesFor(_method);
        // Assert
        Assert.NotNull(actual);
    }
}
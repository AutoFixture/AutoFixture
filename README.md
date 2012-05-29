Introduction
------------------------------------------------------------------------------------------------
AutoFixture is an open source framework for .NET designed to minimize the 'Arrange' phase of your unit tests.  
Its primary goal is to allow developers to focus on *what* is being tested rather than *how to setup* the test scenario, by making it easier to create object graphs containing test data.

The API is designed to make Test-Driven Development more productive and unit tests more refactoring-safe. It does so by removing the need for hand-coding anonymous variables as part of a test's [Fixture Setup][1] phase. Among other features, it also offers a generic implementation of the [Test Data Builder][2] pattern.

Overview
------------------------------------------------------------------------------------------------
When writing unit tests, you typically need to create some objects that represent the initial state of the test. Often, an API will force you to specify much more data than you really care about, so you frequently end up creating objects that has no influence on the test, simply to make the code compile.

AutoFixture can help by creating such [Anonymous Variables][3] for you. Here's an example:

    [Test]
    public void Echo_WithAnonymousInteger_ReturnsSameInteger()
    {
        // Arrange
        Fixture fixture = new Fixture();
        int expectedNumber = fixture.CreateAnonymous<int>();
        MyClass sut = fixture.CreateAnonymous<MyClass>();
    
        // Act
        int result = sut.Echo(expectedNumber);
    
        // Assert
        Assert.AreEqual(expectedNumber, result, "The method did not return the expected number");
    }

This example illustrates the basic principle of AutoFixture:

> *AutoFixture can create values of
> virtually any type without the need
> for you to explicitly define which
> values should be used.*

The number `expectedNumber` is created by a call to `Fixture.CreateAnonymous<T>`, which will create a regular integer value, saving you the effort of explicitly coming up with one.

The example also illustrates how AutoFixture can be used as a [SUT Factory][4] that creates the actual [System Under Test][5].

  [1]: http://xunitpatterns.com/Fixture%20Setup%20Patterns.html
  [2]: http://www.natpryce.com/articles/000714.html
  [3]: http://blogs.msdn.com/ploeh/archive/2008/11/17/anonymous-variables.aspx
  [4]: http://blog.ploeh.dk/2009/02/13/SUTFactory.aspx
  [5]: http://xunitpatterns.com/SUT.html
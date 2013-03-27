## Project Description ##

Write maintainable unit tests, faster.

AutoFixture makes it easier for developers to do Test-Driven Development by automating non-relevant Test Fixture Setup, allowing the Test Developer to focus on the essentials of each test case.

## Overview ##

(Jump straight to the [CheatSheet](https://github.com/AutoFixture/AutoFixture/wiki/Cheat-Sheet) if you just want to see some code samples right away.)

AutoFixture is designed to make Test-Driven Development more productive and unit tests more refactoring-safe. It does so by removing the need for hand-coding anonymous variables as part of a test's Fixture Setup phase. Among other features, it offers a generic implementation of the [Test Data Builder](http://www.natpryce.com/articles/000714.html) pattern.

When writing unit tests, you typically need to create some objects that represent the initial state of the test. Often, an API will force you to specify much more data than you really care about, so you frequently end up creating objects that has no influence on the test, simply to make the code compile.

AutoFixture can help by creating such [Anonymous Variables](http://blogs.msdn.com/ploeh/archive/2008/11/17/anonymous-variables.aspx) for you. Here's a simple example:

```csharp
[TestMethod]
public void IntroductoryTest()
{
    // Fixture setup
    Fixture fixture = new Fixture();

    int expectedNumber = fixture.Create<int>();
    MyClass sut = fixture.Create<MyClass>();
    // Exercise system
    int result = sut.Echo(expectedNumber);
    // Verify outcome
    Assert.AreEqual<int>(expectedNumber, result, "Echo");
    // Teardown
}
```

This example illustrates the basic principle of AutoFixture: It can create values of virtually any type without the need for you to explicitly define which values should be used. The number *expectedNumber* is created by a call to CreateAnonymous<T> - this will create a 'nice', regular integer value, saving you the effort of explicitly coming up with one.

The example also illustrates how AutoFixture can be used as a [SUT Factory](http://blog.ploeh.dk/2009/02/13/SUTFactory.aspx) that creates the actual System Under Test (the MyClass instance).

Given the right combination of unit testing framework and extensions for AutoFixture, we can [further reduce the above test to be even more declarative](http://blog.ploeh.dk/2010/10/08/AutoDataTheoriesWithAutoFixture.aspx):

```csharp
[Theory, AutoData]
public void IntroductoryTest(
    int expectedNumber, MyClass sut)
{
    int result = sut.Echo(expectedNumber);
    Assert.Equal(expectedNumber, result);
}
```

Notice how we can reduce unit tests to state only the relevant parts of the test. The rest (variables, Fixture object) is relegated to attributes and parameter values that are supplied automatically by AutoFixture. The test is now only two lines of code.

Using AutoFixture is as easy as referencing the library and creating a new instance of the Fixture class!

AutoFixture is available via NuGet:

* [AutoFixture](http://nuget.org/packages/AutoFixture)
* [AutoFixture.AutoMoq](http://nuget.org/packages/AutoFixture.AutoMoq)
* [AutoFixture.AutoRhinoMocks](http://nuget.org/packages/AutoFixture.AutoRhinoMocks)
* [AutoFixture.AutoFakeItEasy](http://nuget.org/packages/AutoFixture.AutoFakeItEasy)
* [AutoFixture.AutoNSubstitute](http://nuget.org/packages/AutoFixture.AutoNSubstitute)
* [AutoFixture.Xunit](http://nuget.org/packages/AutoFixture.Xunit)
* [AutoFixture.Idioms](http://nuget.org/packages/AutoFixture.Idioms)
* [SemanticComparison](http://nuget.org/packages/SemanticComparison)

## Documentation ##

* [CheatSheet](https://github.com/AutoFixture/AutoFixture/wiki/Cheat-Sheet)
* [FAQ](https://github.com/AutoFixture/AutoFixture/wiki/FAQ)

## Additional resources ##

* [ploeh blog](http://blog.ploeh.dk/tags.html#AutoFixture-ref)
* [Nikos Baxevanis' blog](http://nikosbaxevanis.com/categories/AutoFixture)
* [Enrico Campidoglio's blog] (http://megakemp.com/tag/autofixture)
* [Questions on Stack Overflow](http://stackoverflow.com/questions/tagged/autofixture)

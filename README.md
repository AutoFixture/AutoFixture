[![Build status](https://ci.appveyor.com/api/projects/status/xove2l9aj4d1rej5/branch/master?svg=true)](https://ci.appveyor.com/project/AutoFixture/autofixture/branch/master)

## Project Description ##

Write maintainable unit tests, faster.

AutoFixture makes it easier for developers to do Test-Driven Development by automating non-relevant Test Fixture Setup, allowing the Test Developer to focus on the essentials of each test case.

> ["saved us quite some time"](#who-uses-autofixture)
> 
> -[Florian Hötzinger, GAB Enterprise IT Solutions GmbH](#who-uses-autofixture) 

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

This example illustrates the basic principle of AutoFixture: It can create values of virtually any type without the need for you to explicitly define which values should be used. The number *expectedNumber* is created by a call to Create<T> - this will create a 'nice', regular integer value, saving you the effort of explicitly coming up with one.

The example also illustrates how AutoFixture can be used as a [SUT Factory](http://blog.ploeh.dk/2009/02/13/SUTFactory.aspx) that creates the actual System Under Test (the MyClass instance).

Given the right combination of unit testing framework and extensions for AutoFixture, we can further reduce the above test to be even more declarative: 

[xUnit](http://blog.ploeh.dk/2010/10/08/AutoDataTheoriesWithAutoFixture.aspx) 
```csharp
[Theory, AutoData]
public void IntroductoryTest(
    int expectedNumber, MyClass sut)
{
    int result = sut.Echo(expectedNumber);
    Assert.Equal(expectedNumber, result);
}
```
and 

[NUnit](http://gertjvr.wordpress.com/2013/09/25/howto-autofixture-nunit2)
```csharp
[Test, AutoData]
public void IntroductoryTest(
    int expectedNumber, MyClass sut)
{
    int result = sut.Echo(expectedNumber);
    Assert.Equal(expectedNumber, result);
}
```

Notice how we can reduce unit tests to state only the relevant parts of the test. The rest (variables, Fixture object) is relegated to attributes and parameter values that are supplied automatically by AutoFixture. The test is now only two lines of code.

Using AutoFixture is as easy as referencing the library and creating a new instance of the Fixture class!

## Downloads

AutoFixture is available via NuGet:

* [AutoFixture](http://nuget.org/packages/AutoFixture)
* [AutoFixture.AutoMoq](http://nuget.org/packages/AutoFixture.AutoMoq)
* [AutoFixture.AutoRhinoMocks](http://nuget.org/packages/AutoFixture.AutoRhinoMocks)
* [AutoFixture.AutoFakeItEasy](http://nuget.org/packages/AutoFixture.AutoFakeItEasy)
* [AutoFixture.AutoNSubstitute](http://nuget.org/packages/AutoFixture.AutoNSubstitute)
* [AutoFixture.AutoFoq](http://www.nuget.org/packages/AutoFixture.AutoFoq)
* [AutoFixture.Xunit](http://nuget.org/packages/AutoFixture.Xunit)
* [AutoFixture.Xunit2](http://nuget.org/packages/AutoFixture.Xunit2)
* [AutoFixture.NUnit2](http://nuget.org/packages/AutoFixture.NUnit2)
* [AutoFixture.NUnit3](http://nuget.org/packages/AutoFixture.NUnit3)
* [AutoFixture.Idioms](http://nuget.org/packages/AutoFixture.Idioms)
* [SemanticComparison](http://nuget.org/packages/SemanticComparison)

## Versioning

AutoFixture follows [Semantic Versioning 2.0.0](http://semver.org/spec/v2.0.0.html).

## Documentation ##

* [CheatSheet](https://github.com/AutoFixture/AutoFixture/wiki/Cheat-Sheet)
* [FAQ](https://github.com/AutoFixture/AutoFixture/wiki/FAQ)

## Questions ##

If you have questions, feel free to ask. The best places to ask are:

* [Stack Overflow - use the *autofixture* tag](http://stackoverflow.com/questions/tagged/autofixture)
* [GitHub issues](http://github.com/AutoFixture/AutoFixture/issues)

## Who uses AutoFixture? ##

AutoFixture is used around the world, as the following quotes testify:

> "I’ve introduced AutoFixture to my developers (at www.gab.de ) some time ago. We’ve been using it successfully with xunit in multiple projects all across the .NET technology stack. We also use it for feeding dummy data to the UI when developing prototypes. That saved us quite some time.
>
> -[Florian Hötzinger](https://twitter.com/hoetz), [GAB Enterprise IT Solutions GmbH](http://www.gab.de)


> "I have used AutoFixture for 3 years, it's a vital tool in my TDD toolbox, a real time-saver. Setting up maintainable and robust unit tests with AutoFixture is easy and straightforward - highly recommendable"
>
> -[Mads Tjørnelund Toustrup](http://madstt.dk), Senior .Net Developer, [d60 a/s](http://d60.dk)


> "Autofixture is more than just another test data generator. It helps me to write tests faster, which are robust against changes in my production code. Moreover, with Autofixture I can focus the tests on the behavior I want to check which why they are easier to read and understand."
>
> -[Hendrik Lösch](http://www.just-about.net), [Saxonia Systems AG](http://www.saxsys.de)

If you want to add your own testimonial to this list, we (the AutoFixture maintainers) would be very grateful. Send us a pull request to this README.md file.

## Additional resources ##

* [Pluralsight course](http://www.shareasale.com/r.cfm?u=1017843&b=611266&m=53701&afftrack=&urllink=www%2Epluralsight%2Ecom%2Fcourses%2Fautofixture%2Ddotnet%2Dunit%2Dtest%2Dget%2Dstarted)
* [ploeh blog](http://blog.ploeh.dk/tags/#AutoFixture-ref)
* [Nikos Baxevanis' blog](http://nikosbaxevanis.com/categories/autofixture)
* [Enrico Campidoglio's blog](http://megakemp.com/tag/autofixture)
* [Gert Jansen van Rensburg's blog](http://gertjvr.wordpress.com/category/autofixture)
* [Questions on Stack Overflow](http://stackoverflow.com/questions/tagged/autofixture)

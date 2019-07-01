[![Build status](https://ci.appveyor.com/api/projects/status/qlmobf6rt05pmt7e/branch/master?svg=true)](https://ci.appveyor.com/project/AutoFixture/autofixture/branch/master) [![NuGet version](https://img.shields.io/nuget/vpre/AutoFixture.svg)](https://www.nuget.org/packages/AutoFixture) <a href="https://twitter.com/AutoFixture"><img src="https://img.shields.io/twitter/follow/AutoFixture.svg?style=social&label=@AutoFixture" alt="AutoFixture" align="right" /></a>

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

```c#
[Fact]
public void IntroductoryTest()
{
    // Arrange
    Fixture fixture = new Fixture();

    int expectedNumber = fixture.Create<int>();
    MyClass sut = fixture.Create<MyClass>();
    // Act
    int result = sut.Echo(expectedNumber);
    // Assert
    Assert.Equal(expectedNumber, result);
}
```

This example illustrates the basic principle of AutoFixture: It can create values of virtually any type without the need for you to explicitly define which values should be used. The number *expectedNumber* is created by a call to Create<T> - this will create a 'nice', regular integer value, saving you the effort of explicitly coming up with one.

The example also illustrates how AutoFixture can be used as a [SUT Factory](http://blog.ploeh.dk/2009/02/13/SUTFactory.aspx) that creates the actual System Under Test (the MyClass instance).

Given the right combination of unit testing framework and extensions for AutoFixture, we can further reduce the above test to be even more declarative: 

[xUnit](http://blog.ploeh.dk/2010/10/08/AutoDataTheoriesWithAutoFixture.aspx) 
```c#
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
```c#
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

## .NET platforms compatibility table

| Product                    | .NET Framework            | .NET Standard               |
| -------------------------- | ------------------------  | ------------------------    |
| AutoFixture                | :heavy_check_mark: 4.5.2  | :heavy_check_mark: 1.5, 2.0 |
| AutoFixture.SeedExtensions | :heavy_check_mark: 4.5.2  | :heavy_check_mark: 1.5, 2.0 |
| AutoFixture.xUnit          | :heavy_check_mark: 4.5.2  | :heavy_minus_sign:          |
| AutoFixture.xUnit2         | :heavy_check_mark: 4.5.2  | :heavy_check_mark: 1.5, 2.0 |
| AutoFixture.NUnit2         | :heavy_check_mark: 4.5.2  | :heavy_minus_sign:          |
| AutoFixture.NUnit3         | :heavy_check_mark: 4.5.2  | :heavy_check_mark: 1.5, 2.0 |
| AutoFakeItEasy             | :heavy_check_mark: 4.5.2  | :heavy_check_mark: 1.6, 2.0 |
| AutoFoq                    | :heavy_check_mark: 4.5.2  | :heavy_minus_sign:          |
| AutoMoq                    | :heavy_check_mark: 4.5.2  | :heavy_check_mark: 1.5, 2.0 |
| AutoNSubstitute            | :heavy_check_mark: 4.5.2  | :heavy_check_mark: 1.5, 2.0 |
| AutoRhinoMock              | :heavy_check_mark: 4.5.2  | :heavy_minus_sign:          |
| Idioms                     | :heavy_check_mark: 4.5.2  | :heavy_check_mark: 2.0      |
| Idioms.FsCheck             | :heavy_check_mark: 4.5.2  | :heavy_check_mark: 2.0      |

## Downloads

AutoFixture is available via NuGet:

* [AutoFixture](http://nuget.org/packages/AutoFixture)
* [AutoFixture.SeedExtensions](http://nuget.org/packages/AutoFixture.SeedExtensions)
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

### vNext feed
The artifacts of the next major version are published to [the MyGet feed](https://www.myget.org/gallery/autofixture):
- `https://www.myget.org/F/autofixture/api/v3/index.json` (Visual Studio 2015+)
- `https://www.myget.org/F/autofixture/api/v2` (Visual Studio 2012+)

You can use this feed to early access and test the next major version of the AutoFixture.

__Notice__, this feed exists for the _preview purpose_ only, so use it with caution:
- new versions of packages might contain breaking changes and API could change drastically from package to package. By other words, we don't follow the SemVer policy for the packages in this feed;
- packages might be cleaned up over time (MyGet has storage limits), so don't consider this feed for the permanent usage (or at least ensure to make a copy of the used packages somewhere else).

## Versioning

AutoFixture follows [Semantic Versioning 2.0.0](http://semver.org/spec/v2.0.0.html) for the public releases (published to the [nuget.org](https://www.nuget.org/)).

## Build

AutoFixture uses [FAKE](http://fsharp.github.io/FAKE/) as a build engine. If you would like to build the AutoFixture locally, run the `Build.cmd` file and wait for the result.

The repository state (the last tag name and number of commits since the last tag) is used to determine the build version. If you would like to override the auto-generated AutoFixture version, pass the `BuildVersion` parameter to the `Build.cmd` file. For example:
```
Build.cmd BuildVersion=3.52.0
```

Refer to the [Build.fsx](Build.fsx) file to get information about all the supported build keys.

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

* [Pluralsight course](https://www.pluralsight.com/courses/autofixture-dotnet-unit-test-get-started)
* [ploeh blog](http://blog.ploeh.dk/tags/#AutoFixture-ref)
* [Nikos Baxevanis' blog](http://blog.nikosbaxevanis.com/tags/#autofixture)
* [Enrico Campidoglio's blog](http://megakemp.com/tag/autofixture)
* [Gert Jansen van Rensburg's blog](http://gertjvr.wordpress.com/category/autofixture)
* [Questions on Stack Overflow](http://stackoverflow.com/questions/tagged/autofixture)

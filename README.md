# AutoFixture

[![License](https://img.shields.io/badge/license-MIT-green)](https://raw.githubusercontent.com/AutoFixture/AutoFixture/master/LICENCE.txt)
[![Build status](https://ci.appveyor.com/api/projects/status/qlmobf6rt05pmt7e/branch/master?svg=true)](https://ci.appveyor.com/project/AutoFixture/autofixture/branch/master)
[![NuGet version](https://img.shields.io/nuget/vpre/AutoFixture.svg)](https://www.nuget.org/packages/AutoFixture)
[![MyGet (with prereleases)](https://img.shields.io/myget/autofixture/vpre/autofixture?label=myget)](https://www.myget.org/gallery/autofixture)
<a href="https://twitter.com/AutoFixture">
    <img src="https://img.shields.io/twitter/follow/AutoFixture?label=%40AutoFixture" alt="AutoFixture" align="right" />
</a>

Write maintainable unit tests, faster.

AutoFixture makes it easier for developers to do Test-Driven Development by automating non-relevant Test Fixture Setup, allowing the Test Developer to focus on the essentials of each test case.

Check the [testimonials](https://github.com/AutoFixture/AutoFixture/wiki/Who-uses-AutoFixture) to see what other people have to say about AutoFixture.

## Table of Contents

- [Overview](#overview)
- [Downloads](#downloads)
- [Documentation](#documentation)
- [Feedback & Questions](#feedback--questions)
- [License](#license)

## Overview

(Jump straight to the [CheatSheet](https://github.com/AutoFixture/AutoFixture/wiki/Cheat-Sheet) if you just want to see some code samples right away.)

AutoFixture is designed to make Test-Driven Development more productive and unit tests more refactoring-safe. It does so by removing the need for hand-coding anonymous variables as part of a test's Fixture Setup phase. Among other features, it offers a generic implementation of the [Test Data Builder](http://www.natpryce.com/articles/000714.html) pattern.

When writing unit tests, you typically need to create some objects that represent the initial state of the test. Often, an API will force you to specify much more data than you really care about, so you frequently end up creating objects that has no influence on the test, simply to make the code compile.

AutoFixture can help by creating such [Anonymous Variables](https://docs.microsoft.com/en-us/archive/blogs/ploeh/anonymous-variables) for you. Here's a simple example:

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

This example illustrates the basic principle of AutoFixture: It can create values of virtually any type without the need for you to explicitly define which values should be used. The number *expectedNumber* is created by a call to `Create<T>` - this will create a 'nice', regular integer value, saving you the effort of explicitly coming up with one.

The example also illustrates how AutoFixture can be used as a [SUT Factory](http://blog.ploeh.dk/2009/02/13/SUTFactory.aspx) that creates the actual System Under Test (the MyClass instance).

Given the right combination of unit testing framework and extensions for AutoFixture, we can further reduce the above test to be even more declarative:

### [xUnit](http://blog.ploeh.dk/2010/10/08/AutoDataTheoriesWithAutoFixture.aspx)

```c#
[Theory, AutoData]
public void IntroductoryTest(int expectedNumber, MyClass sut)
{
    int result = sut.Echo(expectedNumber);
    Assert.Equal(expectedNumber, result);
}
```

### [NUnit](http://gertjvr.wordpress.com/2013/09/25/howto-autofixture-nunit2)

```c#
[Test, AutoData]
public void IntroductoryTest(int expectedNumber, MyClass sut)
{
    int result = sut.Echo(expectedNumber);
    Assert.Equal(expectedNumber, result);
}
```

Notice how we can reduce unit tests to state only the relevant parts of the test. The rest (variables, Fixture object) is relegated to attributes and parameter values that are supplied automatically by AutoFixture. The test is now only two lines of code.

Using AutoFixture is as easy as referencing the library and creating a new instance of the Fixture class!

## Downloads

AutoFixture packages are distributed via NuGet.<br />
To install the packages you can use the integrated package manager of your IDE, the .NET CLI, or reference the package directly in your project file.

```cmd
dotnet add package AutoFixture --version 4.18.0
```

```xml
<PackageReference Include="AutoFixture" Version="4.18.0" />
```

AutoFixture offers a variety of utility packages and integrations with most of the major mocking libraries and testing frameworks.

### Core packages

The core packages offer the full set of AutoFixture's features without requring any testing framework or third party integration.

| Product | Package | Latest stable | Latest preview |
|---------|---------|---------------|----------------|
| The core package | [AutoFixture](http://nuget.org/packages/AutoFixture) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.svg)](https://www.nuget.org/packages/AutoFixture) | [![MyGet](https://img.shields.io/myget/autofixture/vpre/autofixture?label=myget)](https://www.myget.org/feed/autofixture/package/nuget/AutoFixture)|
| Assertion idioms | [AutoFixture.Idioms](http://nuget.org/packages/AutoFixture.Idioms) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.Idioms.svg)](https://www.nuget.org/packages/AutoFixture.Idioms) | [![MyGet](https://img.shields.io/myget/autofixture/vpre/AutoFixture.Idioms?label=myget)](https://www.myget.org/feed/autofixture/package/nuget/AutoFixture.Idioms)|
| Seed extensions  | [AutoFixture.SeedExtensions](http://nuget.org/packages/AutoFixture.SeedExtensions) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.SeedExtensions.svg)](https://www.nuget.org/packages/AutoFixture.SeedExtensions) | [![MyGet](https://img.shields.io/myget/autofixture/vpre/AutoFixture.SeedExtensions?label=myget)](https://www.myget.org/feed/autofixture/package/nuget/AutoFixture.SeedExtensions)|

### Mocking libraries

AutoFixture offers integations with most major .NET mocking libraries.<br/>
These integrations enable such features as configuring mocks, auto-injecting mocks, etc.

| Product | Package | Latest stable | Latest preview |
|---------|---------|---------------|----------------|
| Moq | [AutoFixture.AutoMoq](http://nuget.org/packages/AutoFixture.AutoMoq) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.AutoMoq.svg)](https://www.nuget.org/packages/AutoFixture.AutoMoq) | [![MyGet](https://img.shields.io/myget/autofixture/vpre/AutoFixture.AutoMoq?label=myget)](https://www.myget.org/feed/autofixture/package/nuget/AutoFixture.AutoMoq)|
| NSubstitute | [AutoFixture.AutoNSubstitute](http://nuget.org/packages/AutoFixture.AutoNSubstitute) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.AutoNSubstitute.svg)](https://www.nuget.org/packages/AutoFixture.AutoNSubstitute) | [![MyGet](https://img.shields.io/myget/autofixture/vpre/AutoFixture.AutoNSubstitute?label=myget)](https://www.myget.org/feed/autofixture/package/nuget/AutoFixture.AutoNSubstitute)|
| FakeItEasy | [AutoFixture.AutoFakeItEasy](http://nuget.org/packages/AutoFixture.AutoFakeItEasy) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.AutoFakeItEasy.svg)](https://www.nuget.org/packages/AutoFixture.AutoFakeItEasy) | [![MyGet](https://img.shields.io/myget/autofixture/vpre/AutoFixture.AutoFakeItEasy?label=myget)](https://www.myget.org/feed/autofixture/package/nuget/AutoFixture.AutoFakeItEasy)|
| Rhino Mocks | [AutoFixture.AutoRhinoMocks](http://nuget.org/packages/AutoFixture.AutoRhinoMocks) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.AutoRhinoMocks.svg)](https://www.nuget.org/packages/AutoFixture.AutoRhinoMocks) | [![MyGet](https://img.shields.io/myget/autofixture/vpre/AutoFixture.AutoRhinoMocks?label=myget)](https://www.myget.org/feed/autofixture/package/nuget/AutoFixture.AutoRhinoMocks)|

> **NOTE:** 
> Since AutoFixture tries maintain compatibility with a large number of package versions, the packages bundled with AutoFixture might not contain the latest features of your mocking library.<br />
> Make sure to install the latest version of the mocking library package, alongside the AutoFixture package.

### Testing frameworks

AutoFixture offers integrations with most major .NET testing frameworks.<br />
These integrations enable auto-generation of test cases, combining auto-generated data with inline arguments, etc.

| Product  | Package | Latest stable | Latest preview |
|----------|---------|---------------|----------------|
| Foq      | [AutoFixture.AutoFoq](http://www.nuget.org/packages/AutoFixture.AutoFoq) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.AutoFoq.svg)](https://www.nuget.org/packages/AutoFixture.AutoFoq) | [![MyGet](https://img.shields.io/myget/autofixture/vpre/AutoFixture.AutoFoq?label=myget)](https://www.myget.org/feed/autofixture/package/nuget/AutoFixture.AutoFoq) |
| xUnit v1 | [AutoFixture.Xunit](http://nuget.org/packages/AutoFixture.Xunit) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.Xunit.svg)](https://www.nuget.org/packages/AutoFixture.Xunit) | [![MyGet](https://img.shields.io/myget/autofixture/vpre/AutoFixture.Xunit?label=myget)](https://www.myget.org/feed/autofixture/package/nuget/AutoFixture.Xunit) |
| xUnit v2 | [AutoFixture.Xunit2](http://nuget.org/packages/AutoFixture.Xunit2) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.Xunit2.svg)](https://www.nuget.org/packages/AutoFixture.Xunit2) | [![MyGet](https://img.shields.io/myget/autofixture/vpre/AutoFixture.Xunit2?label=myget)](https://www.myget.org/feed/autofixture/package/nuget/AutoFixture.Xunit2) |
| NUnit v2 | [AutoFixture.NUnit2](http://nuget.org/packages/AutoFixture.NUnit2) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.NUnit2.svg)](https://www.nuget.org/packages/AutoFixture.NUnit2) | [![MyGet](https://img.shields.io/myget/autofixture/vpre/AutoFixture.NUnit2?label=myget)](https://www.myget.org/feed/autofixture/package/nuget/AutoFixture.NUnit2) |
| NUnit v3 | [AutoFixture.NUnit3](http://nuget.org/packages/AutoFixture.NUnit3) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.NUnit3.svg)](https://www.nuget.org/packages/AutoFixture.NUnit3) | [![MyGet](https://img.shields.io/myget/autofixture/vpre/AutoFixture.NUnit3?label=myget)](https://www.myget.org/feed/autofixture/package/nuget/AutoFixture.NUnit3) |

You can check the compatibility with your target framework version on the [wiki](https://github.com/AutoFixture/AutoFixture/wiki#net-platforms-compatibility-table) or on the [NuGet](https://www.nuget.org/profiles/AutoFixture) website.

### vNext feed

The artifacts of the next major version are published to [the MyGet feed](https://www.myget.org/gallery/autofixture):

* `https://www.myget.org/F/autofixture/api/v3/index.json` (Visual Studio 2015+)
* `https://www.myget.org/F/autofixture/api/v2` (Visual Studio 2012+)

You can use this feed to early access and test the next major version of the AutoFixture.

__Notice__, this feed exists for the _preview purpose_ only, so use it with caution:

* new versions of packages might contain breaking changes and API could change drastically from package to package. By other words, we don't follow the SemVer policy for the packages in this feed;
* packages might be cleaned up over time (MyGet has storage limits), so don't consider this feed for the permanent usage (or at least ensure to make a copy of the used packages somewhere else).

## Documentation

* [CheatSheet](https://github.com/AutoFixture/AutoFixture/wiki/Cheat-Sheet)
* [FAQ](https://github.com/AutoFixture/AutoFixture/wiki/FAQ)

### Additional resources

* [Pluralsight course](https://www.pluralsight.com/courses/unit-testing-autofixture-dot-net)
* [ploeh blog](http://blog.ploeh.dk/tags/#AutoFixture-ref)
* [Nikos Baxevanis' blog](http://blog.nikosbaxevanis.com)
* [Enrico Campidoglio's blog](http://megakemp.com/tag/autofixture)
* [Gert Jansen van Rensburg's blog](http://gertjvr.wordpress.com/category/autofixture)
* [Questions on Stack Overflow](http://stackoverflow.com/questions/tagged/autofixture)

## Feedback & Questions

If you have questions, feel free to ask. The best places to ask are:

* [Stack Overflow - use the *autofixture* tag](http://stackoverflow.com/questions/tagged/autofixture)
* [GitHub Q&A Discussions](https://github.com/AutoFixture/AutoFixture/discussions/categories/q-a)

## License

AutoFixture is Open Source software and is released under the [MIT license](https://raw.githubusercontent.com/AutoFixture/AutoFixture/master/LICENCE.txt).<br />
The licenses allows the use of AutoFixture libraries in free and commercial applications and libraries without restrictions.

### .NET Foundation

This project is supported by the [.NET Foundation](https://dotnetfoundation.org).

# AutoFixture

[![License](https://img.shields.io/badge/license-MIT-green)](https://raw.githubusercontent.com/AutoFixture/AutoFixture/master/LICENCE.txt)
[![release](https://github.com/AutoFixture/AutoFixture/actions/workflows/release.yml/badge.svg)](https://github.com/AutoFixture/AutoFixture/actions/workflows/release.yml)
[![NuGet version](https://img.shields.io/nuget/v/AutoFixture?logo=nuget)](https://www.nuget.org/packages/AutoFixture)
[![NuGet preview version](https://img.shields.io/nuget/vpre/AutoFixture?logo=nuget)](https://www.nuget.org/packages/AutoFixture)
[![NuGet downloads](https://img.shields.io/nuget/dt/AutoFixture)](https://www.nuget.org/packages/AutoFixture)
<a href="https://x.com/AutoFixture">
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

| Product | Package | Stable | Preview | Downloads |
|---------|---------|--------|---------|-----------|
| The core package | [AutoFixture](http://www.nuget.org/packages/AutoFixture) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture)](https://www.nuget.org/packages/AutoFixture) | [![NuGet](https://img.shields.io/nuget/vpre/autofixture)](https://www.nuget.org/packages/AutoFixture) | ![NuGet](https://img.shields.io/nuget/dt/autofixture) |
| Assertion idioms | [AutoFixture.Idioms](http://www.nuget.org/packages/AutoFixture.Idioms) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture.Idioms)](https://www.nuget.org/packages/AutoFixture.Idioms) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.Idioms)](https://www.nuget.org/packages/AutoFixture.Idioms) | ![NuGet](https://img.shields.io/nuget/dt/autofixture.idioms) |
| Seed extensions | [AutoFixture.SeedExtensions](http://www.nuget.org/packages/AutoFixture.SeedExtensions) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture.SeedExtensions)](https://www.nuget.org/packages/AutoFixture.SeedExtensions) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.SeedExtensions)](https://www.nuget.org/packages/AutoFixture.SeedExtensions) | ![NuGet](https://img.shields.io/nuget/dt/autofixture.seedextensions) |

### Mocking libraries

AutoFixture offers integations with most major .NET mocking libraries.<br/>
These integrations enable such features as configuring mocks, auto-injecting mocks, etc.

| Product | Package | Stable | Preview | Downloads |
|---------|---------|--------|---------|-----------|
| Moq | [AutoFixture.AutoMoq](http://www.nuget.org/packages/AutoFixture.AutoMoq) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture.AutoMoq)](https://www.nuget.org/packages/AutoFixture.AutoMoq) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.AutoMoq)](https://www.nuget.org/packages/AutoFixture.AutoMoq) | ![NuGet](https://img.shields.io/nuget/dt/autofixture.automoq) |
| NSubstitute | [AutoFixture.AutoNSubstitute](http://www.nuget.org/packages/AutoFixture.AutoNSubstitute) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture.AutoNSubstitute)](https://www.nuget.org/packages/AutoFixture.AutoNSubstitute) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.AutoNSubstitute)](https://www.nuget.org/packages/AutoFixture.AutoNSubstitute) | ![NuGet](https://img.shields.io/nuget/dt/autofixture.AutoNSubstitute) |
| FakeItEasy | [AutoFixture.AutoFakeItEasy](http://www.nuget.org/packages/AutoFixture.AutoFakeItEasy) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture.AutoFakeItEasy)](https://www.nuget.org/packages/AutoFixture.AutoFakeItEasy) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.AutoFakeItEasy)](https://www.nuget.org/packages/AutoFixture.AutoFakeItEasy) | ![NuGet](https://img.shields.io/nuget/dt/autofixture.AutoFakeItEasy) |
| Rhino Mocks | [AutoFixture.AutoRhinoMocks](http://www.nuget.org/packages/AutoFixture.AutoRhinoMocks) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture.AutoRhinoMocks)](https://www.nuget.org/packages/AutoFixture.AutoRhinoMocks) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.AutoRhinoMocks)](https://www.nuget.org/packages/AutoFixture.AutoRhinoMocks) | ![NuGet](https://img.shields.io/nuget/dt/autofixture.AutoRhinoMocks) |

> **NOTE:** 
> Since AutoFixture tries maintain compatibility with a large number of package versions, the packages bundled with AutoFixture might not contain the latest features of your mocking library.<br />
> Make sure to install the latest version of the mocking library package, alongside the AutoFixture package.

### Testing frameworks

AutoFixture offers integrations with most major .NET testing frameworks.<br />
These integrations enable auto-generation of test cases, combining auto-generated data with inline arguments, etc.

| Product | Package | Stable | Preview | Downloads |
|---------|---------|--------|---------|-----------|
| xUnit v3 | [AutoFixture.Xunit3](http://www.nuget.org/packages/AutoFixture.Xunit3) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture.Xunit3)](https://www.nuget.org/packages/AutoFixture.Xunit3) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.Xunit3)](https://www.nuget.org/packages/AutoFixture.Xunit3) | ![NuGet](https://img.shields.io/nuget/dt/autofixture.xUnit3) |
| xUnit v2 | [AutoFixture.Xunit2](http://www.nuget.org/packages/AutoFixture.Xunit2) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture.Xunit2)](https://www.nuget.org/packages/AutoFixture.Xunit2) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.Xunit2)](https://www.nuget.org/packages/AutoFixture.Xunit2) | ![NuGet](https://img.shields.io/nuget/dt/autofixture.xUnit2) |
| xUnit v1 | [AutoFixture.Xunit](http://www.nuget.org/packages/AutoFixture.Xunit) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture.Xunit)](https://www.nuget.org/packages/AutoFixture.Xunit) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.Xunit)](https://www.nuget.org/packages/AutoFixture.Xunit) | ![NuGet](https://img.shields.io/nuget/dt/autofixture.XUnit) |
| NUnit v4 | [AutoFixture.NUnit4](http://www.nuget.org/packages/AutoFixture.NUnit4) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture.NUnit4)](https://www.nuget.org/packages/AutoFixture.NUnit4) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.NUnit4)](https://www.nuget.org/packages/AutoFixture.NUnit4) | ![NuGet](https://img.shields.io/nuget/dt/autofixture.NUnit4) |
| NUnit v3 | [AutoFixture.NUnit3](http://www.nuget.org/packages/AutoFixture.NUnit3) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture.NUnit3)](https://www.nuget.org/packages/AutoFixture.NUnit3) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.NUnit3)](https://www.nuget.org/packages/AutoFixture.NUnit3) | ![NuGet](https://img.shields.io/nuget/dt/autofixture.NUnit3) |
| NUnit v2 | [AutoFixture.NUnit2](http://www.nuget.org/packages/AutoFixture.NUnit2) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture.NUnit2)](https://www.nuget.org/packages/AutoFixture.NUnit2) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.NUnit2)](https://www.nuget.org/packages/AutoFixture.NUnit2) | ![NuGet](https://img.shields.io/nuget/dt/autofixture.NUnit2) |
| Foq | [AutoFixture.AutoFoq](http://www.nuget.org/packages/AutoFixture.AutoFoq) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture.AutoFoq)](https://www.nuget.org/packages/AutoFixture.AutoFoq) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.AutoFoq)](https://www.nuget.org/packages/AutoFixture.AutoFoq) | ![NuGet](https://img.shields.io/nuget/dt/autofixture.AutoFoq) |

You can check the compatibility with your target framework version on the [wiki](https://github.com/AutoFixture/AutoFixture/wiki#net-platforms-compatibility-table) or on the [NuGet](https://www.nuget.org/profiles/AutoFixture) website.

### vNext feed

The artifacts of the next major version are published to [nuget.org](https://www.nuget.org), and are marked with the `preview` suffix (e.g. `5.0.0-preview00007`).</br>
You can use these packages to early access and test the next major version of the AutoFixture.</br>
Make sure to enable the preview packages in your IDE in order to see the latest version.

> __NOTE:__ This preview versions exists for the _preview purpose_ only, so use them with caution:
>
>* New versions of packages might contain breaking changes and API could change drastically from package to package. By other words, we don't follow the SemVer policy for the packages in this feed;
>* Preview packages might be unlisted over time, in order to not clutter the version suggestion dialog in IDEs, but will generally remain available

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

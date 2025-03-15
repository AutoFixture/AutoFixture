# How to contribute to AutoFixture

AutoFixture is currently being developed in C# 10 and F# 6 on .NET Framework V4.6.2, .NET Standard 2.0, and .NET 6 using Visual Studio 2022 or later, with [xUnit.net](http://xunit.net) as the unit testing framework.
So far, all development has been done with TDD, so there's a pretty high degree of code coverage, and the aim is to keep it that way.

## Code of Conduct

Before continuing, make sure that you are familiar with the [Code of Conduct document](https://github.com/AutoFixture/AutoFixture/blob/master/CODE_OF_CONDUCT.md), as it applies to all interactions within the project.

## Issues

Issues in GitHub have historically been a very overloaded term that, depending on the repository, can mean different things, from bugs to feature ideas to be implemented some time in the future, so it's common for people carry assumptions from other repositories they interacted with.
However, this project reserves the use of [Issues](https://github.com/AutoFixture/AutoFixture/issues) only to bugs, tasks, and vulnerability reports, that have been **confirmed** by a maintainer. All other discussions (e.g. feature ideas, questions, etc.) are reserved to the [Discussions](https://github.com/AutoFixture/AutoFixture/discussions) section.
Follow the suggestions in the **New Issue** menu, to ensure you have picked the correct option.

When opening a new issue, follow the instructions included with the template. <ins>**DO NOT** alter the template structure</ins>. Issues not following the template might be ignored and even closed.

## Build

AutoFixture uses [NUKE](https://nuke.build/) as a build engine. If you would like to build the AutoFixture locally, run the `build.cmd` file and wait for the result.

The repository state (the last tag name and number of commits since the last tag) is used to determine the build version. This process is automated using [GitVersion](https://gitversion.net/)

Refer to the output of `build.cmd --help` to get information about all the supported build paramters.

## Verification

As part of the verification build, Code Analysis is executed in a configuration that treats warnings as errors. For unit test projects code most of the rules are suppressed so only missing warnings are expected. No CA warnings should be suppressed unless the documented conditions for suppression are satisfied. Otherwise, a documented agreement between at least two active developers of the project should be reached to allow a suppression of a non-suppressible warning.

The final step of the CI pipeline will be running full solution test coverage. While there are no strict rules on the test coverage ensure that all new flows have been properly tested and documented.

To ensure all rules are respected, before publishing the PR, run the command below from the root directory of the repository.
This will verify that all required code practices have been followed, run all tests in the solution, and will generate a HTML coverage report (located in `./artifacts/reports/`) that you can inspect to ensure you've covered all updated flows.

```sh
build.cmd Verify Cover --no-logo --configuration Release
```

## Pull requests

When developing for AutoFixture, please respect the coding style already present. Look around in the source code to get a feel for it.

Please keep line lengths under 120 characters. Line lengths over 120 characters don't fit into the standard GitHub code listing window, so it requires vertical scrolling to review.

Also, please follow the [Open Source Contribution Etiquette](http://tirania.org/blog/archive/2010/Dec-31.html). AutoFixture is a fairly typical open source project: if you want to contribute, start by [creating a fork](https://help.github.com/articles/fork-a-repo/) and [sending a pull request](https://help.github.com/articles/about-pull-requests/) when you have something you wish to commit. When creating pull requests, please keep the Single Responsibility Principle in mind. A pull request that does a single thing very well is more likely to be accepted. See also the article [The Rules of the Open Road](https://web.archive.org/web/20170407192632/http://blog.half-ogre.com/posts/software/rules-of-the-open-road) for more good tips on working with OSS and Pull Requests, as well as these [10 tips for better Pull Requests](http://blog.ploeh.dk/2015/01/15/10-tips-for-better-pull-requests).

For complex pull requests, you are encouraged to first start a discussion in the [Discussions section](https://github.com/AutoFixture/AutoFixture/discussions). This can save you time, because the AutoFixture regulars can help verify your idea, or point you in the right direction.

Some existing issues are marked with [the `good first issue` tag](https://help.github.com/articles/finding-open-source-projects-on-github/#searching-using-labels). These are good candidates to attempt, if you are just getting started with AutoFixture.

When you submit a pull request, you can expect a response within a day or two. We (the maintainers of AutoFixture) have day jobs, so we may not be able to immediately review your pull request, but we do make it a priority. Also keep in mind that we may not be in your time zone.

Most likely, when we review pull requests, we will make suggestions for improvement. This is normal, so don't interpret it as though we don't like your pull request. On the contrary, if we agree on the overall goal of the pull request, we want to work *with* you to make it a success.

## Versioning

AutoFixture follows [Semantic Versioning 2.0.0](http://semver.org/spec/v2.0.0.html) for the public releases (published to the [nuget.org](https://www.nuget.org/)).

## Continuous Integration

AutoFixture has been set up for Continuous Integration. The build is in [GitHub Actions](https://github.com/AutoFixture/AutoFixture/actions) and runs automatically every time a new commit is pushed to any of the [public branches](https://github.com/AutoFixture/AutoFixture/branches) or a Pull Request is submitted. AutoFixture uses GitHub's [Commit Status API](https://github.com/blog/1227-commit-status-api#pull-requests) to prevent Pull Requests that don't pass the build from being accidentally merged.

The build process is implemented in the `_build` project using [NUKE](http://nuke.build/) and consists of four main steps:

1. Compile all projects
2. Run static analysis on all projects using [FxCop](https://en.wikipedia.org/wiki/FxCop) and [Roslyn Analyzers package](https://www.nuget.org/packages/Microsoft.CodeAnalysis.FxCopAnalyzers).
3. Run [all tests](https://ci.appveyor.com/project/AutoFixture/autofixture/build/tests)
4. Create [NuGet packages](https://ci.appveyor.com/project/AutoFixture/autofixture/build/artifacts)

The NuGet packages produced by the latest build can be downloaded directly from [Releases](https://github.com/AutoFixture/AutoFixture/releases).

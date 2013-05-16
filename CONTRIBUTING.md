# How to contribute to AutoFixture #

AutoFixture is currently being developed in C# on .NET 4 using Visual Studio 2010 or 2012 with [xUnit.net](http://xunit.codeplex.com/) as the unit testing framework. So far, all development has been done with TDD, so there's a pretty high degree of code coverage, and it would be preferable to keep it that way.

## Dependencies ##

All binaries (such as xUnit.net) are included as binaries in the source control repository under the \Lib folder. All additional binaries not part of .NET 4 must go there as well, so that it would be possible to pull down the repository and immediately be able to compile and run all tests. The reason for this structure is mainly because it predates [NuGet](http://nuget.org/); we might move to NuGet packages in the future.

## Verification ##

There are several different targeted solutions to be found under the \Src folder, but be aware that the final verification step before pushing to the repository is to successfully run all the unit tests in the BuildRelease.ps1 file.

As part of the verification build, Visual Studio Code Analysis is executed in a configuration that treats warnings as errors. No CA warnings should be suppressed unless the documented conditions for suppression are satisfied. Otherwise, a documented agreement between at least two active developers of the project should be reached to allow a suppression of a non-suppressable warning.

## Pull requests ##

When developing for AutoFixture, please respect the coding style already present. Look around in the source code to get a feel for it. Also, please follow the [Open Source Contribution Etiquette](http://tirania.org/blog/archive/2010/Dec-31.html). AutoFixture is a fairly typical open source project: if you want to contribute, start by [creating a fork](http://help.github.com/fork-a-repo/) and [sending a pull request](http://help.github.com/send-pull-requests/) when you have something you wish to commit. When creating pull requests, please keep the Single Responsibility Principle in mind. A pull request that does a single thing very well is more likely to be accepted.

For complex pull requests, you are encouraged to first start a discussion on the [issue list](https://github.com/AutoFixture/AutoFixture/issues). This can save you time, because the AutoFixture regulars can help verify your idea, or point you in the right direction.

Some existing issues are marked with [the jump-in tag](http://nikcodes.com/2013/05/10/new-contributor-jump-in/). These are good candidates to attempt, if you are just getting started with AutoFixture.

When you submit a pull request, you can expect a response within a day or two. We (the maintainers of AutoFixture) have day jobs, so we may not be able to immediately review your pull request, but we do make it a priority. Also keep in mind that we may not be in your time zone.

Most likely, when we review pull requests, we will make suggestions for improvement. This is normal, so don't interpret it as though we don't like your pull request. On the contrary, if we agree on the overall goal of the pull request, we want to work *with* you to make it a success.

## Continuous Delivery ##

AutoFixture is set for Continuous Integration on [http://teamcity.codebetter.com/project.html?projectId=project129&tab=projectOverview](http://teamcity.codebetter.com/project.html?projectId=project129&tab=projectOverview). The latest build (including strong names) can be downloaded from there, but Continuous Delivery is also enabled, which means that the latest build of *master* is also automatically published on CodePlex and NuGet.

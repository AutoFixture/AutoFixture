#load "../.fake/build.fsx/intellisense.fsx"

open Fake.BuildServer
open Fake.Core

type AppVeyor.Environment with
    static member IsPullRequest = String.isNotNullOrEmpty AppVeyor.Environment.PullRequestNumber

let setVersion version parameters:AppVeyor.UpdateBuildParams =
    {parameters with Version = version}

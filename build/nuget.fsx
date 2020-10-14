#load "../.fake/build.fsx/intellisense.fsx"

open Fake.Core

type NuGetFeed = { ApiKey: string; Source: string }

let getMyGetFeed () =
  let envKey = Environment.environVarOrNone "MYGET_API_KEY"
  match envKey with
  | None    -> None
  | Some key  -> Some {
    ApiKey = key
    Source = "https://www.myget.org/F/autofixture/api/v2/package"}

let getNuGetFeed () =
  let envKey = Environment.environVarOrNone "NUGET_API_KEY"
  match envKey with
  | None      -> None
  | Some key  -> Some {
    ApiKey = key
    Source = "https://www.nuget.org/api/v2/package"}

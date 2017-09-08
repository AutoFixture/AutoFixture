[<AutoOpen>]
module Ploeh.AutoFixture.Idioms.FsCheckUnitTest.TestDsl

open FsCheck
open System

let internal verify = Swensen.Unquote.Assertions.test
let internal implements<'T> (sut : obj) = typeof<'T>.IsAssignableFrom(sut.GetType())

type Generators =
    static member Version() =
        Arb.generate<byte>
        |> Gen.map int
        |> Gen.four
        |> Gen.map (fun (ma, mi, bu, re) -> Version(ma, mi, bu, re))
        |> Arb.fromGen
module Ploeh.AutoFixture.AutoFoq.UnitTest.TestDsl

let internal verify = Swensen.Unquote.Assertions.test
let inline internal isNull (value : 'a) = match value with | null -> true | _ -> false
let internal implements<'T> (sut : obj) = typeof<'T>.IsAssignableFrom(sut.GetType())
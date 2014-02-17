module Ploeh.AutoFixture.AutoFoq.UnitTest.TestDsl

let internal verify = Swensen.Unquote.Assertions.test
let internal implements<'T> (sut : obj) = typeof<'T>.IsAssignableFrom(sut.GetType())
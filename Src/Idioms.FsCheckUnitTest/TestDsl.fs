module internal Ploeh.AutoFixture.Idioms.FsCheckUnitTest.TestDsl

let verify = Swensen.Unquote.Assertions.test
let implements<'T> (sut : obj) = typeof<'T>.IsAssignableFrom(sut.GetType())
let doesNotThrow = Xunit.Assert.DoesNotThrow
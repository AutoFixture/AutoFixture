namespace Ploeh.AutoFixture.Idioms.FsCheck

open System

type ReturnValueMustNotBeNullException (message) = inherit Exception (message)
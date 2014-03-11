namespace Ploeh.AutoFixture.Idioms.FsCheck

open Ploeh.AutoFixture.Idioms
open Ploeh.AutoFixture.Kernel
open System

type ReturnValueMustNotBeNullAssertion (builder : ISpecimenBuilder) = 
    inherit IdiomaticAssertion()
    do if builder = null then raise (ArgumentNullException("builder"))
    member this.Builder = builder
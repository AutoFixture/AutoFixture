namespace Ploeh.AutoFixture.Idioms.FsCheck

open Ploeh.AutoFixture.Idioms
open Ploeh.AutoFixture.Kernel

type ReturnValueMustNotBeNullAssertion (builder : ISpecimenBuilder) = 
    inherit IdiomaticAssertion()
    member this.Builder = builder
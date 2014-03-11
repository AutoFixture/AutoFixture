namespace Ploeh.AutoFixture.Idioms.FsCheck

open Ploeh.AutoFixture.Idioms
open Ploeh.AutoFixture.Kernel
open System
open System.Reflection

type ReturnValueMustNotBeNullAssertion (builder : ISpecimenBuilder) = 
    inherit IdiomaticAssertion()
    
    do if builder = null then raise <| ArgumentNullException("builder")
    
    member this.Builder = builder
    
    override this.Verify (property : PropertyInfo) =
        if property = null then raise <| ArgumentNullException("property")
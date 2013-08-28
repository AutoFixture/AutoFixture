namespace Ploeh.AutoFixture.AutoFoq

open Ploeh.AutoFixture
open Ploeh.AutoFixture.Kernel
open System

type AutoFoqCustomization =
    val Relay: ISpecimenBuilder
    new(relay: ISpecimenBuilder) = 
        { 
            Relay = ((if relay = null 
                      then raise(ArgumentNullException("relay"))); 
                      relay)
        }
    new() = 
        AutoFoqCustomization(
            FoqRelay(
                MethodInvoker(
                    FoqMethodQuery())))
    member this.Customize fixture = 
        (this :> ICustomization).Customize fixture
    interface ICustomization with
        member this.Customize fixture = 
            match fixture with
            | null -> raise (ArgumentNullException("fixture"))
            | _    -> fixture.ResidueCollectors.Add(this.Relay)

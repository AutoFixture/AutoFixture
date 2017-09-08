namespace Ploeh.AutoFixture.AutoFoq

open Ploeh.AutoFixture
open Ploeh.AutoFixture.Kernel
open System

/// <summary>
/// Enables auto-mocking with Foq.
/// </summary>
type AutoFoqCustomization(relay: ISpecimenBuilder) =
    do if relay = null then raise (ArgumentNullException("relay"))

    /// <summary>
    /// Initializes a new instance of the <see cref="AutoFoqCustomization"/> 
    /// class.
    /// </summary>
    new() = 
        AutoFoqCustomization(
            FilteringSpecimenBuilder(
                MethodInvoker(
                    FoqMethodQuery()),
                AbstractTypeSpecification()))

    /// <summary>
    /// Gets the relay that will be added to 
    /// <see cref="IFixture.ResidueCollectors"/> when <see cref="Customize"/>
    /// is invoked.
    /// </summary>
    member this.Relay = relay

    /// <summary>
    /// Customizes an <see cref="IFixture"/> to enable auto-mocking with Foq.
    /// </summary>
    /// <param name="fixture">The fixture upon which to enable auto-mocking.
    /// </param>
    member this.Customize fixture = 
        (this :> ICustomization).Customize fixture

    interface ICustomization with
        member this.Customize fixture = 
            match fixture with
            | null -> raise (ArgumentNullException("fixture"))
            | _    -> fixture.ResidueCollectors.Add(this.Relay)

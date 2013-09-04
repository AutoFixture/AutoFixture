namespace Ploeh.AutoFixture.AutoFoq

open Ploeh.AutoFixture
open Ploeh.AutoFixture.Kernel
open System

/// <summary>
/// Enables auto-mocking with Foq.
/// </summary>
type AutoFoqCustomization =
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoFoqCustomization"/> 
    /// class with the specified relay.
    /// </summary>
    /// <param name="relay">The relay.</param>
    new(relay: ISpecimenBuilder) = 
        { 
            Relay = ((if relay = null 
                      then raise(ArgumentNullException("relay"))); 
                      relay)
        }

    /// <summary>
    /// Initializes a new instance of the <see cref="AutoFoqCustomization"/> 
    /// class.
    /// </summary>
    new() = 
        AutoFoqCustomization(
            FoqRelay(
                MethodInvoker(
                    FoqMethodQuery())))

    /// <summary>
    /// Gets the relay that will be added to 
    /// <see cref="IFixture.ResidueCollectors"/> when <see cref="Customize"/>
    /// is invoked.
    /// </summary>
    val Relay: ISpecimenBuilder

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

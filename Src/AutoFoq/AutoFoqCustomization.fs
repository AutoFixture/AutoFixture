namespace Ploeh.AutoFixture.AutoFoq

open Ploeh.AutoFixture
open Ploeh.AutoFixture.Kernel
open System

/// <summary>
/// Enables auto-mocking with Foq.
/// </summary>
type AutoFoqCustomization() =
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
            | _    -> fixture.ResidueCollectors.Add(
                          FilteringSpecimenBuilder(
                              MethodInvoker(
                                  FoqMethodQuery(
                                      fixture)),
                              AbstractTypeSpecification()))

namespace Ploeh.AutoFixture.AutoFoq

open Ploeh.AutoFixture.Kernel
open System

/// <summary>
/// Relays a request for an interface or an abstract class to a request for a
/// <see cref="Foq.Mock&lt;T&gt;"/> of that class.
/// </summary>
type FoqRelay =
    /// <summary>
    /// Initializes a new instance of the <see cref="FoqRelay"/> class.
    /// </summary>
    new(builder: ISpecimenBuilder) = 
        FoqRelay(builder, AbstractTypeSpecification())

    /// <summary>
    /// Initializes a new instance of the <see cref="FoqRelay"/> class.
    /// </summary>
    /// <param name="builder">
    /// The builder which creates mock instances.
    /// </param>
    /// <param name="specification">
    /// A specification that determines whether a type should be relayed as a
    /// request for a mock of the same type.
    /// </param>
    new(builder: ISpecimenBuilder, specification: IRequestSpecification) = 
        { 
            Builder = 
                ((if builder = null 
                  then raise(ArgumentNullException("builder"))); 
                  builder)

            Specification = 
                ((if specification = null 
                  then raise(ArgumentNullException("specification"))); 
                  specification)
        }

    /// <summary>
    /// Gets the builder that creates mock instances.
    /// </summary>
    val Builder: ISpecimenBuilder

    /// <summary>
    /// Gets the specification that determines whether a given type should be 
    /// relayed as a request for a mock of the same type.
    /// </summary>
    /// <remarks>
    /// This specification determines whether a given type should be relayed as
    /// a request for a mock of the same type. By default it only returns 
    /// <see langword="true"/> for interfaces and abstract classes, but a 
    /// different specification can be supplied by using the overloaded 
    /// constructor that takes an <see cref="IRequestSpecification" /> as input.
    /// In that case, this member returns the specification supplied through 
    /// the constructor.
    /// </remarks>
    val Specification: IRequestSpecification
    
    /// <summary>
    /// Creates a new specimen based on a request.
    /// </summary>
    /// <param name="request">
    /// The request that describes what to create.
    /// </param>
    /// <param name="context">
    /// A context that can be used to create other specimens.
    /// </param>
    /// <returns>
    /// A dynamic mock instance of the requested interface or abstract class
    /// if possible; otherwise a <see cref="NoSpecimen"/> instance.
    /// </returns>
    member this.Create(request, context) = 
        (this :> ISpecimenBuilder).Create(request, context)
    
    interface ISpecimenBuilder with
        member this.Create(request, context) = 
            match this.Specification.IsSatisfiedBy(request) with
            | false -> NoSpecimen(request) :> obj
            | true  -> let specimen = this.Builder.Create(request, context) 
                       match specimen with
                       | null -> NoSpecimen(request) :> obj
                       | _    -> specimen

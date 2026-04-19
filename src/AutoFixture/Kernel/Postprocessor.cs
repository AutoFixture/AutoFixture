using System;
using System.Collections;
using System.Collections.Generic;

namespace AutoFixture.Kernel;

/// <summary>
/// Performs post-processing on a created specimen.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
    Justification = "The main responsibility of this class isn't to be a 'collection' (which, by the way, it isn't - it's just an Iterator).")]
public class Postprocessor : ISpecimenBuilderNode
{
    /// <summary>
    /// Gets the command, which is applied during postprocessing.
    /// </summary>
    /// <value>The command supplied via one of the constructors.</value>
    public ISpecimenCommand Command { get; }

    /// <summary>
    /// Gets the decorated builder.
    /// </summary>
    public ISpecimenBuilder Builder { get; }

    /// <summary>
    /// Gets the filter that determines whether <see cref="Command"/> should be executed.
    /// </summary>
    public IRequestSpecification Specification { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Postprocessor" />
    /// class.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="ISpecimenBuilder"/> to decorate.
    /// </param>
    /// <param name="command">
    /// The command to apply to the created specimen.
    /// </param>
    public Postprocessor(ISpecimenBuilder builder, ISpecimenCommand command)
        : this(builder, command, new TrueRequestSpecification())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Postprocessor" />
    /// class.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="ISpecimenBuilder"/> to decorate.
    /// </param>
    /// <param name="command">
    /// The command to apply to the created specimen.
    /// </param>
    /// A specification which is used to determine whether postprocessing
    /// should be performed
    /// <param name="specification">
    /// </param>
    public Postprocessor(
        ISpecimenBuilder builder,
        ISpecimenCommand command,
        IRequestSpecification specification)
    {
        Builder = builder ?? throw new ArgumentNullException(nameof(builder));
        Command = command ?? throw new ArgumentNullException(nameof(command));
        Specification = specification ?? throw new ArgumentNullException(nameof(specification));
    }

    /// <summary>
    /// Creates a new specimen based on a request and performs an action on the created
    /// specimen.
    /// </summary>
    /// <param name="request">The request that describes what to create.</param>
    /// <param name="context">A context that can be used to create other specimens.</param>
    /// <returns>
    /// The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The <paramref name="request"/> can be any object, but will often be a
    /// <see cref="Type"/> or other <see cref="System.Reflection.MemberInfo"/> instances.
    /// </para>
    /// </remarks>
    public object Create(object request, ISpecimenContext context)
    {
        var specimen = Builder.Create(request, context);
        if (specimen == null)
            return null;

        if (specimen is NoSpecimen ns)
            return ns;

        if (!Specification.IsSatisfiedBy(request))
            return specimen;

        Command.Execute(specimen, context);
        return specimen;
    }

    /// <summary>Composes the supplied builders.</summary>
    /// <param name="builders">The builders to compose.</param>
    /// <returns>
    /// A new <see cref="ISpecimenBuilderNode" /> instance containing
    /// <paramref name="builders" /> as child nodes.
    /// </returns>
    public ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
    {
        if (builders == null) throw new ArgumentNullException(nameof(builders));

        var composedBuilder = CompositeSpecimenBuilder.ComposeIfMultiple(builders);
        return new Postprocessor(composedBuilder, Command, Specification);
    }

    /// <inheritdoc />
    public IEnumerator<ISpecimenBuilder> GetEnumerator()
    {
        yield return Builder;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

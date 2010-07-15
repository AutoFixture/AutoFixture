using System.Collections.Generic;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Defines a set of <see cref="IList{ISpecimenBuilder}"/> that can be used to customize how
    /// the <see cref="ISpecimenBuilder"/> created by
    /// <see cref="ISpecimenBuilderComposer.Compose"/> is configured.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A standard implementation is that <see cref="ISpecimenBuilderComposer.Compose"/> will
    /// create an <see cref="ISpecimenBuilder"/> instance that wraps the 'real' builder in a
    /// <see cref="CompositeSpecimenBuilder"/> where <see cref="Customizations"/> precede the
    /// 'real' builder and <see cref="ResidueCollectors"/> trail it. However, other implementations
    /// are allowed as long as all customizations are taken into account when Compose is invoked.
    /// </para>
    /// </remarks>
    public interface ICustomizableComposer : ISpecimenBuilderComposer
    {
        /// <summary>
        /// Gets customizations that <see cref="ISpecimenBuilderComposer.Compose"/> will take into
        /// accout.
        /// </summary>
        /// <remarks>
        /// <para>
        /// It is expected that customizations pre-empt whichever other
        /// <see cref="ISpecimenBuilder"/> is created by
        /// <see cref="ISpecimenBuilderComposer.Compose"/>.
        /// </para>
        /// </remarks>
        IList<ISpecimenBuilder> Customizations { get; }

        /// <summary>
        /// Gets the residue collectors that <see cref="ISpecimenBuilderComposer.Compose"/> will
        /// take into account.
        /// </summary>
        /// <remarks>
        /// <para>
        /// It is expected that residue collectors provide fallback mechanisms if no ealier
        /// <see cref="ISpecimenBuilder"/> can handle a request.
        /// </para>
        /// </remarks>
        IList<ISpecimenBuilder> ResidueCollectors { get; }
    }
}

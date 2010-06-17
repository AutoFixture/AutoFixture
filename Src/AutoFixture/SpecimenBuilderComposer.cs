using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Dsl;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Provides an API to customize the creational strategy for a specific type.
    /// </summary>
    public static class SpecimenBuilderComposer
    {
        /// <summary>
        /// Customizes the creation algorithm for a single object.
        /// </summary>
        /// <typeparam name="T">
        /// The type of object for which the algorithm should be customized.
        /// </typeparam>
        /// <returns>
        /// A <see cref="ICustomizationComposer{T}"/> that can be used to customize the creation
        /// algorithm before creating the object.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the FxCop DL.")]
        public static ICustomizationComposer<T> Build<T>(this ISpecimenBuilderComposer composer)
        {
            return new CompositeComposer<T>(new Composer<T>(), new NullComposer<T>(composer.Compose));
        }
    }
}

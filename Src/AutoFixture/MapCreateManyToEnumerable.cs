using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Maps a call to
    /// <see cref="SpecimenFactory.CreateMany{T}(ISpecimenBuilder)" /> to a
    /// call for <see cref="IEnumerable{T}" />.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Normally,
    /// <see cref="SpecimenFactory.CreateMany{T}(ISpecimenBuilder)" /> returns
    /// its own implementation of <see cref="IEnumerable{T}" />, even if other
    /// customizations have changed how IEnumerable&lt;T&gt; are handled. This
    /// ensures that CreateMany&lt;T&gt; still returns many instances, even
    /// when IEnumerable&lt;T&gt; have been changed to return an empty
    /// sequence.
    /// </para>
    /// <para>
    /// If you want to change this default behaviour, you can use this
    /// customization.
    /// </para>
    /// </remarks>
    /// <seealso cref="MultipleToEnumerableRelay" />
    public class MapCreateManyToEnumerable : ICustomization
    {
        /// <summary>
        /// Customizes the specified fixture.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        /// <exception cref="System.ArgumentNullException">
        /// fixture is null
        /// </exception>
        /// <remarks>
        /// <para>
        /// Customizes a <see cref="IFixture" /> so that
        /// <see cref="SpecimenFactory.CreateMany{T}(ISpecimenBuilder)" />
        /// exhibits the same behaviour as a request to create
        /// <see cref="IEnumerable{T}" />.
        /// </para>
        /// </remarks>
        /// <seealso cref="MapCreateManyToEnumerable" />
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            fixture.Customizations.Insert(0, new MultipleToEnumerableRelay());
        }
    }
}

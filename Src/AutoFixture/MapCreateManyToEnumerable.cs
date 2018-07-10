using System;
using System.Collections.Generic;
using AutoFixture.Kernel;

namespace AutoFixture
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
    /// If you want to change this default behavior, you can use this
    /// customization.
    /// </para>
    /// </remarks>
    /// <seealso cref="MultipleToEnumerableRelay" />
    [Obsolete("This relay is obsolete and will be removed in future versions. " +
              "The reason is that it causes recursion overflow for \"many\" requests " +
              "if you forget to add IEnumerable<T> customization.\n" +
              "Please use the following code snippet instead:\n" +
              "fixture.Customizations.Add(" +
              "  new FilteringSpecimenBuilder(" +
              "    new FixedBuilder(" +
              "      SEQUENCE_TO_RETURN)," +
              "    new EqualRequestSpecification(" +
              "      new MultipleRequest(" +
              "        new SeededRequest(" +
              "          typeof(T)," +
              "          default(T))))));\n\n" +
              "If you need that often, please create an extension method inside your project.")]
    public class MapCreateManyToEnumerable : ICustomization
    {
        /// <summary>
        /// Customizes the specified fixture.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        /// <exception cref="System.ArgumentNullException">
        /// fixture is null.
        /// </exception>
        /// <remarks>
        /// <para>
        /// Customizes a <see cref="IFixture" /> so that
        /// <see cref="SpecimenFactory.CreateMany{T}(ISpecimenBuilder)" />
        /// exhibits the same behavior as a request to create
        /// <see cref="IEnumerable{T}" />.
        /// </para>
        /// </remarks>
        /// <seealso cref="MapCreateManyToEnumerable" />
        public void Customize(IFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            fixture.Customizations.Insert(0, new MultipleToEnumerableRelay());
        }
    }
}

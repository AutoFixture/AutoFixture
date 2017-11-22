using System;
using System.Collections.Generic;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// A customization that enables conventions for well-known types that represents multiple
    /// items.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When this customization is added to an <see cref="IFixture"/> instance, requests for
    /// common sequence and collection types will be satisfied with instances populated with
    /// multiple items.
    /// </para>
    /// <para>
    /// Normally, <see cref="Fixture"/> can satisfy requests for <see cref="List{T}" /> and similar
    /// collection types, but the returned instances will be empty. When the
    /// <see cref="MultipleCustomization" /> is added to an <see cref="IFixture" />, such
    /// collection specimens will be populated with items.
    /// </para>
    /// <para>
    /// Please note that apart from the concrete types <see cref="List{T}"/>,
    /// <see cref="System.Collections.ObjectModel.Collection{T}"/> etc. this
    /// <see cref="ICustomization"/> also resolves requests for the related interfaces
    /// <see cref="IList{T}"/>, <see cref="ICollection{T}"/> etc. This can potentially conflict
    /// with other customizations, such as the auto-mocking extensions for AutoFixture. In this
    /// case latest customization to be added to a Fixture wins.
    /// </para>
    /// </remarks>
    [Obsolete("This customization is no longer needed as builders are available out-of-the-box. It will be removed in future version of AutoFixture.")]
    public class MultipleCustomization : ICustomization
    {
        /// <summary>
        /// Customizes the specified fixture by adding conventions for populating sequences and
        /// collections.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        /// <seealso cref="MultipleCustomization" />
        public void Customize(IFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            fixture.ResidueCollectors.Add(new DictionaryRelay());
            fixture.ResidueCollectors.Add(new CollectionRelay());
            fixture.ResidueCollectors.Add(new ListRelay());
            fixture.ResidueCollectors.Add(new EnumerableRelay());

            fixture.Customizations.Add(
                new FilteringSpecimenBuilder(
                    new Postprocessor(
                        new MethodInvoker(
                            new ModestConstructorQuery()),
                        new DictionaryFiller()),
                    new DictionarySpecification()));
            fixture.Customizations.Add(
                new FilteringSpecimenBuilder(
                    new MethodInvoker(
                        new ListFavoringConstructorQuery()),
                    new CollectionSpecification()));
            fixture.Customizations.Add(
                new FilteringSpecimenBuilder(
                    new MethodInvoker(
                        new EnumerableFavoringConstructorQuery()),
                    new HashSetSpecification()));
            fixture.Customizations.Add(
                new FilteringSpecimenBuilder(
                    new MethodInvoker(
                        new EnumerableFavoringConstructorQuery()),
                    new ListSpecification()));
        }
    }
}

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using AutoFixture.Kernel;

namespace AutoFixture.Dsl
{
    /// <summary>
    /// Provides statements that can be used to control how specimens are post-processed.
    /// </summary>
    /// <typeparam name="T">The type of specimen.</typeparam>
    public interface IPostprocessComposer<T> : ISpecimenBuilder
    {
        /// <summary>
        /// Performs the specified action on a specimen.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Do", Justification = "Renaming would be a breaking change.")]
        IPostprocessComposer<T> Do(Action<T> action);

        /// <summary>
        /// Disables auto-properties for a type of specimen.
        /// </summary>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        IPostprocessComposer<T> OmitAutoProperties();

        /// <summary>
        /// Registers that a writable property or field should be assigned an anonymous value as
        /// part of specimen post-processing.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property of field.</typeparam>
        /// <param name="propertyPicker">
        /// An expression that identifies the property or field that will should have a value
        /// assigned.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "With", Justification = "Renaming would be a breaking change.")]
        IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker);

        /// <summary>
        /// Registers that a writable property or field should be assigned a specific value as
        /// part of specimen post-processing.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property of field.</typeparam>
        /// <param name="propertyPicker">
        /// An expression that identifies the property or field that will have
        /// <paramref name="value"/> assigned.
        /// </param>
        /// <param name="value">
        /// The value to assign to the property or field identified by
        /// <paramref name="propertyPicker"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "With",
            Justification = "Renaming would be a breaking change.")]
        IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, TProperty value);

        /// <summary>
        /// Registers that a writable property or field should be assigned generated value as a part of specimen post-processing.
        /// </summary>
        /// <param name="propertyPicker">
        /// An expression that identifies the property or field that will have <paramref name="valueFactory"/> result assigned.
        /// </param>
        /// <param name="valueFactory">
        /// The factory of value to assign to the property or field identified by <paramref name="propertyPicker"/>.
        /// </param>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "With", Justification = "It's a part of the public API we currently have.")]
        IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, Func<TProperty> valueFactory);

        /// <summary>
        /// Registers that a writable property or field should be assigned generated value as a part of specimen post-processing.
        /// </summary>
        /// <param name="propertyPicker">
        /// An expression that identifies the property or field that will have <paramref name="valueFactory"/> result assigned.
        /// </param>
        /// <param name="valueFactory">
        /// The factory of value to assign to the property or field identified by <paramref name="propertyPicker"/>.
        /// </param>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "With", Justification = "It's a part of the public API we currently have.")]
        IPostprocessComposer<T> With<TProperty, TInput>(Expression<Func<T, TProperty>> propertyPicker, Func<TInput, TProperty> valueFactory);

        /// <summary>
        /// Enables auto-properties for a type of specimen.
        /// </summary>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        IPostprocessComposer<T> WithAutoProperties();

        /// <summary>
        /// Registers that a writable property should not be assigned any automatic value as
        /// part of specimen post-processing.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property or field to ignore.</typeparam>
        /// <param name="propertyPicker">
        /// An expression that identifies the property or field to be ignored.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        IPostprocessComposer<T> Without<TProperty>(Expression<Func<T, TProperty>> propertyPicker);
    }
}
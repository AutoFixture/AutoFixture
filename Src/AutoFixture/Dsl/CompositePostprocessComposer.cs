using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Dsl
{
    /// <summary>
    /// Aggregates an arbitrary number of <see cref="IPostprocessComposer{T}"/> instances.
    /// </summary>
    /// <typeparam name="T">The type of specimen to customize.</typeparam>
    public class CompositePostprocessComposer<T> : IPostprocessComposer<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompositePostprocessComposer&lt;T&gt;"/>
        /// class with a sequence of <see cref="IPostprocessComposer{T}"/> instances.
        /// </summary>
        /// <param name="composers">The composers to aggregate.</param>
        public CompositePostprocessComposer(IEnumerable<IPostprocessComposer<T>> composers)
            : this(composers.ToArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositePostprocessComposer&lt;T&gt;"/>
        /// class with an array of <see cref="IPostprocessComposer{T}"/> instances. 
        /// </summary>
        /// <param name="composers">The composers to aggregate.</param>
        public CompositePostprocessComposer(params IPostprocessComposer<T>[] composers)
        {
            if (composers == null)
            {
                throw new ArgumentNullException(nameof(composers));
            }

            this.Composers = composers;
        }

        /// <summary>
        /// Gets the aggregated composers.
        /// </summary>
        public IEnumerable<IPostprocessComposer<T>> Composers { get; }

        /// <summary>
        /// Performs the specified action on a specimen.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> Do(Action<T> action)
        {
            return new CompositePostprocessComposer<T>(from c in this.Composers
                                                       select c.Do(action));
        }

        /// <summary>
        /// Disables auto-properties for a type of specimen.
        /// </summary>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> OmitAutoProperties()
        {
            return new CompositePostprocessComposer<T>(from c in this.Composers
                                                       select c.OmitAutoProperties());
        }

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
        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            return new CompositePostprocessComposer<T>(from c in this.Composers
                                                       select c.With(propertyPicker));
        }

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
        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, TProperty value)
        {
            return new CompositePostprocessComposer<T>(from c in this.Composers
                                                       select c.With(propertyPicker, value));
        }

        /// <summary>
        /// Enables auto-properties for a type of specimen.
        /// </summary>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> WithAutoProperties()
        {
            return new CompositePostprocessComposer<T>(from c in this.Composers
                                                       select c.WithAutoProperties());
        }

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
        public IPostprocessComposer<T> Without<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            return new CompositePostprocessComposer<T>(from c in this.Composers
                                                       select c.Without(propertyPicker));
        }

        /// <summary>Creates a new specimen based on a request.</summary>
        /// <param name="request">
        /// The request that describes what to create.
        /// </param>
        /// <param name="context">
        /// A context that can be used to create other specimens.
        /// </param>
        /// <returns>
        /// The requested specimen if possible; otherwise a
        /// <see cref="NoSpecimen" /> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <paramref name="request" /> can be any object, but will often be a
        /// <see cref="Type" /> or other <see cref="System.Reflection.MemberInfo" /> instances.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            return new CompositeSpecimenBuilder(
                this.Composers.Cast<ISpecimenBuilder>())
                .Create(
                    request,
                    context);
        }
    }
}

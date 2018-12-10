using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoFixture.Kernel;

namespace AutoFixture.Dsl
{
    /// <summary>
    /// Aggregates an arbitrary number of <see cref="IPostprocessComposer{T}"/> instances.
    /// </summary>
    /// <typeparam name="T">The type of specimen to customize.</typeparam>
    [Obsolete("This class is deprecated and will be removed in future versions of AutoFixture. " +
              "Please file an issue on GitHub if you need this class in your project.")]
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
            this.Composers = composers ?? throw new ArgumentNullException(nameof(composers));
        }

        /// <summary>
        /// Gets the aggregated composers.
        /// </summary>
        public IEnumerable<IPostprocessComposer<T>> Composers { get; }

        /// <inheritdoc />
        public IPostprocessComposer<T> Do(Action<T> action)
        {
            return new CompositePostprocessComposer<T>(from c in this.Composers
                                                       select c.Do(action));
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> OmitAutoProperties()
        {
            return new CompositePostprocessComposer<T>(from c in this.Composers
                                                       select c.OmitAutoProperties());
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            return new CompositePostprocessComposer<T>(from c in this.Composers
                                                       select c.With(propertyPicker));
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, TProperty value)
        {
            return new CompositePostprocessComposer<T>(from c in this.Composers
                                                       select c.With(propertyPicker, value));
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, Func<TProperty> valueFactory)
        {
            return new CompositePostprocessComposer<T>(from c in this.Composers
                                                       select c.With(propertyPicker, valueFactory));
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> With<TProperty, TInput>(Expression<Func<T, TProperty>> propertyPicker, Func<TInput, TProperty> valueFactory)
        {
            return new CompositePostprocessComposer<T>(from c in this.Composers
                                                       select c.With(propertyPicker, valueFactory));
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> WithAutoProperties()
        {
            return new CompositePostprocessComposer<T>(from c in this.Composers
                                                       select c.WithAutoProperties());
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> Without<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            return new CompositePostprocessComposer<T>(from c in this.Composers
                                                       select c.Without(propertyPicker));
        }

        /// <inheritdoc />
        public object Create(object request, ISpecimenContext context)
        {
            return new CompositeSpecimenBuilder(this.Composers)
                       .Create(request, context);
        }
    }
}

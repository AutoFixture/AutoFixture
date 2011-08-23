using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// A Decorator of <see cref="IPostprocessComposer{T}"/> that applies behaviors when
    /// <see cref="Compose"/> is invoked.
    /// </summary>
    /// <typeparam name="T">The type of specimen to customize.</typeparam>
    /// <remarks>
    /// <para>
    /// In this context, a <i>behavior</i> is an <see cref="ISpecimenBuilderTransformation"/> that
    /// is used to decorate the <see cref="ISpecimenBuilder"/> composed by the decorated
    /// <see cref="Composer"/> with decorating builders.
    /// </para>
    /// <para>
    /// This can be used to ensure that the composed builder is decorated with appropriate builders
    /// such as <see cref="ThrowingRecursionGuard"/> or <see cref="TraceWriter"/>.
    /// </para>
    /// </remarks>
    public class BehaviorPostprocessComposer<T> : IPostprocessComposer<T>
    {
        private readonly IPostprocessComposer<T> composer;
        private readonly IEnumerable<ISpecimenBuilderTransformation> behaviors;

        /// <summary>
        /// Initializes a new instance of the <see cref="BehaviorPostprocessComposer&lt;T&gt;"/>
        /// class.
        /// </summary>
        /// <param name="composer">The composer to decorate.</param>
        /// <param name="behaviors">
        /// The behaviors which will be applied when <see cref="Compose"/> is invoked.
        /// </param>
        public BehaviorPostprocessComposer(IPostprocessComposer<T> composer, IEnumerable<ISpecimenBuilderTransformation> behaviors)
            : this(composer, behaviors.ToArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BehaviorPostprocessComposer&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="composer">The composer to decorate.</param>
        /// <param name="behaviors">
        /// The behaviors which will be applied when <see cref="Compose"/> is invoked.
        /// </param>
        public BehaviorPostprocessComposer(IPostprocessComposer<T> composer, params ISpecimenBuilderTransformation[] behaviors)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }
            if (behaviors == null)
            {
                throw new ArgumentNullException("behaviors");
            }

            this.composer = composer;
            this.behaviors = behaviors;
        }

        /// <summary>
        /// Gets the behaviors that will be applied when <see cref="Compose"/> is invoked.
        /// </summary>
        public IEnumerable<ISpecimenBuilderTransformation> Behaviors
        {
            get { return this.behaviors; }
        }

        /// <summary>
        /// Gets the decorated composer.
        /// </summary>
        public IPostprocessComposer<T> Composer
        {
            get { return this.composer; }
        }

        /// <summary>
        /// Returns a new <see cref="BehaviorPostprocessComposer{T}"/> that decorates the new
        /// composer, but otherwise preserves other state information of the instance (such as the
        /// <see cref="Behaviors"/>.
        /// </summary>
        /// <param name="newComposer">The new composer to decorate.</param>
        /// <returns>
        /// A new <see cref="BehaviorPostprocessComposer{T}"/> that decorates
        /// <paramref name="newComposer"/>.
        /// </returns>
        public BehaviorPostprocessComposer<T> With(IPostprocessComposer<T> newComposer)
        {
            if (newComposer == null)
            {
                throw new ArgumentNullException("newComposer");
            }

            return new BehaviorPostprocessComposer<T>(newComposer, this.Behaviors);
        }

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
            return this.With(this.Composer.Do(action));
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
            return this.With(this.Composer.OmitAutoProperties());
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
            return this.With(this.Composer.With(propertyPicker));
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
            return this.With(this.Composer.With(propertyPicker, value));
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
            return this.With(this.Composer.WithAutoProperties());
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
            return this.With(this.Composer.Without(propertyPicker));
        }

        /// <summary>
        /// Composes a new <see cref="ISpecimenBuilder"/> instance.
        /// </summary>
        /// <returns>
        /// A new <see cref="ISpecimenBuilder"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The return value is created from the <see cref="ISpecimenBuilder"/> instances created
        /// by the decorated <see cref="Composer"/>, itself decorated with the decorating builders
        /// created by <see cref="Behaviors"/>.
        /// </para>
        /// </remarks>
        public ISpecimenBuilder Compose()
        {
            return this.Behaviors.Aggregate(
                this.Composer.Compose(),
                (builder, behavior) => 
                    behavior.Transform(builder));
        }
    }
}

using System;
using System.Linq.Expressions;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Dsl
{
    /// <summary>
    /// An <see cref="ICustomizationComposer{T}"/> that does not customize anything, but can still
    /// compose an <see cref="ISpecimenBuilder"/> - typically by returning the instance it is
    /// configured to use.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NullComposer<T> : ICustomizationComposer<T>
    {
        private readonly Func<ISpecimenBuilder> compose;

        /// <summary>
        /// Initializes a new instance of the <see cref="NullComposer&lt;T&gt;"/> class.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Initialized in this way, <see cref="Compose"/> will return an empty
        /// <see cref="CompositeSpecimenBuilder"/>.
        /// </para>
        /// </remarks>
        public NullComposer()
            : this(new CompositeSpecimenBuilder())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullComposer&lt;T&gt;"/> class with an
        /// <see cref="ISpecimenBuilder"/> that will be returned by the <see cref="Compose"/>
        /// method.
        /// </summary>
        /// <param name="builder">
        /// The builder to return by the <see cref="Compose"/> method.
        /// </param>
        public NullComposer(ISpecimenBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            this.compose = () => builder;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullComposer&lt;T&gt;"/> class with a
        /// function used to implement the <see cref="Compose"/> method.
        /// </summary>
        /// <param name="factory">
        /// The function that will be used to implement <see cref="Compose"/>.
        /// </param>
        public NullComposer(Func<ISpecimenBuilder> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            this.compose = factory;
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="factory">Ignored.</param>
        /// <returns>The current instance.</returns>
        public IPostprocessComposer<T> FromSeed(Func<T, T> factory)
        {
            return this;
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="factory">Ignored.</param>
        /// <returns>The current instance.</returns>
        public IPostprocessComposer<T> FromFactory(ISpecimenBuilder factory)
        {
            return this;
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="factory">Ignored.</param>
        /// <returns>The current instance.</returns>
        public IPostprocessComposer<T> FromFactory(Func<T> factory)
        {
            return this;
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="factory">Ignored.</param>
        /// <returns>The current instance.</returns>
        public IPostprocessComposer<T> FromFactory<TInput>(Func<TInput, T> factory)
        {
            return this;
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="factory">Ignored.</param>
        /// <returns>The current instance.</returns>
        public IPostprocessComposer<T> FromFactory<TInput1, TInput2>(Func<TInput1, TInput2, T> factory)
        {
            return this;
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="factory">Ignored.</param>
        /// <returns>The current instance.</returns>
        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3>(Func<TInput1, TInput2, TInput3, T> factory)
        {
            return this;
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="factory">Ignored.</param>
        /// <returns>The current instance.</returns>
        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3, TInput4>(Func<TInput1, TInput2, TInput3, TInput4, T> factory)
        {
            return this;
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="action">Ignored.</param>
        /// <returns>The current instance.</returns>
        public IPostprocessComposer<T> Do(Action<T> action)
        {
            return this;
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <returns>The current instance.</returns>
        public IPostprocessComposer<T> OmitAutoProperties()
        {
            return this;
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="propertyPicker">Ignored.</param>
        /// <returns>The current instance.</returns>
        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            return this;
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="propertyPicker">Ignored.</param>
        /// <param name="value">Ignored.</param>
        /// <returns>The current instance.</returns>
        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, TProperty value)
        {
            return this;
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <returns>The current instance.</returns>
        public IPostprocessComposer<T> WithAutoProperties()
        {
            return this;
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="propertyPicker">Ignored.</param>
        /// <returns>The current instance.</returns>
        public IPostprocessComposer<T> Without<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            return this;
        }

        /// <summary>
        /// Composes a new <see cref="ISpecimenBuilder"/> instance.
        /// </summary>
        /// <returns>
        /// An <see cref="ISpecimenBuilder"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Which <see cref="ISpecimenBuilder"/> is returned depends on how the
        /// <see cref="NullComposer{T}"/> instance was configured through its constructor.
        /// </para>
        /// </remarks>
        public ISpecimenBuilder Compose()
        {
            return this.compose();
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
            return this.compose().Create(request, context);
        }
    }
}

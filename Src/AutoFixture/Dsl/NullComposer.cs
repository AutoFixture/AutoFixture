using System;
using System.Linq.Expressions;
using AutoFixture.Kernel;

namespace AutoFixture.Dsl
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
        public NullComposer()
            : this(new CompositeSpecimenBuilder())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullComposer&lt;T&gt;"/> class with an
        /// <see cref="ISpecimenBuilder"/> that will be returned by the <see cref="Compose"/> method.
        /// </summary>
        public NullComposer(ISpecimenBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            this.compose = () => builder;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullComposer&lt;T&gt;"/> class with a
        /// function used to implement the <see cref="Compose"/> method.
        /// </summary>
        public NullComposer(Func<ISpecimenBuilder> factory)
        {
            this.compose = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> FromSeed(Func<T, T> factory) => this;

        /// <inheritdoc />
        public IPostprocessComposer<T> FromFactory(ISpecimenBuilder factory) => this;

        /// <inheritdoc />
        public IPostprocessComposer<T> FromFactory(Func<T> factory) => this;

        /// <inheritdoc />
        public IPostprocessComposer<T> FromFactory<TInput>(Func<TInput, T> factory) => this;

        /// <inheritdoc />
        public IPostprocessComposer<T> FromFactory<TInput1, TInput2>(Func<TInput1, TInput2, T> factory) => this;

        /// <inheritdoc />
        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3>(Func<TInput1, TInput2, TInput3, T> factory) => this;

        /// <inheritdoc />
        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3, TInput4>(Func<TInput1, TInput2, TInput3, TInput4, T> factory) => this;

        /// <inheritdoc />
        public IPostprocessComposer<T> Do(Action<T> action) => this;

        /// <inheritdoc />
        public IPostprocessComposer<T> OmitAutoProperties() => this;

        /// <inheritdoc />
        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker) => this;

        /// <inheritdoc />
        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, TProperty value) => this;

        /// <inheritdoc />
        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, Func<TProperty> valueFactory) => this;

        /// <inheritdoc />
        public IPostprocessComposer<T> With<TProperty, TInput>(Expression<Func<T, TProperty>> propertyPicker, Func<TInput, TProperty> valueFactory) => this;

        /// <inheritdoc />
        public IPostprocessComposer<T> WithAutoProperties() => this;

        /// <inheritdoc />
        public IPostprocessComposer<T> Without<TProperty>(Expression<Func<T, TProperty>> propertyPicker) => this;

        /// <summary>
        /// Composes a new <see cref="ISpecimenBuilder"/> instance.
        /// </summary>
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

        /// <inheritdoc />
        public object Create(object request, ISpecimenContext context)
        {
            return this.compose().Create(request, context);
        }
    }
}

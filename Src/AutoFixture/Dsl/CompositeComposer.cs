using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Dsl
{
    /// <summary>
    /// Aggregates an arbitrary number of <see cref="ICustomizationComposer{T}"/> instances.
    /// </summary>
    /// <typeparam name="T">The type of specimen to customize.</typeparam>
    [Obsolete("Please use CompositeNodeComposer<T> instead.")]
    public class CompositeComposer<T> : ICustomizationComposer<T>
    {
        private readonly IEnumerable<ICustomizationComposer<T>> composers;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeComposer&lt;T&gt;"/> class
        /// with a sequence of <see cref="ICustomizationComposer{T}"/> instances.
        /// </summary>
        /// <param name="composers">The composers to aggregate.</param>
        public CompositeComposer(IEnumerable<ICustomizationComposer<T>> composers)
            : this(composers.ToArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeComposer&lt;T&gt;"/> class
        /// with an array of <see cref="ICustomizationComposer{T}"/> instances.
        /// </summary>
        /// <param name="composers">The composers to aggregate.</param>
        public CompositeComposer(params ICustomizationComposer<T>[] composers)
        {
            if (composers == null)
            {
                throw new ArgumentNullException("composers");
            }

            this.composers = composers;
        }

        /// <summary>
        /// Gets the aggregated composers.
        /// </summary>
        public IEnumerable<ICustomizationComposer<T>> Composers
        {
            get { return this.composers; }
        }

        /// <summary>
        /// Specifies a function that defines how to create a specimen from a seed.
        /// </summary>
        /// <param name="factory">The factory used to create specimens from seeds.</param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> FromSeed(Func<T, T> factory)
        {
            return new CompositePostprocessComposer<T>(from c in this.composers
                                                       select c.FromSeed(factory));
        }

        /// <summary>
        /// Specifies an <see cref="ISpecimenBuilder"/> that can create specimens of the
        /// appropriate type. Mostly for advanced scenarios.
        /// </summary>
        /// <param name="factory">
        /// An <see cref="ISpecimenBuilder"/> that can create specimens of the appropriate type.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> FromFactory(ISpecimenBuilder factory)
        {
            return new CompositePostprocessComposer<T>(from c in this.composers
                                                       select c.FromFactory(factory));
        }

        /// <summary>
        /// Specifies that an anonymous object should be created in a particular way; often by
        /// using a constructor.
        /// </summary>
        /// <param name="factory">
        /// A function that will be used to create the object. This will often be a constructor.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> FromFactory(Func<T> factory)
        {
            return new CompositePostprocessComposer<T>(from c in this.composers
                                                       select c.FromFactory(factory));
        }

        /// <summary>
        /// Specifies that a specimen should be created in a particular way, using a single input
        /// parameter for the factory.
        /// </summary>
        /// <typeparam name="TInput">
        /// The type of input parameter to use when invoking <paramref name="factory"/>
        /// .</typeparam>
        /// <param name="factory">
        /// A function that will be used to create the object. This will often be a constructor
        /// that takes a single constructor argument of type <typeparamref name="TInput"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> FromFactory<TInput>(Func<TInput, T> factory)
        {
            return new CompositePostprocessComposer<T>(from c in this.composers
                                                       select c.FromFactory(factory));
        }

        /// <summary>
        /// Specifies that a specimen should be created in a particular way, using two input
        /// parameters for the construction.
        /// </summary>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <param name="factory">
        /// A function that will be used to create the object. This will often be a constructor
        /// that takes two constructor arguments of type <typeparamref name="TInput1"/> and
        /// <typeparamref name="TInput2"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> FromFactory<TInput1, TInput2>(Func<TInput1, TInput2, T> factory)
        {
            return new CompositePostprocessComposer<T>(from c in this.composers
                                                       select c.FromFactory(factory));
        }

        /// <summary>
        /// Specifies that a specimen should be created in a particular way, using three input
        /// parameters for the construction.
        /// </summary>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput3">
        /// The type of the third input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <param name="factory">
        /// A function that will be used to create the object. This will often be a constructor
        /// that takes three constructor arguments of type <typeparamref name="TInput1"/>,
        /// <typeparamref name="TInput2"/> and <typeparamref name="TInput3"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3>(Func<TInput1, TInput2, TInput3, T> factory)
        {
            return new CompositePostprocessComposer<T>(from c in this.composers
                                                       select c.FromFactory(factory));
        }

        /// <summary>
        /// Specifies that a specimen should be created in a particular way, using four input
        /// parameters for the construction.
        /// </summary>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput3">
        /// The type of the third input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput4">
        /// The type of the fourth input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <param name="factory">
        /// A function that will be used to create the object. This will often be a constructor
        /// that takes three constructor arguments of type <typeparamref name="TInput1"/>,
        /// <typeparamref name="TInput2"/>, <typeparamref name="TInput3"/> and
        /// <typeparamref name="TInput4"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3, TInput4>(Func<TInput1, TInput2, TInput3, TInput4, T> factory)
        {
            return new CompositePostprocessComposer<T>(from c in this.composers
                                                       select c.FromFactory(factory));
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
            return new CompositePostprocessComposer<T>(this.composers.ToArray()).Do(action);
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
            return new CompositePostprocessComposer<T>(this.composers.ToArray()).OmitAutoProperties();
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
            return new CompositePostprocessComposer<T>(this.composers.ToArray()).With(propertyPicker);
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
            return new CompositePostprocessComposer<T>(this.composers.ToArray()).With(propertyPicker, value);
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
            return new CompositePostprocessComposer<T>(this.composers.ToArray()).WithAutoProperties();
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
            return new CompositePostprocessComposer<T>(this.composers.ToArray()).Without(propertyPicker);
        }

        /// <summary>
        /// Composes a new <see cref="ISpecimenBuilder"/> instance.
        /// </summary>
        /// <returns>
        /// A new <see cref="ISpecimenBuilder"/> instance.
        /// </returns>
        public ISpecimenBuilder Compose()
        {
            return new CompositeSpecimenBuilder(from c in this.composers
                                                select c.Compose());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using System.Linq.Expressions;

namespace Ploeh.AutoFixture.Dsl
{
    /// <summary>
    /// Enables composition customization of a single type of specimen.
    /// </summary>
    /// <typeparam name="T">The type of specimen.</typeparam>
    public class Composer<T> : ICustomizationComposer<T>
    {
        private readonly ISpecimenBuilder factoryBuilder;
        private readonly IEnumerable<ISpecifiedSpecimenCommand<T>> postprocessors;
        private readonly bool enableAutoProperties;

        /// <summary>
        /// Initializes a new instance of the <see cref="Composer&lt;T&gt;"/> class.
        /// </summary>
        public Composer()
            : this(new ModestConstructorInvoker(), Enumerable.Empty<ISpecifiedSpecimenCommand<T>>(), false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Composer&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="factory">
        /// A factory which will be used to create specimen instances.
        /// </param>
        /// <param name="postprocessors">
        /// Post-processors that will be executed on each created specimen.
        /// </param>
        /// <param name="enableAutoProperties">
        /// Enables auto properties if set to <see langword="true"/>.
        /// </param>
        public Composer(ISpecimenBuilder factory, IEnumerable<ISpecifiedSpecimenCommand<T>> postprocessors, bool enableAutoProperties)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }
            if (postprocessors == null)
            {
                throw new ArgumentNullException("postprocessors");
            }

            this.factoryBuilder = factory;
            this.postprocessors = postprocessors.ToList();
            this.enableAutoProperties = enableAutoProperties;
        }

        /// <summary>
        /// Gets a value indicating whether writable properties and fields will be assigned with
        /// anonymous values.
        /// </summary>
        public bool EnableAutoProperties
        {
            get { return this.enableAutoProperties; }
        }

        /// <summary>
        /// Gets the factory used to create specimens.
        /// </summary>
        public ISpecimenBuilder Factory
        {
            get { return this.factoryBuilder; }
        }

        /// <summary>
        /// Gets the postprocessors that will be executed on each created specimen.
        /// </summary>
        public IEnumerable<ISpecifiedSpecimenCommand<T>> Postprocessors
        {
            get { return this.postprocessors; }
        }

        /// <summary>
        /// Controls whether auto-properties will be enabled or not.
        /// </summary>
        /// <param name="enable">Set to <see langword="true"/> to enable auto-properties.</param>
        /// <returns>
        /// A new instance of <see cref="Composer{T}"/> with <see cref="EnableAutoProperties"/> set
        /// to the value of <paramref name="enable"/>.
        /// </returns>
        public Composer<T> WithAutoProperties(bool enable)
        {
            return new Composer<T>(this.Factory, this.Postprocessors, enable);
        }

        /// <summary>
        /// Controls which factory will be used when specimens are created.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns>
        /// A new instance of <see cref="Composer{T}"/> with <see cref="Factory"/> set to the value
        /// of <paramref name="factory"/>.
        /// </returns>
        public Composer<T> WithFactory(ISpecimenBuilder factory)
        {
            return new Composer<T>(factory, this.postprocessors, this.EnableAutoProperties);
        }

        /// <summary>
        /// Adds a post-processor to the customization.
        /// </summary>
        /// <param name="postprocessor">The postprocessor to add.</param>
        /// <returns>
        /// A new instance of <see cref="Composer{T}"/> with <paramref name="postprocessor"/> added
        /// to <see cref="Postprocessors"/>.
        /// </returns>
        public Composer<T> WithPostprocessor(ISpecifiedSpecimenCommand<T> postprocessor)
        {
            if (postprocessor == null)
            {
                throw new ArgumentNullException("postprocessor");
            }

            return new Composer<T>(this.Factory, this.postprocessors.Concat(new[] { postprocessor }), this.EnableAutoProperties);
        }

        #region IFactoryComposer<T> Members

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
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            return this.WithFactory(new SeededFactory<T>(factory));
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
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            return this.WithFactory(new SpecimenFactory<T>(factory));
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
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            return this.WithFactory(new SpecimenFactory<TInput, T>(factory));
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
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            return this.WithFactory(new SpecimenFactory<TInput1, TInput2, T>(factory));
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
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            return this.WithFactory(new SpecimenFactory<TInput1, TInput2, TInput3, T>(factory));
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
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            return this.WithFactory(new SpecimenFactory<TInput1, TInput2, TInput3, TInput4, T>(factory));
        }

        #endregion

        #region IPostprocessComposer<T> Members

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
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            var postprocessor = new UnspecifiedSpecimenCommand<T>(action);
            return this.WithPostprocessor(postprocessor);
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
            return this.WithAutoProperties(false);
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
            if (propertyPicker == null)
            {
                throw new ArgumentNullException("propertyPicker");
            }

            var postprocessor = new BindingCommand<T, TProperty>(propertyPicker);
            return this.WithPostprocessor(postprocessor);
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
            if (propertyPicker == null)
            {
                throw new ArgumentNullException("propertyPicker");
            }

            var postprocessor = new BindingCommand<T, TProperty>(propertyPicker, value);
            return this.WithPostprocessor(postprocessor);
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
            return this.WithAutoProperties(true);
        }

        /// <summary>
        /// Withouts the specified property picker.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="propertyPicker">The property picker.</param>
        /// <returns></returns>
        public IPostprocessComposer<T> Without<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            if (propertyPicker == null)
            {
                throw new ArgumentNullException("propertyPicker");
            }

            var postprocessor = new SpecifiedNullCommand<T, TProperty>(propertyPicker);
            return this.WithPostprocessor(postprocessor);
        }

        #endregion

        #region ISpecimenBuilderComposer Members

        /// <summary>
        /// Composes a new <see cref="ISpecimenBuilder"/> instance based on the current
        /// customization.
        /// </summary>
        /// <returns>
        /// A new <see cref="ISpecimenBuilder"/> instance created and post-processed according to
        /// the rules encapsulated by the <see cref="Composer{T}"/> instance.
        /// </returns>
        public ISpecimenBuilder Compose()
        {
            var builder = this.Factory;
            foreach (var p in this.Postprocessors)
            {
                builder = new Postprocessor<T>(builder, p.Execute);
            }

            if (this.EnableAutoProperties)
            {
                var reservedProperties =
                    new InverseRequestSpecification(
                        new OrRequestSpecification(
                            this.Postprocessors.Cast<IRequestSpecification>().Concat(new[] { new FalseRequestSpecification() })));
                builder = new Postprocessor<T>(
                    builder,
                    new AutoPropertiesCommand<T>(reservedProperties).Execute,
                    new AnyTypeSpecification());
            }

            return new FilteringSpecimenBuilder(builder, new ExactTypeSpecification(typeof(T)));
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.Dsl;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Provides anonymous object creation service.
    /// </summary>
    public class Fixture : ISpecimenBuilderComposer
    {
        private readonly CompositeSpecimenBuilder customizer;
        private readonly ISpecimenBuilder engine;
        private readonly CompositeSpecimenBuilder residueCollector;
        private readonly IMany many;

        /// <summary>
        /// Initializes a new instance of the <see cref="Fixture"/> class.
        /// </summary>
        public Fixture()
            : this(new DefaultEngineParts())
        {
        }

        public Fixture(DefaultRelays engineParts)
            : this(new CompositeSpecimenBuilder(engineParts), engineParts)
        {
        }

        public Fixture(ISpecimenBuilder engine, IMany many)
        {
            if (engine == null)
            {
                throw new ArgumentNullException("engine");
            }
            if (many == null)
            {
                throw new ArgumentNullException("many");
            }

            this.customizer = new CompositeSpecimenBuilder();
            this.engine = engine;
            this.residueCollector = new CompositeSpecimenBuilder();
            this.many = many;
        }

        public IList<ISpecimenBuilder> Customizations
        {
            get { return this.customizer.Builders; }
        }

        public ISpecimenBuilder Engine
        {
            get { return this.engine; }
        }

        /// <summary>
        /// Gets or sets a number that controls how many objects are created when a
        /// <see cref="Fixture"/> creates more than one anonymous objects.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The default value is 3.
        /// </para>
        /// </remarks>
        /// <seealso cref="AddManyTo{T}(ICollection{T})" />
        /// <seealso cref="AddManyTo{T}(ICollection{T}, Func{T})" />
        /// <seealso cref="CreateMany()" />
        /// <seealso cref="CreateMany(int)" />
        /// <seealso cref="Repeat"/>
        public int RepeatCount
        {
            get { return this.many.Count; }
            set { this.many.Count = value; }
        }

        public IList<ISpecimenBuilder> ResidueCollectors
        {
            get { return this.residueCollector.Builders; }
        }

        /// <summary>
        /// Gets or sets if writable properties should generally be assigned a value when 
        /// generating an anonymous object.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The default value is false.
        /// </para>
        /// </remarks>
        public bool OmitAutoProperties { get; set; }

        /// <summary>
        /// Gets or sets a callback that can be invoked to resolve unresolved types.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Func<Type, object> Resolver { get; set; }

        /// <summary>
        /// Adds many anonymously created objects to a list.
        /// </summary>
        /// <typeparam name="T">The type of object that is contained in the list.</typeparam>
        /// <param name="collection">
        /// The list to which the anonymously created objects will be added.
        /// </param>
        /// <remarks>
        /// <para>
        /// The number of objects created and added is determined by <see cref="RepeatCount"/>.
        /// </para>
        /// </remarks>
        /// <seealso cref="AddManyTo{T}(ICollection{T}, int)"/>
        /// <seealso cref="AddManyTo{T}(ICollection{T}, Func{T})"/>
        public void AddManyTo<T>(ICollection<T> collection)
        {
            this.AddManyTo(collection, this.CreateAnonymous<T>);
        }

        /// <summary>
        /// Adds many anonymously created objects to a list.
        /// </summary>
        /// <typeparam name="T">The type of object that is contained in the list.</typeparam>
        /// <param name="collection">
        /// The list to which the anonymously created objects will be added.
        /// </param>
        /// <param name="repeatCount">The number of objects created and added.</param>
        /// <seealso cref="AddManyTo{T}(ICollection{T})"/>
        /// <seealso cref="AddManyTo{T}(ICollection{T}, Func{T})"/>
        public void AddManyTo<T>(ICollection<T> collection, int repeatCount)
        {
            collection.AddMany(this.CreateAnonymous<T>, repeatCount);
        }

        /// <summary>
        /// Adds many objects to a list using the provided function to create each object.
        /// </summary>
        /// <typeparam name="T">The type of object that is contained in the list.</typeparam>
        /// <param name="collection">
        /// The list to which the created objects will be added.
        /// </param>
        /// <param name="creator">
        /// The function that creates each object which is subsequently added to
        /// <paramref name="collection"/>.
        /// </param>
        /// <remarks>
        /// <para>
        /// The number of objects created and added is determined by <see cref="RepeatCount"/>.
        /// </para>
        /// </remarks>
        /// <seealso cref="AddManyTo{T}(ICollection{T})"/>
        /// <seealso cref="AddManyTo{T}(ICollection{T}, int)"/>
        public void AddManyTo<T>(ICollection<T> collection, Func<T> creator)
        {
            collection.AddMany(creator, this.RepeatCount);
        }

        /// <summary>
        /// Customizes the creation algorithm for a single object.
        /// </summary>
        /// <typeparam name="T">
        /// The type of object for which the algorithm should be customized.
        /// </typeparam>
        /// <returns>
        /// A <see cref="ICustomizationComposer{T}"/> that can be used to customize the creation
        /// algorithm before creating the object.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the FxCop DL.")]
        public ICustomizationComposer<T> Build<T>()
        {
            return new CompositeComposer<T>(
                new Composer<T>().WithAutoProperties(this.EnableAutoProperties),
                new NullComposer<T>(this.Compose));
        }

        /// <summary>
        /// Customizes the creation algorithm for all objects of a given type.
        /// </summary>
        /// <typeparam name="T">The type of object to customize.</typeparam>
        /// <param name="composerTransformation">
        /// A function that customizes a given <see cref="ICustomizationComposer{T}"/> and returns
        /// the modified composer.
        /// </param>
        public void Customize<T>(Func<ICustomizationComposer<T>, ISpecimenBuilderComposer> composerTransformation)
        {
            var c = composerTransformation(new Composer<T>().WithAutoProperties(this.EnableAutoProperties));
            this.customizer.Builders.Add(c.Compose());
        }

        /// <summary>
        /// Invokes the supplied action with an anonymous parameter value.
        /// </summary>
        /// <typeparam name="T">The type of the anonymous parameter.</typeparam>
        /// <param name="action">The action to invoke.</param>
        public void Do<T>(Action<T> action)
        {
            T x = this.CreateAnonymous<T>();
            action(x);
        }

        /// <summary>
        /// Invokes the supplied action with anonymous parameter values.
        /// </summary>
        /// <typeparam name="T1">The type of the first anonymous parameter.</typeparam>
        /// <typeparam name="T2">The type of the second anonymous parameter.</typeparam>
        /// <param name="action">The action to invoke.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "1", Justification = "This naming follows the convention of Action<T1, T2>, of which it is just a thin wrapper.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "2", Justification = "This naming follows the convention of Action<T1, T2>, of which it is just a thin wrapper.")]
        public void Do<T1, T2>(Action<T1, T2> action)
        {
            T1 x1 = this.CreateAnonymous<T1>();
            T2 x2 = this.CreateAnonymous<T2>();
            action(x1, x2);
        }

        /// <summary>
        /// Invokes the supplied action with anonymous parameter values.
        /// </summary>
        /// <typeparam name="T1">The type of the first anonymous parameter.</typeparam>
        /// <typeparam name="T2">The type of the second anonymous parameter.</typeparam>
        /// <typeparam name="T3">The type of the third anonymous parameter.</typeparam>
        /// <param name="action">The action to invoke.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "1", Justification = "This naming follows the convention of Action<T1, T2, T3>, of which it is just a thin wrapper.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "2", Justification = "This naming follows the convention of Action<T1, T2, T3>, of which it is just a thin wrapper.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "3", Justification = "This naming follows the convention of Action<T1, T2, T3>, of which it is just a thin wrapper.")]
        public void Do<T1, T2, T3>(Action<T1, T2, T3> action)
        {
            T1 x1 = this.CreateAnonymous<T1>();
            T2 x2 = this.CreateAnonymous<T2>();
            T3 x3 = this.CreateAnonymous<T3>();
            action(x1, x2, x3);
        }

        /// <summary>
        /// Invokes the supplied action with anonymous parameter values.
        /// </summary>
        /// <typeparam name="T1">The type of the first anonymous parameter.</typeparam>
        /// <typeparam name="T2">The type of the second anonymous parameter.</typeparam>
        /// <typeparam name="T3">The type of the third anonymous parameter.</typeparam>
        /// <typeparam name="T4">The type of the fourth anonymous parameter.</typeparam>
        /// <param name="action">The action to invoke.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "1", Justification = "This naming follows the convention of Action<T1, T2, T3, T4>, of which it is just a thin wrapper.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "2", Justification = "This naming follows the convention of Action<T1, T2, T3, T4>, of which it is just a thin wrapper.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "3", Justification = "This naming follows the convention of Action<T1, T2, T3, T4>, of which it is just a thin wrapper.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "4", Justification = "This naming follows the convention of Action<T1, T2, T3, T4>, of which it is just a thin wrapper.")]
        public void Do<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action)
        {
            T1 x1 = this.CreateAnonymous<T1>();
            T2 x2 = this.CreateAnonymous<T2>();
            T3 x3 = this.CreateAnonymous<T3>();
            T4 x4 = this.CreateAnonymous<T4>();
            action(x1, x2, x3, x4);
        }

        /// <summary>
        /// Freezes the type to a single value.
        /// </summary>
        /// <typeparam name="T">The type to freeze.</typeparam>
        /// <returns>
        /// The value that will subsequently always be created for <typeparamref name="T"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <see cref="Freeze()"/> method freezes the type to always return the same instance
        /// whenever an instance of the type is requested either directly, or indirectly as a
        /// nested value of other types.
        /// </para>
        /// </remarks>
        /// <seealso cref="Freeze{T}(T)"/>
        /// <seealso cref="Freeze{T}(Func{LatentObjectBuilder{T}, ObjectBuilder{T}})"/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the FxCop DL.")]
        public T Freeze<T>()
        {
            var value = this.CreateAnonymous<T>();
            return this.FreezeValue<T>(value);
        }

        /// <summary>
        /// Freezes the type to a single value.
        /// </summary>
        /// <typeparam name="T">The type to freeze.</typeparam>
        /// <param name="seed">
        /// Any data that adds additional information when creating the anonymous object.
        /// </param>
        /// <returns>
        /// The value that will subsequently always be created for <typeparamref name="T"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <see cref="Freeze{T}(T)"/> method freezes the type to always return the same
        /// instance whenever an instance of the type is requested either directly, or indirectly
        /// as a nested value of other types.
        /// </para>
        /// </remarks>
        /// <seealso cref="Freeze{T}()"/>
        /// <seealso cref="Freeze{T}(Func{LatentObjectBuilder{T}, ObjectBuilder{T}})"/>
        public T Freeze<T>(T seed)
        {
            var value = this.CreateAnonymous<T>(seed);
            return this.FreezeValue<T>(value);
        }

        /// <summary>
        /// Freezes the type to a single value.
        /// </summary>
        /// <typeparam name="T">The type to freeze.</typeparam>
        /// <param name="composerTransformation">
        /// A function that customizes a given <see cref="ICustomizationComposer{T}"/> and returns
        /// the modified composer.
        /// </param>
        /// <returns>
        /// The value that will subsequently always be created for <typeparamref name="T"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <see cref="Freeze{T}(T)"/> method freezes the type to always return the same
        /// instance whenever an instance of the type is requested either directly, or indirectly
        /// as a nested value of other types. The frozen instance is created by an
        /// <see cref="ObjectBuilder{T}"/> that is the result of applying the
        /// <paramref name="composerTransformation"/>.
        /// </para>
        /// </remarks>
        /// <seealso cref="Freeze{T}()"/>
        /// <seealso cref="Freeze{T}(T)"/>
        public T Freeze<T>(Func<ICustomizationComposer<T>, ISpecimenBuilderComposer> composerTransformation)
        {
            var c = this.Build<T>();
            var value = composerTransformation(c).CreateAnonymous<T>();
            return this.FreezeValue(value);
        }

        /// <summary>
        /// Invokes the supplied function with an anonymous parameter value and returns the result.
        /// </summary>
        /// <typeparam name="T">The type of the anonymous parameter.</typeparam>
        /// <typeparam name="TResult">The return type.</typeparam>
        /// <param name="function">The function to invoke.</param>
        /// <returns>The return value of <paramref name="function"/>.</returns>
        public TResult Get<T, TResult>(Func<T, TResult> function)
        {
            T x = this.CreateAnonymous<T>();
            return function(x);
        }

        /// <summary>
        /// Invokes the supplied function with anonymous parameter values and returns the result.
        /// </summary>
        /// <typeparam name="T1">The type of the first anonymous parameter.</typeparam>
        /// <typeparam name="T2">The type of the second anonymous parameter.</typeparam>
        /// <typeparam name="TResult">The return type.</typeparam>
        /// <param name="function">The function to invoke.</param>
        /// <returns>The return value of <paramref name="function"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "1", Justification = "This naming follows the convention of Func<T1, T2, TResult>, of which it is just a thin wrapper.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "2", Justification = "This naming follows the convention of Func<T1, T2, TResult>, of which it is just a thin wrapper.")]
        public TResult Get<T1, T2, TResult>(Func<T1, T2, TResult> function)
        {
            T1 x1 = this.CreateAnonymous<T1>();
            T2 x2 = this.CreateAnonymous<T2>();
            return function(x1, x2);
        }

        /// <summary>
        /// Invokes the supplied function with anonymous parameter values and returns the result.
        /// </summary>
        /// <typeparam name="T1">The type of the first anonymous parameter.</typeparam>
        /// <typeparam name="T2">The type of the second anonymous parameter.</typeparam>
        /// <typeparam name="T3">The type of the third anonymous parameter.</typeparam>
        /// <typeparam name="TResult">The return type.</typeparam>
        /// <param name="function">The function to invoke.</param>
        /// <returns>The return value of <paramref name="function"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "1", Justification = "This naming follows the convention of Func<T1, T2, T3, TResult>, of which it is just a thin wrapper.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "2", Justification = "This naming follows the convention of Func<T1, T2, T3, TResult>, of which it is just a thin wrapper.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "3", Justification = "This naming follows the convention of Func<T1, T2, T3, TResult>, of which it is just a thin wrapper.")]
        public TResult Get<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> function)
        {
            T1 x1 = this.CreateAnonymous<T1>();
            T2 x2 = this.CreateAnonymous<T2>();
            T3 x3 = this.CreateAnonymous<T3>();
            return function(x1, x2, x3);
        }

        /// <summary>
        /// Invokes the supplied function with anonymous parameter values and returns the result.
        /// </summary>
        /// <typeparam name="T1">The type of the first anonymous parameter.</typeparam>
        /// <typeparam name="T2">The type of the second anonymous parameter.</typeparam>
        /// <typeparam name="T3">The type of the third anonymous parameter.</typeparam>
        /// <typeparam name="T4">The type of the fourth anonymous parameter.</typeparam>
        /// <typeparam name="TResult">The return type.</typeparam>
        /// <param name="function">The function to invoke.</param>
        /// <returns>The return value of <paramref name="function"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "1", Justification = "This naming follows the convention of Func<T1, T2, T3, T4, TResult>, of which it is just a thin wrapper.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "2", Justification = "This naming follows the convention of Func<T1, T2, T3, T4, TResult>, of which it is just a thin wrapper.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "3", Justification = "This naming follows the convention of Func<T1, T2, T3, T4, TResult>, of which it is just a thin wrapper.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "4", Justification = "This naming follows the convention of Func<T1, T2, T3, T4, TResult>, of which it is just a thin wrapper.")]
        public TResult Get<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> function)
        {
            T1 x1 = this.CreateAnonymous<T1>();
            T2 x2 = this.CreateAnonymous<T2>();
            T3 x3 = this.CreateAnonymous<T3>();
            T4 x4 = this.CreateAnonymous<T4>();
            return function(x1, x2, x3, x4);
        }

        /// <summary>
        /// Registers a specific instance for a specific type.
        /// </summary>
        /// <typeparam name="T">
        /// The type for which <paramref name="item"/> should be registered.
        /// </typeparam>
        /// <param name="item">The item to register.</param>
        public void Register<T>(T item)
        {
            this.Customize<T>(c => c.FromFactory(() => item).OmitAutoProperties());
        }

        /// <summary>
        /// Registers a creation function for a specifc type.
        /// </summary>
        /// <typeparam name="T">
        /// The type for which <paramref name="creator"/> should be registered.
        /// </typeparam>
        /// <param name="creator">
        /// A function that will be used to create objects of type <typeparamref name="T"/> every
        /// time the <see cref="Fixture"/> is asked to create an object of that type.
        /// </param>
        public void Register<T>(Func<T> creator)
        {
            //this.Customize<T>(ob => ob.WithConstructor(creator).OmitAutoProperties());
            this.Customize<T>(c => c.FromFactory(creator).OmitAutoProperties());
        }

        /// <summary>
        /// Registers a creation function for a specific type, when that creation function requires
        /// a single input parameter.
        /// </summary>
        /// <typeparam name="TInput">
        /// The type of the input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="T">
        /// The type for which <paramref name="creator"/> should be registered.
        /// </typeparam>
        /// <param name="creator">
        /// A function that will be used to create objects of type <typeparamref name="T"/> every
        /// time the <see cref="Fixture"/> is asked to create an object of that type.
        /// </param>
        public void Register<TInput, T>(Func<TInput, T> creator)
        {
            //this.Customize<T>(ob => ob.WithConstructor<TInput>(creator).OmitAutoProperties());
            this.Customize<T>(c => c.FromFactory(creator).OmitAutoProperties());
        }

        /// <summary>
        /// Registers a creation function for a specific type, when that creation function requires
        /// two input parameters.
        /// </summary>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="T">
        /// The type for which <paramref name="creator"/> should be registered.
        /// </typeparam>
        /// <param name="creator">
        /// A function that will be used to create objects of type <typeparamref name="T"/> every
        /// time the <see cref="Fixture"/> is asked to create an object of that type.
        /// </param>
        public void Register<TInput1, TInput2, T>(Func<TInput1, TInput2, T> creator)
        {
            //this.Customize<T>(ob => ob.WithConstructor<TInput1, TInput2>(creator).OmitAutoProperties());
            this.Customize<T>(c => c.FromFactory(creator).OmitAutoProperties());
        }

        /// <summary>
        /// Registers a creation function for a specific type, when that creation function requires
        /// three input parameters.
        /// </summary>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="TInput3">
        /// The type of the third input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="T">
        /// The type for which <paramref name="creator"/> should be registered.
        /// </typeparam>
        /// <param name="creator">
        /// A function that will be used to create objects of type <typeparamref name="T"/> every
        /// time the <see cref="Fixture"/> is asked to create an object of that type.
        /// </param>
        public void Register<TInput1, TInput2, TInput3, T>(Func<TInput1, TInput2, TInput3, T> creator)
        {
            //this.Customize<T>(ob => ob.WithConstructor<TInput1, TInput2, TInput3>(creator).OmitAutoProperties());
            this.Customize<T>(c => c.FromFactory(creator).OmitAutoProperties());
        }

        /// <summary>
        /// Registers a creation function for a specific type, when that creation function requires
        /// four input parameters.
        /// </summary>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="TInput3">
        /// The type of the third input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="TInput4">
        /// The type of the fourth input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="T">
        /// The type for which <paramref name="creator"/> should be registered.
        /// </typeparam>
        /// <param name="creator">
        /// A function that will be used to create objects of type <typeparamref name="T"/> every
        /// time the <see cref="Fixture"/> is asked to create an object of that type.
        /// </param>
        public void Register<TInput1, TInput2, TInput3, TInput4, T>(Func<TInput1, TInput2, TInput3, TInput4, T> creator)
        {
            //this.Customize<T>(ob => ob.WithConstructor<TInput1, TInput2, TInput3, TInput4>(creator).OmitAutoProperties());
            this.Customize<T>(c => c.FromFactory(creator).OmitAutoProperties());
        }

        /// <summary>
        /// Repeats a function many times.
        /// </summary>
        /// <typeparam name="T">
        /// The type of object that <paramref name="function"/> creates.
        /// </typeparam>
        /// <param name="function">
        /// A function that creates an instance of <typeparamref name="T"/>.
        /// </param>
        /// <returns>A sequence of objects created by <paramref name="function"/>.</returns>
        /// <remarks>
        /// <para>
        /// The number of times <paramref name="function"/> is invoked is determined by
        /// <see cref="RepeatCount"/>.
        /// </para>
        /// </remarks>
        public IEnumerable<T> Repeat<T>(Func<T> function)
        {
            return function.Repeat(this.RepeatCount);
        }

        #region ISpecimenBuilderComposer Members

        public ISpecimenBuilder Compose()
        {
            var builder = this.Engine;

            if (this.EnableAutoProperties)
            {
                builder = new Postprocessor(
                    builder,
                    new AutoPropertiesCommand().Execute,
                    new AnyTypeSpecification());
            }

            return new CompositeSpecimenBuilder(
                this.customizer,
                builder,
                this.residueCollector,
                new TerminatingSpecimenBuilder());
        }

        #endregion

        private bool EnableAutoProperties
        {
            get { return !this.OmitAutoProperties; }
        }

        private T FreezeValue<T>(T value)
        {
            this.Register(value);
            return value;
        }
    }
}

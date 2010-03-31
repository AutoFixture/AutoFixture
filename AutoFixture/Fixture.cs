using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Provides anonymous object creation service.
    /// </summary>
    public class Fixture
    {
        private const int manyEquivalence = 3;

        private readonly Dictionary<Type, Func<object, object>> typeMappings;

        /// <summary>
        /// Initializes a new instance of the <see cref="Fixture"/> class.
        /// </summary>
        public Fixture()
        {
            this.typeMappings = new Dictionary<Type, Func<object, object>>();
            this.typeMappings[typeof(bool)] = new BooleanSwitch().CreateAnonymous;
            this.typeMappings[typeof(byte)] = new ByteSequenceGenerator().CreateAnonymous;
            this.typeMappings[typeof(sbyte)] = new SByteSequenceGenerator().CreateAnonymous;
            this.typeMappings[typeof(ushort)] = new UInt16SequenceGenerator().CreateAnonymous;
            this.typeMappings[typeof(short)] = new Int16SequenceGenerator().CreateAnonymous;
            this.typeMappings[typeof(uint)] = new UInt32SequenceGenerator().CreateAnonymous;
            this.typeMappings[typeof(int)] = new Int32SequenceGenerator().CreateAnonymous;
            this.typeMappings[typeof(ulong)] = new UInt64SequenceGenerator().CreateAnonymous;
            this.typeMappings[typeof(long)] = new Int64SequenceGenerator().CreateAnonymous;
            this.typeMappings[typeof(decimal)] = new DecimalSequenceGenerator().CreateAnonymous;
            this.typeMappings[typeof(float)] = new SingleSequenceGenerator().CreateAnonymous;
            this.typeMappings[typeof(double)] = new DoubleSequenceGenerator().CreateAnonymous;
            this.typeMappings[typeof(Guid)] = GuidGenerator.CreateAnonymous;
            this.typeMappings[typeof(DateTime)] = seed => DateTime.Now;
            this.typeMappings[typeof(string)] = GuidStringGenerator.CreateAnonymous;

            this.RepeatCount = Fixture.manyEquivalence;
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
        public int RepeatCount { get; set; }

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
        /// Gets a dictionary of type mappings that defines how objects of specific types will be
        /// created.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use the Customize method instead of the TypeMappings dictionary.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "The alternative design is to define an interface that mimics Func<object, object>, and expose a dictionary of Type and this interface. That would be a more heavy-weight solution that doesn't add a lot of clarity or value.")]
        public IDictionary<Type, Func<object, object>> TypeMappings
        {
            get { return this.typeMappings; }
        }

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
        /// A <see cref="LatentObjectBuilder{T}"/> that can be used to customize the creation
        /// algorithm before creating the object.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the FxCop DL.")]
        public LatentObjectBuilder<T> Build<T>()
        {
            return this.CreateLatentObjectBuilder<T>();
        }

        /// <summary>
        /// Creates an anonymous object.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <returns>An anonymous object of type <typeparamref name="T"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the FxCop DL.")]
        public T CreateAnonymous<T>()
        {
            return this.CreateAnonymous<T>(default(T));
        }

        /// <summary>
        /// Creates an anonymous object, potentially using the supplied seed as additional
        /// information when creating the object.
        /// </summary>
        /// <param name="seed">
        /// Any data that adds additional information when creating the anonymous object.
        /// </param>
        /// <returns>An anonymous object.</returns>
        public virtual T CreateAnonymous<T>(T seed)
        {
            return new CustomizedObjectFactory(this.typeMappings, new ThrowingRecursionHandler(), this.RepeatCount, this.OmitAutoProperties, this.Resolver).CreateAnonymous(seed);
        }

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <returns>A sequence of anonymous object of type <typeparamref name="T"/>.</returns>
        /// <remarks>
        /// <para>
        /// The number of objects created is determined by <see cref="RepeatCount"/>.
        /// </para>
        /// </remarks>
        /// <seealso cref="CreateMany{T}(int)"/>
        /// <seealso cref="CreateMany{T}(T)"/>
        /// <seealso cref="CreateMany{T}(T, int)"/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the FxCop DL.")]
        public IEnumerable<T> CreateMany<T>()
        {
            return this.CreateMany<T>(this.RepeatCount);
        }

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <param name="seed">
        /// An initial value that may or may not be used as input for the algorithm creating the
        /// return value.
        /// </param>
        /// <returns>A sequence of anonymous object of type <typeparamref name="T"/>.</returns>
        /// <seealso cref="CreateMany{T}()"/>
        /// <seealso cref="CreateMany{T}(int)"/>
        /// <seealso cref="CreateMany{T}(T, int)"/>
        public IEnumerable<T> CreateMany<T>(T seed)
        {
            return this.CreateMany(seed, this.RepeatCount);
        }

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <param name="repeatCount">The number of objects to create.</param>
        /// <returns>A sequence of anonymous objects of type <typeparamref name="T"/>.</returns>
        /// <seealso cref="CreateMany{T}()"/>
        /// <seealso cref="CreateMany{T}(T)"/>
        /// <seealso cref="CreateMany{T}(T, int)"/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the FxCop DL.")]
        public IEnumerable<T> CreateMany<T>(int repeatCount)
        {
            return this.CreateMany(default(T), repeatCount);
        }

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <param name="seed">
        /// An initial value that may or may not be used as input for the algorithm creating the
        /// return value.
        /// </param>
        /// <param name="repeatCount">The number of objects to create.</param>
        /// <returns>A sequence of anonymous objects of type <typeparamref name="T"/>.</returns>
        /// <seealso cref="CreateMany{T}()"/>
        /// <seealso cref="CreateMany{T}(T)"/>
        /// <seealso cref="CreateMany{T}(int)"/>
        public virtual IEnumerable<T> CreateMany<T>(T seed, int repeatCount)
        {
            Func<T> f = () => this.CreateAnonymous(seed);
            return f.Repeat(repeatCount);
        }

        /// <summary>
        /// Customizes the creation algorithm for all objects of a given type.
        /// </summary>
        /// <typeparam name="T">The type of object to customize.</typeparam>
        /// <param name="builderTransform">
        /// A function that customizes a given <see cref="ObjectBuilder{T}"/> and returns the
        /// modified  builder.
        /// </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "The alternative design is to define an interface that mimics Func<ObjectBuilder<T>, ObjectBuilder<T>>, and expose this interface as a parameter. That would be a more heavy-weight solution that doesn't add a lot of clarity or value.")]
        public void Customize<T>(Func<LatentObjectBuilder<T>, ObjectBuilder<T>> builderTransform)
        {
            var b = this.CreateLatentObjectBuilder<T>();
            this.TypeMappings[typeof(T)] = x => builderTransform(b).CreateAnonymous();
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
        /// <param name="builderTransform">
        /// A function that customizes a given <see cref="ObjectBuilder{T}"/> and returns the
        /// modified  builder.
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
        /// <paramref name="builderTransform"/>.
        /// </para>
        /// </remarks>
        /// <seealso cref="Freeze{T}()"/>
        /// <seealso cref="Freeze{T}(T)"/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "The alternative design is to define an interface that mimics Func<ObjectBuilder<T>, ObjectBuilder<T>>, and expose this interface as a parameter. That would be a more heavy-weight solution that doesn't add a lot of clarity or value.")]
        public T Freeze<T>(Func<LatentObjectBuilder<T>, ObjectBuilder<T>> builderTransform)
        {
            var b = this.CreateLatentObjectBuilder<T>();
            var value = builderTransform(b).CreateAnonymous();
            return this.FreezeValue<T>(value);
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
            this.Customize<T>(ob => ob.WithConstructor(() => item).OmitAutoProperties());
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
            this.Customize<T>(ob => ob.WithConstructor(creator).OmitAutoProperties());
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
            this.Customize<T>(ob => ob.WithConstructor<TInput>(creator).OmitAutoProperties());
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
            this.Customize<T>(ob => ob.WithConstructor<TInput1, TInput2>(creator).OmitAutoProperties());
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
            this.Customize<T>(ob => ob.WithConstructor<TInput1, TInput2, TInput3>(creator).OmitAutoProperties());
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
            this.Customize<T>(ob => ob.WithConstructor<TInput1, TInput2, TInput3, TInput4>(creator).OmitAutoProperties());
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

        private LatentObjectBuilder<T> CreateLatentObjectBuilder<T>()
        {
            return new LatentObjectBuilder<T>(this.typeMappings, new ThrowingRecursionHandler(), this.RepeatCount, this.OmitAutoProperties, this.Resolver);
        }

        private T FreezeValue<T>(T value)
        {
            this.Register(value);
            return value;
        }
    }
}

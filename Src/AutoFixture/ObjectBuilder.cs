using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Customizes and creates anonymous object instances of a given type.
    /// </summary>
    /// <typeparam name="T">The type of object to create.</typeparam>
    public abstract class ObjectBuilder<T> : IBuilder
    {
        private readonly List<MemberAnnotatedAction<T>> actions;
        private readonly CustomizedObjectFactory customizedFactory;
        private bool omitAutoProperties;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectBuilder{T}"/> class.
        /// </summary>
        /// <param name="typeMappings">
        /// A dictionary of type mappings that defines how objects of specific types will be
        /// created.
        /// </param>
        /// <param name="recursionHandler">
        /// A special recursion monitor that will keep track of created objects and prevent
        /// endless recursion loops when generating objects.
        /// </param>
        /// <param name="repeatCount">
        /// A number that controls how many objects are created when an
        /// <see cref="ObjectBuilder{T}"/> creates more than one anonymous object.
        /// </param>
        /// <param name="omitAutoProperties">
        /// Indicates whether writable properties should be assigned a value or not. 
        /// The setting can be overridden by <see cref="OmitAutoProperties"/> and 
        /// <see cref="WithAutoProperties"/>.
        /// </param>
        /// <param name="resolveCallback">
        /// A callback that can be invoked to resolve unresolved types.
        /// </param>
        protected ObjectBuilder(IDictionary<Type, Func<object, object>> typeMappings, RecursionHandler recursionHandler, int repeatCount, bool omitAutoProperties, Func<Type, object> resolveCallback)
        {
            if (typeMappings == null)
            {
                throw new ArgumentNullException("typeMappings");
            }

            this.omitAutoProperties = omitAutoProperties;
            this.customizedFactory = new CustomizedObjectFactory(typeMappings, recursionHandler, repeatCount, omitAutoProperties, ObjectBuilder<T>.EnsureResolveCallback(resolveCallback));
            this.actions = new List<MemberAnnotatedAction<T>>();
        }

        /// <summary>
        /// Creates an anonymous object.
        /// </summary>
        /// <returns>An anonymous object.</returns>
        public T CreateAnonymous()
        {
            return this.CreateAnonymous(default(T));
        }

        /// <summary>
        /// Creates an anonymous object with a seed.
        /// </summary>
        /// <param name="seed">
        /// An initial value that may or may not be used as input for the algorithm creating the
        /// return value.
        /// </param>
        /// <returns>An anonymous object.</returns>
        public T CreateAnonymous(T seed)
        {
            T createdObject = this.Create(seed);

            this.actions.Concat(this.GetNonOverriddenMembers()).ToList()
                .ForEach(a => a.Action(createdObject));

            return createdObject;
        }

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <returns>A sequence of anonymous objects.</returns>
        /// <seealso cref="CreateMany(T)"/>
        /// <seealso cref="CreateMany(int)"/>
        /// <seealso cref="CreateMany(T, int)"/>
        public IEnumerable<T> CreateMany()
        {
            return this.CreateMany(this.CustomizedFactory.RepeatCount);
        }

        /// <summary>
        /// Creates many anonymous objects based on a seed.
        /// </summary>
        /// <param name="seed">
        /// An initial value that may or may not be used as input for the algorithm creating the
        /// return value.
        /// </param>
        /// <returns>A sequence of anonymous object.</returns>
        /// <seealso cref="CreateMany()"/>
        /// <seealso cref="CreateMany(int)"/>
        /// <seealso cref="CreateMany(T, int)"/>
        public IEnumerable<T> CreateMany(T seed)
        {
            return this.CreateMany(seed, this.CustomizedFactory.RepeatCount);
        }

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <param name="repeatCount">The number of objects to create.</param>
        /// <returns>A sequence of anonymous objects.</returns>
        /// <seealso cref="CreateMany()"/>
        /// <seealso cref="CreateMany(T)"/>
        /// <seealso cref="CreateMany(T, int)"/>
        public IEnumerable<T> CreateMany(int repeatCount)
        {
            return ((Func<T>)this.CreateAnonymous).Repeat(repeatCount);
        }

        /// <summary>
        /// Creates many anonymous objects based on a seed.
        /// </summary>
        /// <param name="seed">
        /// An initial value that may or may not be used as input for the algorithm creating the
        /// return value.
        /// </param>
        /// <param name="repeatCount">The number of objects to create.</param>
        /// <returns>A sequence of anonymous objects.</returns>
        /// <seealso cref="CreateMany()"/>
        /// <seealso cref="CreateMany(T)"/>
        /// <seealso cref="CreateMany(int)"/>
        public IEnumerable<T> CreateMany(T seed, int repeatCount)
        {
            Func<T> f = () => this.CreateAnonymous(seed);
            return f.Repeat(repeatCount);
        }

        /// <summary>
        /// Performs the specified action on the object being built.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>
        /// An <see cref="ObjectBuilder{T}"/> that can be used to create anonymous objects, or
        /// further customize the creation algorithm.
        /// </returns>
        public ObjectBuilder<T> Do(Action<T> action)
        {
            var clone = this.Clone();
            clone.actions.Add(new MemberAnnotatedAction<T>(action));
            return clone;
        }

        /// <summary>
        /// Instructs the <see cref="ObjectBuilder{T}"/> to not assign values to writable properties
        /// when subsequently creating an anonymous object.
        /// </summary>
        /// <returns>
        /// An <see cref="ObjectBuilder{T}"/> that can be used to create anonymous objects, or
        /// further customize the creation algorithm.
        /// </returns>
        public ObjectBuilder<T> OmitAutoProperties()
        {
            var clone = this.Clone();
            clone.omitAutoProperties = true;
            return clone;
        }

        /// <summary>
        /// Instructs the <see cref="ObjectBuilder{T}"/> to assign values to writable properties
        /// when subsequently creating an anonymous object. This is the default behavior.
        /// </summary>
        /// <returns>
        /// An <see cref="ObjectBuilder{T}"/> that can be used to create anonymous objects, or
        /// further customize the creation algorithm.
        /// </returns>
        public ObjectBuilder<T> WithAutoProperties()
        {
            var clone = this.Clone();
            clone.omitAutoProperties = false;
            return clone;
        }

        /// <summary>
        /// Registers that a writable property or field should be assigned a value when an object
        /// is subsequently created.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property or field.</typeparam>
        /// <param name="propertyPicker">
        /// An expression that identifies the property or field that will should have a value
        /// assigned.
        /// </param>
        /// <returns>
        /// An <see cref="ObjectBuilder{T}"/> that can be used to create anonymous objects, or
        /// further customize the creation algorithm.
        /// </returns>
        /// <remarks>
        /// <para>
        /// While the default behavior is that writable properties or fields are automatically
        /// assigned values, <see cref="OmitAutoProperties"/> can change this behavior. In this
        /// case, the With method offers an explicit opt-in functionality. When used without
        /// OmitAutoProperties, this With overload has no particular effect, since a value would
        /// have been assigned none the less.
        /// </para>
        /// </remarks>
        /// <seealso cref="OmitAutoProperties"/>
        /// <seealso cref="With{TProperty}(Expression{Func{T, TProperty}}, TProperty)"/>
        /// <seealso cref="Without"/>
        public ObjectBuilder<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            TProperty value = this.CustomizedFactory.CreateAnonymous(default(TProperty));
            return this.With(propertyPicker, value);
        }

        /// <summary>
        /// Registers that a writable property or field should be assigned a specific value when an
        /// object subsequently is created.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property or field to assign.</typeparam>
        /// <param name="propertyPicker">
        /// An expression that identifies the property or field that will have
        /// <paramref name="value"/> assigned.
        /// </param>
        /// <param name="value">
        /// The value to assign to the property or field identified by
        /// <paramref name="propertyPicker"/>.
        /// </param>
        /// <returns>
        /// An <see cref="ObjectBuilder{T}"/> that can be used to create anonymous objects, or
        /// further customize the creation algorithm.
        /// </returns>
        /// <seealso cref="With{TProperty}(Expression{Func{T, TProperty}})"/>
        /// <seealso cref="Without"/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Although not used in the method body per se, Expression<TDelegate> is used to constrain the caller.")]
        public ObjectBuilder<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, TProperty value)
        {
            MemberExpression me = (MemberExpression)propertyPicker.Body;
            var action = AccessorFactory.Create(me.Member).CreateAssignment((t, s) => value).ToAnnotatedAction<T>();

            var clone = this.Clone();
            clone.actions.Add(action);
            return clone;
        }

        /// <summary>
        /// Registers that a writable property should not be assigned any automatic value when an
        /// object is subsequently created.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property or field to ignore.</typeparam>
        /// <param name="propertyPicker">
        /// An expression that identifies the property or field to be ignored.
        /// </param>
        /// <returns>
        /// An <see cref="ObjectBuilder{T}"/> that can be used to create anonymous objects, or
        /// further customize the creation algorithm.
        /// </returns>
        /// <seealso cref="With{TProperty}(Expression{Func{T, TProperty}})"/>
        /// <seealso cref="With{TProperty}(Expression{Func{T, TProperty}}, TProperty)"/>
        /// <seealso cref="OmitAutoProperties"/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "The explicit use of Expression<Func<T>> enables type inference from the test code. With only the base class LambdaExpression, calling code would have to explicitly spell out the property type as a generic type parameter. This would hurt readability of the calling code.")]
        public ObjectBuilder<T> Without<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            MemberExpression me = (MemberExpression)propertyPicker.Body;
            var action = new NullAccessor(me.Member).ToAnnotatedAction<T>();

            var clone = this.Clone();
            clone.actions.Add(action);
            return clone;
        }

        /// <summary>
        /// Registers a specific recursion handler to use when creating subsequent instances.
        /// </summary>
        /// <param name="recursionHandler">The recursion handler.</param>
        /// <returns>
        /// An <see cref="ObjectBuilder{T}"/> that can be used to create anonymous objects, or
        /// further customize the creation algorithm.
        /// </returns>
        public ObjectBuilder<T> UsingRecursionHandler(RecursionHandler recursionHandler)
        {
            var clone = this.Clone();
            clone.CustomizedFactory.SetRecursionHandler(recursionHandler);
            return clone;
        }

        #region IBuilder Members

        object IBuilder.Create(object seed)
        {
            if (seed == null)
            {
                return this.CreateAnonymous();
            }
            return this.CreateAnonymous((T)seed);
        }

        #endregion

        /// <summary>
        /// Returns a new instance with the same properties as the current instance.
        /// </summary>
        /// <returns>A clone of the current instance.</returns>
        protected abstract ObjectBuilder<T> CloneCore();

        /// <summary>
        /// Creates an anonymous object.
        /// </summary>
        /// <returns>An anonymous object.</returns>
        protected abstract T Create(T seed);

        internal CustomizedObjectFactory CustomizedFactory
        {
            get { return this.customizedFactory; }
        }

        private ObjectBuilder<T> Clone()
        {
            var clone = this.CloneCore();
            clone.actions.AddRange(this.actions);
            clone.omitAutoProperties = this.omitAutoProperties;
            return clone;
        }

        private static Func<Type, object> EnsureResolveCallback(Func<Type, object> resolveCallback)
        {
            if (resolveCallback == null)
            {
                return t => null;
            }
            return resolveCallback;
        }

        private IEnumerable<MemberAnnotatedAction<T>> GetNonOverriddenMembers()
        {
            if (this.omitAutoProperties)
            {
                return Enumerable.Empty<MemberAnnotatedAction<T>>();
            }

            return from m in typeof(T).GetMembers()
                   where (m is PropertyInfo || m is FieldInfo)
                   && !(from aa in this.actions
                        select aa.Member).Contains(m, new MemberInfoNameComparer())
                   select this.CustomizedFactory.CreateAssigment(m).ToAnnotatedAction<T>();
        }
    }
}

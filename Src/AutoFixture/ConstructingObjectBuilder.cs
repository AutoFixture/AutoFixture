using System;
using System.Collections.Generic;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates object by returning objects based on a function supplied by an external caller.
    /// </summary>
    /// <typeparam name="T">The type of object to create.</typeparam>
    public class ConstructingObjectBuilder<T> : ObjectBuilder<T>
    {
        private readonly Func<T, T> creator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructingObjectBuilder{T}"/> class.
        /// </summary>
        /// <param name="typeMappings">
        /// A dictionary of type mappings that defines how objects of specific types will be
        /// created.
        /// </param>
        /// <param name="recursionHandler">
        /// A special recursion monitor that will keep track of created objects and prevent
        /// endless recursion loops when generating objects.
        /// </param>
        /// <param name="creator">
        /// The function that defines how <see cref="Create"/> will create an object of the
        /// requested type.
        /// </param>
        /// <param name="repeatCount">
        /// A number that controls how many objects are created when an
        /// <see cref="ObjectBuilder{T}"/> creates more than one anonymous object.
        /// </param>
        /// <param name="omitAutoProperties">
        /// Indicates whether writable properties should be assigned a value or not. 
        /// The setting can be overridden by <see cref="ObjectBuilder{T}.OmitAutoProperties"/> and 
        /// <see cref="ObjectBuilder{T}.WithAutoProperties"/>.
        /// </param>
        /// <param name="resolveCallback">
        /// A callback that can be invoked to resolve unresolved types.
        /// </param>
        public ConstructingObjectBuilder(IDictionary<Type, Func<object, object>> typeMappings, RecursionHandler recursionHandler, int repeatCount, bool omitAutoProperties, Func<Type, object> resolveCallback, Func<T, T> creator)
            : base(typeMappings, recursionHandler, repeatCount, omitAutoProperties, resolveCallback)
        {
            this.creator = creator;
        }

        /// <summary>
        /// Returns a new instance with the same properties as the current instance.
        /// </summary>
        /// <returns>A clone of the current instance.</returns>
        protected override ObjectBuilder<T> CloneCore()
        {
            return this.CustomizedFactory.CreateConstructingBuilder<T>(this.creator);
        }

        /// <summary>
        /// Returns an object created by the function supplied through the instance's constructor.
        /// </summary>
        /// <returns>
        /// An object created by the function that was supplied through the instance's constructor
        /// </returns>
        protected override T Create(T seed)
        {
            return this.creator(seed);
        }
    }
}

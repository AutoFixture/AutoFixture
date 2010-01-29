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
        /// <param name="creator">
        /// The function that defines how <see cref="Create"/> will create an object of the
        /// requested type.
        /// </param>
        /// <param name="repeatCount">
        /// A number that controls how many objects are created when an
        /// <see cref="ObjectBuilder{T}"/> creates more than one anonymous object.
        /// </param>
        /// <param name="resolveCallback">
        /// A callback that can be invoked to resolve unresolved types.
        /// </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "The alternative design is to define an interface that mimics Func<object, object>, and expose a dictionary of Type and this interface. That would be a more heavy-weight solution that doesn't add a lot of clarity or value.")]
        public ConstructingObjectBuilder(IDictionary<Type, Func<object, object>> typeMappings, int repeatCount, Func<Type, object> resolveCallback, Func<T, T> creator)
            : base(typeMappings, repeatCount, resolveCallback)
        {
            this.creator = creator;
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

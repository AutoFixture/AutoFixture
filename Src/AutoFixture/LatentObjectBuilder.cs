using System;
using System.Collections.Generic;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates objects by invoking a specified constructor, possibly using anonymous constructor
    /// arguments.
    /// </summary>
    /// <typeparam name="T">The type of object to create.</typeparam>
    public class LatentObjectBuilder<T> : ObjectBuilder<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LatentObjectBuilder{T}"/> class.
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
        /// The setting can be overridden by <see cref="ObjectBuilder{T}.OmitAutoProperties"/> and 
        /// <see cref="ObjectBuilder{T}.WithAutoProperties"/>.
        /// </param>
        /// <param name="resolveCallback">
        /// A callback that can be invoked to resolve unresolved types.
        /// </param>
        public LatentObjectBuilder(IDictionary<Type, Func<object, object>> typeMappings, RecursionHandler recursionHandler, int repeatCount, bool omitAutoProperties, Func<Type, object> resolveCallback)
            : base(typeMappings, recursionHandler, repeatCount, omitAutoProperties, resolveCallback)
        {
        }

        /// <summary>
        /// Defines a creational function that creates anonymous variables based on a seed.
        /// </summary>
        /// <param name="creator">The function used to create anonymous variables.</param>
        /// <returns>
        /// A <see cref="ConstructingObjectBuilder{T}"/> that can be used to create anonymous
        /// objects, or further customize the creation algorithm.
        /// </returns>
        public ConstructingObjectBuilder<T> FromSeed(Func<T, T> creator)
        {
            if (creator == null)
            {
                throw new ArgumentNullException("creator");
            }

            return this.CustomizedFactory.CreateConstructingBuilder<T>(creator);
        }

        /// <summary>
        /// Specifies that an anonymous object should be created in a particular way; often by
        /// using a constructor.
        /// </summary>
        /// <param name="creator">
        /// A function that will be used to create the object. This will often be a constructor.
        /// </param>
        /// <returns>
        /// A <see cref="ConstructingObjectBuilder{T}"/> that can be used to create anonymous objects,
        /// or further customize the creation algorithm.
        /// </returns>
        public ConstructingObjectBuilder<T> WithConstructor(Func<T> creator)
        {
            return this.CustomizedFactory.CreateConstructingBuilder<T>(seed => creator());
        }

        /// <summary>
        /// Specifies that an anonymous object should be created in a particular way, using a
        /// single input parameter for the construction.
        /// </summary>
        /// <typeparam name="TInput">
        /// The type of input parameter to use when invoking <paramref name="creator"/>.
        /// </typeparam>
        /// <param name="creator">
        /// A function that will be used to create the object. This will often be a constructor
        /// that takes a single constructor argument of type <typeparamref name="TInput"/>.
        /// </param>
        /// <returns>
        /// A <see cref="ConstructingObjectBuilder{T}"/> that can be used to create anonymous objects,
        /// or further customize the creation algorithm.
        /// </returns>
        public ConstructingObjectBuilder<T> WithConstructor<TInput>(Func<TInput, T> creator)
        {
            Func<T, T> f = seed =>
                {
                    TInput p = this.CustomizedFactory.CreateAnonymous(default(TInput));
                    return creator(p);
                };

            return this.CustomizedFactory.CreateConstructingBuilder<T>(f);
        }

        /// <summary>
        /// Specifies that an anonymous object should be created in a particular way, using two
        /// input parameters for the construction.
        /// </summary>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter to use when invoking <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter to use when invoking <paramref name="creator"/>.
        /// </typeparam>
        /// <param name="creator">
        /// A function that will be used to create the object. This will often be a constructor
        /// that takes two constructor arguments of type <typeparamref name="TInput1"/> and
        /// <typeparamref name="TInput2"/>.
        /// </param>
        /// <returns>
        /// A <see cref="ConstructingObjectBuilder{T}"/> that can be used to create anonymous objects,
        /// or further customize the creation algorithm.
        /// </returns>
        public ConstructingObjectBuilder<T> WithConstructor<TInput1, TInput2>(Func<TInput1, TInput2, T> creator)
        {
            Func<T, T> f = seed =>
                {
                    TInput1 p1 = this.CustomizedFactory.CreateAnonymous(default(TInput1));
                    TInput2 p2 = this.CustomizedFactory.CreateAnonymous(default(TInput2));

                    return creator(p1, p2);
                };
            return this.CustomizedFactory.CreateConstructingBuilder<T>(f);
        }

        /// <summary>
        /// Specifies that an anonymous object should be created in a particular way, using three
        /// input parameters for the construction.
        /// </summary>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter to use when invoking <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter to use when invoking <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="TInput3">
        /// The type of the third input parameter to use when invoking <paramref name="creator"/>.
        /// </typeparam>
        /// <param name="creator">
        /// A function that will be used to create the object. This will often be a constructor
        /// that takes three constructor arguments of type <typeparamref name="TInput1"/>,
        /// <typeparamref name="TInput2"/> and <typeparamref name="TInput3"/>.
        /// </param>
        /// <returns>
        /// A <see cref="ConstructingObjectBuilder{T}"/> that can be used to create anonymous
        /// objects, or further customize the creation algorithm.
        /// </returns>
        public ConstructingObjectBuilder<T> WithConstructor<TInput1, TInput2, TInput3>(Func<TInput1, TInput2, TInput3, T> creator)
        {
            Func<T, T> f = seed =>
                {
                    TInput1 p1 = this.CustomizedFactory.CreateAnonymous(default(TInput1));
                    TInput2 p2 = this.CustomizedFactory.CreateAnonymous(default(TInput2));
                    TInput3 p3 = this.CustomizedFactory.CreateAnonymous(default(TInput3));

                    return creator(p1, p2, p3);
                };

            return this.CustomizedFactory.CreateConstructingBuilder<T>(f);
        }

        /// <summary>
        /// Specifies that an anonymous object should be created in a particular way, using four
        /// input parameters for the construction.
        /// </summary>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter to use when invoking <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter to use when invoking <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="TInput3">
        /// The type of the third input parameter to use when invoking <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="TInput4">
        /// The type of the fourth input parameter to use when invoking <paramref name="creator"/>.
        /// </typeparam>
        /// <param name="creator">
        /// A function that will be used to create the object. This will often be a constructor
        /// that takes three constructor arguments of type <typeparamref name="TInput1"/>,
        /// <typeparamref name="TInput2"/>, <typeparamref name="TInput3"/> and
        /// <typeparamref name="TInput4"/>.
        /// </param>
        /// <returns>
        /// A <see cref="ConstructingObjectBuilder{T}"/> that can be used to create anonymous
        /// objects, or further customize the creation algorithm.
        /// </returns>
        public ConstructingObjectBuilder<T> WithConstructor<TInput1, TInput2, TInput3, TInput4>(Func<TInput1, TInput2, TInput3, TInput4, T> creator)
        {
            Func<T, T> f = seed =>
                {
                    TInput1 p1 = this.CustomizedFactory.CreateAnonymous(default(TInput1));
                    TInput2 p2 = this.CustomizedFactory.CreateAnonymous(default(TInput2));
                    TInput3 p3 = this.CustomizedFactory.CreateAnonymous(default(TInput3));
                    TInput4 p4 = this.CustomizedFactory.CreateAnonymous(default(TInput4));

                    return creator(p1, p2, p3, p4);
                };
            return this.CustomizedFactory.CreateConstructingBuilder<T>(f);
        }

        /// <summary>
        /// Returns a new instance with the same properties as the current instance.
        /// </summary>
        /// <returns>A clone of the current instance.</returns>
        protected override ObjectBuilder<T> CloneCore()
        {
            return this.CustomizedFactory.CreateLatentBuilder<T>();
        }

        /// <summary>
        /// Creates an anonymous object.
        /// </summary>
        /// <returns>An anonymous object.</returns>
        protected override T Create(T seed)
        {
            return (T)this.CustomizedFactory.Construct(typeof(T));
        }
    }
}

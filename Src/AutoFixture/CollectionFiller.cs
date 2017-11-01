using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoFixture
{
    /// <summary>
    /// Contains extension methods for populating collections with specimens.
    /// </summary>
    public static class CollectionFiller
    {
        /// <summary>
        /// Adds many objects to a list.
        /// </summary>
        /// <typeparam name="T">The type of object contained within the list.</typeparam>
        /// <param name="collection">The collection to which objects should be added.</param>
        /// <param name="creator">
        /// A function that will be called many times to create objects that will be added to
        /// <paramref name="collection"/>.</param>
        /// <param name="repeatCount">
        /// The number of times <paramref name="creator"/> is invoked, and hence the number of
        /// items added to <paramref name="collection"/>.
        /// </param>
        public static void AddMany<T>(this ICollection<T> collection, Func<T> creator, int repeatCount)
        {
            Enumerable.Repeat(creator, repeatCount).ToList().ForEach(f => collection.Add(f()));
        }

        /// <summary>
        /// Adds many anonymously created objects to a list.
        /// </summary>
        /// <typeparam name="T">The type of object that is contained in the list.</typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="collection">
        /// The list to which the anonymously created objects will be added.
        /// </param>
        /// <remarks>
        /// <para>
        /// The number of objects created and added is determined by
        /// <see cref="IFixture.RepeatCount"/>.
        /// </para>
        /// </remarks>
        /// <seealso cref="AddManyTo{T}(IFixture, ICollection{T}, int)"/>
        /// <seealso cref="AddManyTo{T}(IFixture, ICollection{T}, Func{T})"/>
        public static void AddManyTo<T>(this IFixture fixture, ICollection<T> collection)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            fixture.AddManyTo(collection, fixture.Create<T>);
        }

        /// <summary>
        /// Adds many anonymously created objects to a list.
        /// </summary>
        /// <typeparam name="T">The type of object that is contained in the list.</typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="collection">
        /// The list to which the anonymously created objects will be added.
        /// </param>
        /// <param name="repeatCount">The number of objects created and added.</param>
        /// <seealso cref="AddManyTo{T}(IFixture, ICollection{T})"/>
        /// <seealso cref="AddManyTo{T}(IFixture, ICollection{T}, Func{T})"/>
        public static void AddManyTo<T>(this IFixture fixture, ICollection<T> collection, int repeatCount)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            collection.AddMany(fixture.Create<T>, repeatCount);
        }

        /// <summary>
        /// Adds many objects to a list using the provided function to create each object.
        /// </summary>
        /// <typeparam name="T">The type of object that is contained in the list.</typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="collection">
        /// The list to which the created objects will be added.
        /// </param>
        /// <param name="creator">
        /// The function that creates each object which is subsequently added to
        /// <paramref name="collection"/>.
        /// </param>
        /// <remarks>
        /// <para>
        /// The number of objects created and added is determined by
        /// <see cref="IFixture.RepeatCount"/>.
        /// </para>
        /// </remarks>
        /// <seealso cref="AddManyTo{T}(IFixture, ICollection{T})"/>
        /// <seealso cref="AddManyTo{T}(IFixture, ICollection{T}, int)"/>
        public static void AddManyTo<T>(this IFixture fixture, ICollection<T> collection, Func<T> creator)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            collection.AddMany(creator, fixture.RepeatCount);
        }
    }
}

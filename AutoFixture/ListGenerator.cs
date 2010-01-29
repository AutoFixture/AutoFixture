using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Contains extension methods that works with lists in a repeatable fashion.
    /// </summary>
    public static class ListGenerator
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
            creator.Repeat(repeatCount).ToList().ForEach(item => collection.Add(item));
        }

        /// <summary>
        /// Repeats a function a specified number of times and returns a sequence of the created
        /// objects.
        /// </summary>
        /// <typeparam name="T">
        /// The type of objects contained within in the returned sequence.
        /// </typeparam>
        /// <param name="function">
        /// A function that will be called many times to create objects that will be returned as a
        /// sequence.
        /// </param>
        /// <param name="repeatCount">
        /// The number of times <paramref name="function"/> is invoked, and hence the number of
        /// items returned.
        /// </param>
        /// <returns>
        /// A sequence of objects created by <paramref name="function"/>.
        /// </returns>
        public static IEnumerable<T> Repeat<T>(this Func<T> function, int repeatCount)
        {
            List<T> list = new List<T>();
            for (int i = 0; i < repeatCount; i++)
            {
                list.Add(function());
            }
            return list;
        }
    }
}

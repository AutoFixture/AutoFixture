using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Ploeh.SemanticComparison.Fluent
{
    /// <summary>
    /// Contains extension methods for working with <see cref="Likeness{TSource, TDestination}"/>.
    /// </summary>
    public static class LikenessSourceFactory
    {
        /// <summary>
        /// Creates a new <see cref="LikenessSource{TSource}"/> from an object instance.
        /// </summary>
        /// <typeparam name="TSource">The type of the source value.</typeparam>
        /// <param name="value">The source value.</param>
        /// <returns>A new <see cref="LikenessSource{TSource}"/> instance.</returns>
        /// <remarks>
        /// <para>
        /// This method is particularly handy for anonymous types, since it can use type
        /// inferencing to determine <typeparamref name="TSource"/> from the value itself. This is
        /// essentially the only way you can create a <see cref="Likeness{TSource, TDestination}"/>
        /// from the public API when the source value is an instance of an anonymous type.
        /// </para>
        /// </remarks>
        public static LikenessSource<TSource> AsSource<TSource>(this TSource value)
        {
            return new LikenessSource<TSource>(value);
        }
    }
}

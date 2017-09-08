using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Replaces an indexed element in a sequence with a given value.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the items in the sequences consumed and produced.
    /// </typeparam>
    public class IndexedReplacement<T> : IExpansion<T>
    {
        private readonly int replacementIndex;
        private readonly IEnumerable<T> source;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexedReplacement&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="replacementIndex">
        /// The index of the element which should be replaced.
        /// </param>
        /// <param name="source">The sequence in which an element should be replaced.</param>
        /// <remarks>
        /// <para>
        /// When <see cref="Expand" /> is invoked, the element at
        /// <paramref name="replacementIndex" /> in <paramref name="source" /> will be replaced by
        /// the new value. The index is zero-based.
        /// </para>
        /// </remarks>
        /// <seealso cref="Expand" />
        public IndexedReplacement(int replacementIndex, params T[] source)
        {
            this.replacementIndex = replacementIndex;
            this.source = source;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexedReplacement&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="replacementIndex">
        /// The index of the element which should be replaced.
        /// </param>
        /// <param name="source">The sequence in which an element should be replaced.</param>
        /// <remarks>
        /// <para>
        /// When <see cref="Expand" /> is invoked, the element at
        /// <paramref name="replacementIndex" /> in <paramref name="source" /> will be replaced by
        /// the new value. The index is zero-based.
        /// </para>
        /// </remarks>
        /// <seealso cref="Expand" />        
        public IndexedReplacement(int replacementIndex, IEnumerable<T> source)
            : this(replacementIndex, source.ToArray())
        {
        }

        /// <summary>
        /// Replaces the pre-selected element in the source sequence with a new element.
        /// </summary>
        /// <param name="value">The value which will replace the existing element.</param>
        /// <returns>
        /// A copy of <see cref="Source" /> where the pre-selected element has been replaced with
        /// <paramref name="value" />.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method replaces an element in <see cref="Source" /> with
        /// <paramref name="value" />. The element to be replaced is the element at the
        /// <see cref="ReplacementIndex" />. The replacement is not done directly in the
        /// <see cref="Source" />, but rather in a copy which is then returned by the method call.
        /// </para>
        /// </remarks>
        public IEnumerable<T> Expand(T value)
        {
            var list = this.source.ToList();
            list[this.replacementIndex] = value;
            return list;
        }

        /// <summary>
        /// Gets the index of the element which will be replaced by the <see cref="Expand" />
        /// method.
        /// </summary>
        /// <remarks>
        /// <para>The index is zero-based.</para>
        /// </remarks>
        public int ReplacementIndex
        {
            get { return this.replacementIndex; }
        }

        /// <summary>
        /// Gets the sequence of elements where one element will be replaced by the
        /// <see cref="Expand" /> method.
        /// </summary>
        public IEnumerable<T> Source
        {
            get { return this.source; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    public class IndexedReplacement<T> : IExpansion<T>
    {
        private readonly int replacementIndex;
        private readonly IEnumerable<T> source;

        public IndexedReplacement(int replacementIndex, params T[] source)
        {
            this.replacementIndex = replacementIndex;
            this.source = source;
        }

        public IndexedReplacement(int replacementIndex, IEnumerable<T> source)
            : this(replacementIndex, source.ToArray())
        {
        }

        #region IExpansion<T> Members

        public IEnumerable<T> Expand(T value)
        {
            var list = this.source.ToList();
            list[this.replacementIndex] = value;
            return list;
        }

        #endregion

        public int ReplacementIndex
        {
            get { return this.replacementIndex; }
        }

        public IEnumerable<T> Source
        {
            get { return this.source; }
        }
    }
}

using System.Collections.Generic;

namespace TestTypeFoundation
{
    public class DictionaryHolder<TK, TV>
    {
        public DictionaryHolder()
        {
            this.Dictionary = new Dictionary<TK, TV>();
        }

        public IDictionary<TK, TV> Dictionary { get; private set; }
    }
}

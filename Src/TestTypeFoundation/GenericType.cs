using System;

namespace Ploeh.TestTypeFoundation
{
    public class GenericType<T> where T : class
    {
        public GenericType(T t)
        {
            if (t == null)
            {
                throw new ArgumentNullException(nameof(t));
            }

            this.Value = t;
        }

        private T Value { get; }
    }
}
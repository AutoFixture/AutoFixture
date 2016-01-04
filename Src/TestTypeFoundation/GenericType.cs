using System;

namespace Ploeh.TestTypeFoundation
{
    public class GenericType<T> where T : class
    {
        private readonly T t;

        public GenericType(T t)
        {
            if (t == null)
            {
                throw new ArgumentNullException(nameof(t));
            }

            this.t = t;
        }

        T Value { get { return this.t; }}
    }
}
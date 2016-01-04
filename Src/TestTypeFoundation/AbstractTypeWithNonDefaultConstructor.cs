using System;

namespace Ploeh.TestTypeFoundation
{
    public abstract class AbstractTypeWithNonDefaultConstructor<T>
    {
        protected AbstractTypeWithNonDefaultConstructor(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.Property = value;
        }

        public T Property { get; }
    }
}

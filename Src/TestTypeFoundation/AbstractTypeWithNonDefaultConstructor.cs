using System;

namespace TestTypeFoundation;

public abstract class AbstractTypeWithNonDefaultConstructor<T>
{
    protected AbstractTypeWithNonDefaultConstructor(T value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        Property = value;
    }

    public T Property { get; }
}
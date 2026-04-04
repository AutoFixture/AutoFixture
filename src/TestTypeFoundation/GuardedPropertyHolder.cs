using System;

namespace TestTypeFoundation;

public class GuardedPropertyHolder<T>
    where T : class
{
    private T _property;

    public T Property
    {
        get
        {
            return _property;
        }

        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            _property = value;
        }
    }
}
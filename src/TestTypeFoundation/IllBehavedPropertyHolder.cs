namespace TestTypeFoundation;

public class IllBehavedPropertyHolder<T>
{
    private T _propertyIllBehavedSet;

    public T PropertyIllBehavedGet
    {
        get
        {
            return default(T);
        }

        set
        {
        }
    }

    public T PropertyIllBehavedSet
    {
        get
        {
            return _propertyIllBehavedSet;
        }

        set
        {
            _propertyIllBehavedSet = default(T);
        }
    }
}
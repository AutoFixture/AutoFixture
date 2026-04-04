namespace TestTypeFoundation;

public class InternalGetterPropertyHolder<T>
{
    public InternalGetterPropertyHolder(T property)
    {
        Property = property;
    }

    public T Property { internal get; set; }
}
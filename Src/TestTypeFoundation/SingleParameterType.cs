namespace TestTypeFoundation;

public class SingleParameterType<T>
{
    public SingleParameterType(T parameter)
    {
        Parameter = parameter;
    }

    public T Parameter { get; private set; }
}
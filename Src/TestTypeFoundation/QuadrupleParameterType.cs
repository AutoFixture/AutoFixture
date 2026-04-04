namespace TestTypeFoundation;

public class QuadrupleParameterType<T1, T2, T3, T4>
{
    public QuadrupleParameterType(T1 parameter1, T2 parameter2, T3 parameter3, T4 parameter4)
    {
        Parameter1 = parameter1;
        Parameter2 = parameter2;
        Parameter3 = parameter3;
        Parameter4 = parameter4;
    }

    public T1 Parameter1 { get; private set; }

    public T2 Parameter2 { get; private set; }

    public T3 Parameter3 { get; private set; }

    public T4 Parameter4 { get; private set; }
}
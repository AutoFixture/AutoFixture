namespace Ploeh.TestTypeFoundation
{
    public class DoubleParameterType<T1, T2>
    {
        public DoubleParameterType(T1 parameter1, T2 parameter2)
        {
            this.Parameter1 = parameter1;
            this.Parameter2 = parameter2;
        }

        public T1 Parameter1 { get; private set; }

        public T2 Parameter2 { get; private set; }
    }
}

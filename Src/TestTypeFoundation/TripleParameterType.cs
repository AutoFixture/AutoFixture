namespace Ploeh.TestTypeFoundation
{
    public class TripleParameterType<T1, T2, T3>
    {
        public TripleParameterType(T1 parameter1, T2 parameter2, T3 parameter3)
        {
            this.Parameter1 = parameter1;
            this.Parameter2 = parameter2;
            this.Parameter3 = parameter3;
        }

        public T1 Parameter1 { get; private set; }

        public T2 Parameter2 { get; private set; }

        public T3 Parameter3 { get; private set; }
    }
}

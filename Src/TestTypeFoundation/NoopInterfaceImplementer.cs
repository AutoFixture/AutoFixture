namespace TestTypeFoundation
{
    public class NoopInterfaceImplementer : IInterface
    {
        public object MakeIt(object obj)
        {
            return obj;
        }
    }
}

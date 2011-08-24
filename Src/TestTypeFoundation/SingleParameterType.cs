namespace Ploeh.TestTypeFoundation
{
    public class SingleParameterType<T>
    {
        public SingleParameterType(T parameter)
        {
            this.Parameter = parameter;
        }

        public T Parameter { get; private set; }
    }
}

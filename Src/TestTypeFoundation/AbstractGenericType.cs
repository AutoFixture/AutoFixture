namespace Ploeh.TestTypeFoundation
{
    public abstract class AbstractGenericType<T>
    {
        private readonly T t;

        public AbstractGenericType(T t)
        {
            this.t = t;
        }

        public T Value { get { return this.t; }}
    }
}
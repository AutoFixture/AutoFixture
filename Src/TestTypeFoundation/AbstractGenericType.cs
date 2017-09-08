namespace Ploeh.TestTypeFoundation
{
    public abstract class AbstractGenericType<T>
    {
        protected AbstractGenericType(T t)
        {
            this.Value = t;
        }

        public T Value { get; }
    }
}
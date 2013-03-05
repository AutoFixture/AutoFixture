namespace Ploeh.TestTypeFoundation
{
    public abstract class AbstractGenericType<T>
    {
        protected AbstractGenericType(T t)
        {
            Value = t;
        }

        public T Value { get; set; }
    }
}
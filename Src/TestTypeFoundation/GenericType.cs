namespace Ploeh.TestTypeFoundation
{
    public class GenericType<T> : AbstractGenericType<T>
        where T : class
    {
        public GenericType(T t) : base(t)
        {
        }
    }
}
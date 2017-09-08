namespace Ploeh.TestTypeFoundation
{
    public class ReadOnlyPropertyHolder<T>
    {
        public T Property { get; private set; }
    }
}

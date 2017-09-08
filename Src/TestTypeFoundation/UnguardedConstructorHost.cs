namespace Ploeh.TestTypeFoundation
{
    public class UnguardedConstructorHost<T>
    {
        public UnguardedConstructorHost(T item)
        {
            this.Item = item;
        }

        public T Item { get; }
    }
}

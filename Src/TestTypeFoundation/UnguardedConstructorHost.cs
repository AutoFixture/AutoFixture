namespace Ploeh.TestTypeFoundation
{
    public class UnguardedConstructorHost<T>
    {
        private readonly T item;

        public UnguardedConstructorHost(T item)
        {
            this.item = item;
        }

        public T Item
        {
            get { return this.item; }
        }
    }
}

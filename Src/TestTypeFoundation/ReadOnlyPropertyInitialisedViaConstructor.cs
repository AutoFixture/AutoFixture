namespace Ploeh.TestTypeFoundation
{
    class ReadOnlyPropertyInitialisedViaConstructor<T>
    {
        public ReadOnlyPropertyInitialisedViaConstructor(T initialValue)
        {
            this.Property = initialValue;
        }

        public T Property { get; private set; }
    }
}
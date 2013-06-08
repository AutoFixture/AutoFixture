namespace Ploeh.TestTypeFoundation
{
    class ReadOnlyFieldInitialisedViaConstructor<T>
    {
        public readonly T Field;

        public ReadOnlyFieldInitialisedViaConstructor(T value)
        {
            this.Field = value;
        }
    }
}
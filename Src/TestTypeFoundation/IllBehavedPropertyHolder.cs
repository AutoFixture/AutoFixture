namespace Ploeh.TestTypeFoundation
{
    public class IllBehavedPropertyHolder<T>
    {
        private T propertyIllBehavedSet;

        public T PropertyIllBehavedGet
        {
            get
            {
                return default(T);
            }
            
            set
            {
            }
        }

        public T PropertyIllBehavedSet
        {
            get
            {
                return this.propertyIllBehavedSet;
            }

            set
            {
                this.propertyIllBehavedSet = default(T);
            }
        }
    }
}
using System;

namespace Ploeh.TestTypeFoundation
{
    public class InvariantReferenceTypePropertyHolder<T> where T : class
    {
        private T property;

        public T Property
        {
            get
            {
                return this.property;     
            }

            set
            {
                if (value == default(T))
                {
                    throw new ArgumentNullException("value");
                }

                this.property = value;     
            }
        } 
    }
}
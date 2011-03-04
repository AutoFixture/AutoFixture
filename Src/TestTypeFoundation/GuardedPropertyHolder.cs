using System;

namespace Ploeh.TestTypeFoundation
{
    public class GuardedPropertyHolder<T>
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
                if (object.Equals(value, default(T)))
                {
                    throw new ArgumentNullException("value");
                }

                this.property = value;     
            }
        } 
    }
}
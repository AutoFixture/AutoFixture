using System;

namespace Ploeh.TestTypeFoundation
{
    public class GuardedPropertyHolder<T> where T : class
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
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.property = value;     
            }
        } 
    }
}
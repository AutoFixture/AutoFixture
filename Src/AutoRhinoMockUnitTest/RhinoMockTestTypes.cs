using System;

namespace AutoRhinoMockUnitTest
{
    public class RhinoMockTestTypes
    {
        public abstract class AnotherAbstractTypeWithNonDefaultConstructor<T>
        {
            private readonly T value;
            public AnotherAbstractTypeWithNonDefaultConstructor(T value)
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.value = value;
            }

            public T Value
            {
                get
                {
                    return this.value;
                }
            }
        }
        
        public class ConcreteGenericType<T>
        {
            private readonly T value;

            public ConcreteGenericType(T value)
            {
                this.value = value;
            }

            public T Value { get { return this.value; } }
        }
    }
}
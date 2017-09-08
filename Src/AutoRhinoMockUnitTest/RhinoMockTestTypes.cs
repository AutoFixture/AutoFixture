namespace Ploeh.AutoFixture.AutoRhinoMock.UnitTest
{
    public class RhinoMockTestTypes
    {
        public abstract class AnotherAbstractTypeWithNonDefaultConstructor<T>
        {
            private readonly T value;

            protected AnotherAbstractTypeWithNonDefaultConstructor(T value)
            {
                this.value = value;
            }
        }
        
        public sealed class ConcreteGenericType<T>
        {
            private readonly T value;

            public ConcreteGenericType(T value)
            {
                this.value = value;
            }

            public T Value { get { return this.value; } }
        }

        public sealed class ConcreteDoublyGenericType<T1, T2>
        {
            private readonly T1 value1;
            private readonly T2 value2;

            public ConcreteDoublyGenericType(T1 value1, T2 value2)
            {
                this.value1 = value1;
                this.value2 = value2;
            }

            public T1 Value1 { get { return this.value1; } }
            public T2 Value2 { get { return this.value2; } }
        }
    }
}
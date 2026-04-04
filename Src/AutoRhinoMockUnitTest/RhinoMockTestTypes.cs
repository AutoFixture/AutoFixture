namespace AutoFixture.AutoRhinoMock.UnitTest;

public class RhinoMockTestTypes
{
    public abstract class AnotherAbstractTypeWithNonDefaultConstructor<T>
    {
        private readonly T _value;

        protected AnotherAbstractTypeWithNonDefaultConstructor(T value)
        {
            _value = value;
        }
    }

    public sealed class ConcreteGenericType<T>
    {
        public ConcreteGenericType(T value)
        {
            Value = value;
        }

        public T Value { get; }
    }

    public sealed class ConcreteDoublyGenericType<T1, T2>
    {
        public ConcreteDoublyGenericType(T1 value1, T2 value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public T1 Value1 { get; }
        public T2 Value2 { get; }
    }
}
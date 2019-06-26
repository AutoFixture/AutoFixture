using System;

namespace TestTypeFoundation
{
    public sealed class TypeWithFactoryMethodReturningInterface
        : IEquatable<TypeWithFactoryMethodReturningInterface>
    {
        private TypeWithFactoryMethodReturningInterface()
        {
        }
        
        public static IEquatable<TypeWithFactoryMethodReturningInterface> Instance
        {
            get;
        } = new TypeWithFactoryMethodReturningInterface();

        public bool Equals(TypeWithFactoryMethodReturningInterface other) =>
            true;

        public override bool Equals(object obj) =>
            obj is TypeWithFactoryMethodReturningInterface other &&
            Equals(other);

        public override int GetHashCode() => 0;
    }
}

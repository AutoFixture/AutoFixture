using System;

namespace Ploeh.SemanticComparison.UnitTest.TestTypes
{
    public class TypeWithPrivateDefaultCtorAndOtherCtor<T>
    {
        private TypeWithPrivateDefaultCtorAndOtherCtor()
        {
            
        }
        public TypeWithPrivateDefaultCtorAndOtherCtor(T value) : this()
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.Property = value;
        }

        public T Property { get; }
    }
}
using System;

namespace Ploeh.SemanticComparison.UnitTest.TestTypes
{
    public class TypeWithPrivateDefaultCtorAndOtherCtor<T>
    {
        private readonly T property;

        private TypeWithPrivateDefaultCtorAndOtherCtor()
        {
            
        }
        public TypeWithPrivateDefaultCtorAndOtherCtor(T value) : this()
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.property = value;
        }

        public T Property
        {
            get { return this.property; }
        }

    }
}
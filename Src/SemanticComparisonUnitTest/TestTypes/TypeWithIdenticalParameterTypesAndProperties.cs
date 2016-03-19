using System;

namespace Ploeh.SemanticComparison.UnitTest.TestTypes
{
    public class TypeWithIdenticalParameterTypesAndProperties
    {
        public TypeWithIdenticalParameterTypesAndProperties(
            long parameter1,
            long parameter2,
            long parameter3)
        {
            this.Property1 = parameter1;
            this.Property2 = parameter2;
            this.Property3 = parameter3;
            this.Property4 = 400;
        }

        protected TypeWithIdenticalParameterTypesAndProperties(object source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
        }

        public long Property1 { get; }

        public long Property2 { get; }

        public long Property3 { get; }

        public long Property4 { get; }
    }
}

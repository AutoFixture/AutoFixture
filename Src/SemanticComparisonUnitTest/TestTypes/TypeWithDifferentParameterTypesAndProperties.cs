using System;

namespace Ploeh.SemanticComparison.UnitTest.TestTypes
{
    public class TypeWithDifferentParameterTypesAndProperties
    {
        public TypeWithDifferentParameterTypesAndProperties(
            double field1,
            string field2,
            int field3)
            : this(field1, field2, field3, Guid.NewGuid())
        {
        }

        public TypeWithDifferentParameterTypesAndProperties(
            double field1,
            string field2,
            int field3,
            Guid field4)
        {
            this.Property1 = field1;
            this.Property2 = field2;
            this.Property3 = field3;
            this.Property4 = field4;
        }

        protected TypeWithDifferentParameterTypesAndProperties(object source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
        }

        public double Property1 { get; }

        public string Property2 { get; }

        public int Property3 { get; }

        public Guid Property4 { get; }
    }
}

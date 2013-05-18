using System;

namespace Ploeh.SemanticComparison.UnitTest
{
    public class TypeWithDifferentParameterTypesAndProperties
    {
        private readonly double field1;
        private readonly string field2;
        private readonly int field3;
        private readonly Guid field4;

        public TypeWithDifferentParameterTypesAndProperties(
            double field1,
            string field2,
            int field3)
        {
            this.field1 = field1;
            this.field2 = field2;
            this.field3 = field3;
            this.field4 = Guid.NewGuid();
        }

        protected TypeWithDifferentParameterTypesAndProperties(object source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
        }

        public double Property1
        {
            get { return this.field1; }
        }

        public string Property2
        {
            get { return this.field2; }
        }

        public int Property3
        {
            get { return this.field3; }
        }

        public Guid Property4
        {
            get { return this.field4; }
        }
    }
}

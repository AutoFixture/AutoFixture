using System;

namespace Ploeh.SemanticComparison.UnitTest
{
    public class TypeWithIdenticalParameterTypesAndProperties
    {
        private readonly long field1;
        private readonly long field2;
        private readonly long field3;
        private readonly long field4;

        public TypeWithIdenticalParameterTypesAndProperties(
            long parameter1,
            long parameter2,
            long parameter3)
        {
            this.field1 = parameter1;
            this.field2 = parameter2;
            this.field3 = parameter3;
            this.field4 = 400;
        }

        protected TypeWithIdenticalParameterTypesAndProperties(object source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
        }

        public long Property1
        {
            get { return this.field1; }
        }

        public long Property2
        {
            get { return this.field2; }
        }

        public long Property3
        {
            get { return this.field3; }
        }

        public long Property4
        {
            get { return this.field4; }
        }
    }
}

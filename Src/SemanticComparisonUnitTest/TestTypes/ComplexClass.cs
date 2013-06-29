using System;

namespace Ploeh.SemanticComparison.UnitTest.TestTypes
{
    public class ComplexClass
    {
        public int Number { get; set; }

        public string Text { get; set; }

        public Version Version { get; set; }

        public OperatingSystem OperatingSystem { get; set; }

        public ValueObject Value { get; set; }

        public Entity Entity { get; set; }
    }
}

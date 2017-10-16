namespace AutoFixture.NUnit3.UnitTest
{
    public class TestNameStrategiesFixture
    {
        private static IFixture CreateFixtureWithInjectedValues()
        {
            var result = new Fixture();
            // Make so that fixed values will be returned
            result.Inject(42);
            result.Inject("foo");
            return result;
        }

        public class AutoDataFixedNameAttribute : AutoDataAttribute
        {
            public AutoDataFixedNameAttribute()
            {
                this.TestMethodBuilder = new FixedNameTestMethodBuilder();
            }
        }

        public class AutoDataVolatileNameAttribute : AutoDataAttribute
        {
            public AutoDataVolatileNameAttribute() : base(CreateFixtureWithInjectedValues)
            {
                this.TestMethodBuilder = new VolatileNameTestMethodBuilder();
            }
        }

        public class InlineAutoDataFixedNameAttribute : InlineAutoDataAttribute
        {
            public InlineAutoDataFixedNameAttribute(params object[] arguments) 
                : base(arguments)
            {
                this.TestMethodBuilder = new FixedNameTestMethodBuilder();
            }
        }

        public class InlineAutoDataVolatileNameAttribute : InlineAutoDataAttribute
        {
            public InlineAutoDataVolatileNameAttribute(params object[] arguments) 
                : base(CreateFixtureWithInjectedValues, arguments)
            {
                this.TestMethodBuilder = new VolatileNameTestMethodBuilder();
            }
        }

        [AutoDataFixedName]
        public void FixedNameDecoratedMethod(int expectedNumber, MyClass sut)
        {
        }

        [AutoDataVolatileName]
        public void VolatileNameDecoratedMethod(int expectedNumber, string value)
        {
        }

        [InlineAutoDataFixedName("alpha", "beta")]
        public void InlineFixedNameDecoratedMethod(string p1, string p2, string p3)
        {
        }

        [InlineAutoDataVolatileName("alpha", "beta")]
        public void InlineVolatileNameDecoratedMethod(string p1, string p2, string p3)
        {
        }
    }
}

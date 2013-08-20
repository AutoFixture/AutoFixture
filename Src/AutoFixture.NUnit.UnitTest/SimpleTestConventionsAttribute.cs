using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.NUnit;

namespace AutoSpecificationFor.Tests
{
    public class SimpleTestConventionsAttribute : TestConventionsAttribute
    {
        public SimpleTestConventionsAttribute()
            : base(new Fixture().Customize(new AutoNSubstituteCustomization()))
        {
        }
    }
}
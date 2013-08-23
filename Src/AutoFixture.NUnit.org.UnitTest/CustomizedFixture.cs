using Ploeh.AutoFixture;
using Ploeh.TestTypeFoundation;

namespace Ploe.AutoFixture.NUnit.org.UnitTest
{
    internal class CustomizedFixture : Fixture
    {
        public CustomizedFixture()
        {
            Customize<PropertyHolder<string>>(c => c.With(x => x.Property, "Ploeh"));
        }
    }
}

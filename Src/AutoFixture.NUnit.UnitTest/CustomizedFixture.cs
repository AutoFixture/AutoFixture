using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixture.NUnit.UnitTest
{
    internal class CustomizedFixture : Fixture
    {
        public CustomizedFixture()
        {
            Customize<PropertyHolder<string>>(c => c.With(x => x.Property, "Ploeh"));
        }
    }
}

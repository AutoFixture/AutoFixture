using TestTypeFoundation;

namespace AutoFixture.Xunit3.UnitTest.TestTypes
{
    internal class CustomizedFixture : Fixture
    {
        public CustomizedFixture()
        {
            this.Customize<PropertyHolder<string>>(c => c.With(x => x.Property, "Ploeh"));
        }
    }
}
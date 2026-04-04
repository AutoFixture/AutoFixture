using TestTypeFoundation;

namespace AutoFixture.Xunit.UnitTest;

internal class CustomizedFixture : Fixture
{
    public CustomizedFixture()
    {
        Customize<PropertyHolder<string>>(c => c.With(x => x.Property, "Ploeh"));
    }
}
namespace Ploeh.AutoFixture.NUnit3.UnitTest
{
    /// <summary>
    /// A stub of <see cref="InlineAutoDataAttribute"/> for the benefit of unit testing
    /// </summary>
    public class InlineAutoDataAttributeStub : InlineAutoDataAttribute
    {
        public InlineAutoDataAttributeStub(IFixture fixture, params object[] arguments)
            : base(fixture, arguments)
        {
        }
    }
}
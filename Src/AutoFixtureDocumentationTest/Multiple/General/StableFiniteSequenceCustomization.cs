using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureDocumentationTest.Multiple.General
{
    public class StableFiniteSequenceCustomization : ICustomization
    {
        #region ICustomization Members

        public void Customize(IFixture fixture)
        {
            var stableRelay = new StableFiniteSequenceRelay();
            fixture.Customizations.Add(stableRelay);
        }

        #endregion
    }
}

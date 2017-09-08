using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureDocumentationTest.Multiple.General
{
    public class StableFiniteSequenceCustomization :
        ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var stableRelay = 
                new StableFiniteSequenceRelay();
            fixture.Customizations.Add(stableRelay);
        }
    }
}

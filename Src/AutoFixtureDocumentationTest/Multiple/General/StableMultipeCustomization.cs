using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureDocumentationTest.Multiple.General
{
    public class StableMultipeCustomization : 
        CompositeCustomization
    {
        public StableMultipeCustomization()
            : base(
                new StableFiniteSequenceCustomization(),
                new MultipleCustomization())
        {
        }
    }
}

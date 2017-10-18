using AutoFixture;

namespace AutoFixtureDocumentationTest.Multiple.General
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

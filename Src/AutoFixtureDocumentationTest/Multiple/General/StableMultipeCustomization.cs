using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

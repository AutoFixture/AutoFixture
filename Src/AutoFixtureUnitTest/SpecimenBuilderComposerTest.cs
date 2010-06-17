using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixtureUnitTest.Dsl;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureUnitTest
{
    public class SpecimenBuilderComposerTest
    {
        [Fact]
        public void BuilReturnsCorrectType()
        {
            // Fixture setup
            var composer = new DelegatingComposer();
            // Exercise system
            ICustomizationComposer<string> result = composer.Build<string>();
            // Verify outcome
            Assert.IsAssignableFrom<CompositeComposer<string>>(result);
            // Teardown
        }

        [Fact]
        public void BuildReturnsResultContainingCorrectNullComposer()
        {
            // Fixture setup
            var builder = new DelegatingSpecimenBuilder();
            var composer = new DelegatingComposer { OnCompose = () => builder };
            // Exercise system
            var result = composer.Build<string>();
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositeComposer<string>>(result);
            var nullComposer = Assert.IsAssignableFrom<NullComposer<string>>(composite.Composers.Last());
            Assert.Equal(builder, nullComposer.Compose());
            // Teardown
        }

        [Fact]
        public void BuildReturnResultContainingCorrectComposer()
        {
            // Fixture setup
            var composer = new DelegatingComposer<int>();
            // Exercise system
            var result = composer.Build<int>();
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositeComposer<int>>(result);
            Assert.IsAssignableFrom<Composer<int>>(composite.Composers.First());
            // Teardown
        }
    }
}

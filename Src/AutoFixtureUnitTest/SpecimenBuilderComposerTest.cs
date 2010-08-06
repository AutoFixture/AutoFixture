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
    /// <summary>
    /// These tests mostly deal with boundary cases (like null
    /// guards) that are specific to the extension methods.
    /// Implementation are covered elsewhere (most notable in
    /// FixtureTest).
    /// </summary>
    public class SpecimenBuilderComposerTest
    {
        [Fact]
        public void SingleParameterDoWithNullFixtureThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenBuilderComposer.Do<object>(null, x => { }));
            // Teardown
        }

        [Fact]
        public void DoubleParameterDoWithNullFixtureThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenBuilderComposer.Do<object, object>(null, (x, y) => { }));
            // Teardown
        }

        [Fact]
        public void TripleParameterDoWithNullFixtureThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenBuilderComposer.Do<object, object, object>(null, (x, y, z) => { }));
            // Teardown
        }

        [Fact]
        public void QuadrupleParameterDoWithNullFixtureThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenBuilderComposer.Do<object, object, object, object>(null, (x, y, z, æ) => { }));
            // Teardown
        }

        [Fact]
        public void SingleParameterGetWithNullFixtureThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenBuilderComposer.Get<object, object>(null, x => x));
            // Teardown
        }

        [Fact]
        public void DoubleParameterGetWithNullFixtureThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenBuilderComposer.Get<object, object, object>(null, (x, y) => x));
            // Teardown
        }

        [Fact]
        public void TripleParameterGetWithNullFixtureThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenBuilderComposer.Get<object, object, object, object>(null, (x, y, z) => x));
            // Teardown
        }

        [Fact]
        public void QuadrupleParameterGetWithNullFixtureThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenBuilderComposer.Get<object, object, object, object, object>(null, (x, y, z, æ) => x));
            // Teardown
        }

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

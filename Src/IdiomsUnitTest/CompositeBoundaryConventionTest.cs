using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class CompositeBoundaryConventionTest
    {
        [Fact]
        public void SutIsBoundaryConvention()
        {
            // Fixture setup
            // Exercise system
            var sut = new CompositeBoundaryConvention();
            // Verify outcome
            Assert.IsAssignableFrom<IBoundaryConvention>(sut);
            // Teardown
        }

        [Fact]
        public void InitializedWithNullArrayThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeBoundaryConvention((IBoundaryConvention[])null));
            // Teardown
        }

        [Fact]
        public void InitializedWithNullEnumerableThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeBoundaryConvention((IEnumerable<IBoundaryConvention>)null));
            // Teardown
        }

        [Fact]
        public void InitializedWithArrayHasCorrectConventions()
        {
            // Fixture setup
            var expectedConventions = new[] { new DelegatingBoundaryConvention(), new DelegatingBoundaryConvention(), new DelegatingBoundaryConvention() };
            var sut = new CompositeBoundaryConvention(expectedConventions);
            // Exercise system
            var result = sut.Conventions;
            // Verify outcome
            Assert.True(expectedConventions.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void InitializedWithEnumerableHasCorrectConventions()
        {
            // Fixture setup
            var expectedConventions = new IBoundaryConvention[] { new DelegatingBoundaryConvention(), new DelegatingBoundaryConvention(), new DelegatingBoundaryConvention() }.AsEnumerable();
            var sut = new CompositeBoundaryConvention(expectedConventions);
            // Exercise system
            var result = sut.Conventions;
            // Verify outcome
            Assert.True(expectedConventions.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void CreateBoundaryBehaviorsReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            var type = typeof(short);
            var expectedBehaviors = new ExceptionBoundaryBehavior[] { new DelegatingBoundaryBehavior(), new DelegatingBoundaryBehavior(), new DelegatingBoundaryBehavior(), new DelegatingBoundaryBehavior() };
            var conventions = new[]
            {
                new DelegatingBoundaryConvention { OnCreateBoundaryBehaviors = t => t == type ? new[]{ expectedBehaviors[0] } : new[]{new DelegatingBoundaryBehavior()}},
                new DelegatingBoundaryConvention { OnCreateBoundaryBehaviors = t => t == type ? new ExceptionBoundaryBehavior[]{ } : new[]{new DelegatingBoundaryBehavior()}},
                new DelegatingBoundaryConvention { OnCreateBoundaryBehaviors = t => t == type ? new[]{ expectedBehaviors[1], expectedBehaviors[2], expectedBehaviors[3] } : new[]{new DelegatingBoundaryBehavior()}}
            };

            var sut = new CompositeBoundaryConvention(conventions);
            // Exercise system
            var result = sut.CreateBoundaryBehaviors(type);
            // Verify outcome
            Assert.True(expectedBehaviors.SequenceEqual(result));
            // Teardown
        }
    }
}

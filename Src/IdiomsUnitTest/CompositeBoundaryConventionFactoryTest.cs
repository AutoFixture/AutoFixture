using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class CompositeBoundaryConventionFactoryTest
    {
        [Fact]
        public void SutIsBoundaryConventionFactory()
        {
            // Fixture setup
            // Exercise system
            var sut = new CompositeBoundaryConventionFactory();
            // Verify outcome
            Assert.IsAssignableFrom<IBoundaryConventionFactory>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullArrayThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeBoundaryConventionFactory((IBoundaryConventionFactory[])null));
            // Teardown
        }

        [Fact]
        public void InitializeWillNullEnumerableThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeBoundaryConventionFactory((IEnumerable<IBoundaryConventionFactory>)null));
            // Teardown
        }

        [Fact]
        public void InitializedWithArrayHasCorrectFactories()
        {
            // Fixture setup
            var expectedFactories = new[] { new DelegatingBoundaryConventionFactory(), new DelegatingBoundaryConventionFactory(), new DelegatingBoundaryConventionFactory() };
            var sut = new CompositeBoundaryConventionFactory(expectedFactories);
            // Exercise system
            var result = sut.Factories;
            // Verify outcome
            Assert.True(expectedFactories.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void InitializedWithEnumerableHasCorrectFactories()
        {
            // Fixture setup
            var expectedFactories = new IBoundaryConventionFactory[] { new DelegatingBoundaryConventionFactory(), new DelegatingBoundaryConventionFactory(), new DelegatingBoundaryConventionFactory() }.AsEnumerable();
            var sut = new CompositeBoundaryConventionFactory(expectedFactories);
            // Exercise system
            var result = sut.Factories;
            // Verify outcome
            Assert.True(expectedFactories.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void GetConventionReturnsCorrectResult()
        {
            // Fixture setup
            var type = typeof(string);
            var expectedConventions = new[] { new DelegatingBoundaryConvention(), new DelegatingBoundaryConvention(), new DelegatingBoundaryConvention() };
            var factories = new[]
            {
                new DelegatingBoundaryConventionFactory{ OnGetConvention = t => t == type ? expectedConventions[0] : new DelegatingBoundaryConvention() },
                new DelegatingBoundaryConventionFactory{ OnGetConvention = t => t == type ? expectedConventions[1] : new DelegatingBoundaryConvention() },
                new DelegatingBoundaryConventionFactory{ OnGetConvention = t => t == type ? expectedConventions[2] : new DelegatingBoundaryConvention() }
            };

            var sut = new CompositeBoundaryConventionFactory(factories);
            // Exercise system
            var result = sut.GetConvention(type);
            // Verify outcome
            var c = Assert.IsAssignableFrom<CompositeBoundaryConvention>(result);
            Assert.True(expectedConventions.SequenceEqual(c.Conventions));
            // Teardown
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class DefaultBoundaryConventionFactoryTest
    {
        [Fact]
        public void SutIsCollection()
        {
            // Fixture setup
            // Exercise system
            var sut = new DefaultBoundaryConventionFactory();
            // Verify outcome
            Assert.IsAssignableFrom<CompositeBoundaryConventionFactory>(sut);
            // Teardown
        }

        [Fact]
        public void ItemTypesAreCorrect()
        {
            // Fixture setup
            var expected = new[] {
                typeof(GuidBoundaryConventionFactory),
                typeof(ValueTypeBoundaryConventionFactory),
                typeof(StringBoundaryConventionFactory), 
                typeof(ReferenceTypeBoundaryConventionFactory)
            };

            // Exercise system
            var sut = new DefaultBoundaryConventionFactory();
            // Verify outcome
            var result = (from item in sut.Factories
                          select item.GetType()).ToList();
            Assert.True(expected.SequenceEqual(result), "Default ctor");
            // Teardown
        }
    }
}

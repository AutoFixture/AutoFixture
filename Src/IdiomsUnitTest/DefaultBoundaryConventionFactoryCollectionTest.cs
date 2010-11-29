using System;
using System.Collections.ObjectModel;
using System.Linq;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class DefaultBoundaryConventionFactoryCollectionTest
    {
        [Fact]
        public void SutIsCollection()
        {
            // Fixture setup
            // Exercise system
            var sut = new DefaultBoundaryConventionFactoryCollection();
            // Verify outcome
            Assert.IsAssignableFrom<Collection<IBoundaryConventionFactory>>(sut);
            // Teardown
        }

        [Fact]
        public void ItemTypesAreCorrect()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expected = new[] {
                typeof(GuidBoundaryConventionFactory),
                typeof(ValueTypeBoundaryConventionFactory),
                typeof(StringBoundaryConventionFactory), 
                typeof(ReferenceTypeBoundaryConventionFactory)
            };

            // Exercise system
            var sut = fixture.CreateAnonymous<DefaultBoundaryConventionFactoryCollection>();
            // Verify outcome
            var result = (from item in sut select item.GetType()).ToList();
            Assert.True(expected.SequenceEqual(result), "Default ctor");
            // Teardown
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class DefaultTypeGuardSpecificationCollectionTest
    {
        [Fact]
        public void SutIsCollection()
        {
            // Fixture setup
            // Exercise system
            var sut = new DefaultTypeGuardSpecificationCollection();
            // Verify outcome
            Assert.IsAssignableFrom<Collection<ITypeGuardSpecification>>(sut);
            // Teardown
        }

        [Fact]
        public void ItemTypesAreCorrect()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expected = new[] {
                typeof(GuidGuardSpecification),
                typeof(ValueTypeGuardSpecification),
                typeof(StringGuardSpecification), 
                typeof(ReferenceTypeGuardSpecification)
            };

            // Exercise system
            var sut = fixture.CreateAnonymous<DefaultTypeGuardSpecificationCollection>();
            // Verify outcome
            var result = (from item in sut select item.GetType()).ToList();
            Assert.True(expected.SequenceEqual(result), "Default ctor");
            // Teardown
        }
    }
}

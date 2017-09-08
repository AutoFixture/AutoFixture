using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class CompositeCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Fixture setup
            // Exercise system
            var sut = new CompositeCustomization();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomization>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullArrayThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeCustomization((ICustomization[])null));
            // Teardown
        }

        [Fact]
        public void CustomizationsIsCorrectWhenInitializedWithArray()
        {
            // Fixture setup
            var customizations = new[]
            {
                new DelegatingCustomization(),
                new DelegatingCustomization(),
                new DelegatingCustomization()
            };

            var sut = new CompositeCustomization(customizations);
            // Exercise system
            IEnumerable<ICustomization> result = sut.Customizations;
            // Verify outcome
            Assert.True(customizations.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullEnumerableThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeCustomization((IEnumerable<ICustomization>)null));
            // Teardown
        }

        [Fact]
        public void CustomizationsIsCorrectWhenInitializedWithEnumerable()
        {
            // Fixture setup
            var customizations = new[]
            {
                new DelegatingCustomization(),
                new DelegatingCustomization(),
                new DelegatingCustomization()
            };

            var sut = new CompositeCustomization(customizations);
            // Exercise system
            var result = sut.Customizations;
            // Verify outcome
            Assert.True(customizations.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void CustomizeCustomizesFixtureForAllCustomizations()
        {
            // Fixture setup
            var fixture = new Fixture();

            var verifications = new List<bool>();
            var customizations = new[]
            {
                new DelegatingCustomization { OnCustomize = f => verifications.Add(f == fixture)},
                new DelegatingCustomization { OnCustomize = f => verifications.Add(f == fixture)},
                new DelegatingCustomization { OnCustomize = f => verifications.Add(f == fixture)}
            };

            var sut = new CompositeCustomization(customizations);
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            Assert.Equal(customizations.Length, verifications.Count);
            Assert.True(verifications.All(b => b));
            // Teardown
        }
    }
}

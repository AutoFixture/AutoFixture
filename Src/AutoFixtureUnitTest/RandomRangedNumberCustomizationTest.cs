﻿using System;
using System.Linq;
using AutoFixture;
using Xunit;

namespace AutoFixtureUnitTest
{
    [Obsolete]
    public class RandomRangedNumberCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Fixture setup
            // Exercise system
            var sut = new RandomRangedNumberCustomization();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomization>(sut);
            // Teardown
        }

        [Fact]
        public void CustomizeWithNullFixtureThrows()
        {
            // Fixture setup
            var sut = new RandomRangedNumberCustomization();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
            // Teardown
        }

        [Fact]
        public void CustomizeAddsCorrectBuilderToFixture()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new RandomRangedNumberCustomization();
            // Exercise system
            sut.Customize(fixture);
            var result = fixture.Customizations
                .OfType<RandomRangedNumberGenerator>()
                .SingleOrDefault();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }
    }
}
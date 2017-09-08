using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class DefaultEnginePartsTest
    {
        [Fact]
        public void SutIsTransmissionBuilders()
        {
            // Fixture setup
            // Exercise system
            var sut = new DefaultEngineParts();
            // Verify outcome
            Assert.IsAssignableFrom<DefaultRelays>(sut);
            // Teardown
        }

        [Fact]
        public void InitializedWithDefaultConstructorSutHasCorrectContents()
        {
            // Fixture setup
            var expectedBuilders = new DefaultPrimitiveBuilders()
                .Concat(new DefaultRelays())
                .Select(b => b.GetType());
            // Exercise system
            var sut = new DefaultEngineParts();
            // Verify outcome
            Assert.True(expectedBuilders.SequenceEqual(sut.Select(b => b.GetType())));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullBuildersThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new DefaultEngineParts((IEnumerable<ISpecimenBuilder>)null));
            // Teardown
        }

        [Fact]
        public void InitializedWithEnumerableBuildersSutHasCorrectContents()
        {
            // Fixture setup
            var primitiveBuilders = Enumerable.Range(1, 3).Select(i => new DelegatingSpecimenBuilder()).Cast<ISpecimenBuilder>().ToList();
            var expectedBuilders = primitiveBuilders
                .Concat(new DefaultRelays())
                .Select(b => b.GetType());
            // Exercise system
            var sut = new DefaultEngineParts(primitiveBuilders);
            // Verify outcome
            Assert.True(expectedBuilders.SequenceEqual(sut.Select(b => b.GetType())));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullArrayThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new DefaultEngineParts((ISpecimenBuilder[])null));
            // Teardown
        }

        [Fact]
        public void InitializedWithBuildersArraySutHasCorrectContents()
        {
            // Fixture setup
            var primitiveBuilders = Enumerable.Range(1, 3).Select(i => new DelegatingSpecimenBuilder()).ToArray();
            var expectedBuilders = primitiveBuilders
                .Concat(new DefaultRelays())
                .Select(b => b.GetType());
            // Exercise system
            var sut = new DefaultEngineParts(primitiveBuilders);
            // Verify outcome
            Assert.True(expectedBuilders.SequenceEqual(sut.Select(b => b.GetType())));
            // Teardown
        }
    }
}

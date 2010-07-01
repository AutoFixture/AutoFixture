using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    public class DefaultEngineBuildersTest
    {
        [Fact]
        public void SutIsTransmissionBuilders()
        {
            // Fixture setup
            // Exercise system
            var sut = new DefaultEngineBuilders();
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
            var sut = new DefaultEngineBuilders();
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
                new DefaultEngineBuilders((IEnumerable<ISpecimenBuilder>)null));
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
            var sut = new DefaultEngineBuilders(primitiveBuilders);
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
                new DefaultEngineBuilders((ISpecimenBuilder[])null));
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
            var sut = new DefaultEngineBuilders(primitiveBuilders);
            // Verify outcome
            Assert.True(expectedBuilders.SequenceEqual(sut.Select(b => b.GetType())));
            // Teardown
        }
    }
}

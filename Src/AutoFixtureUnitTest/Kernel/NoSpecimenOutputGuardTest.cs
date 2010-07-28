using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class NoSpecimenOutputGuardTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var sut = new NoSpecimenOutputGuard(dummyBuilder);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullBuilderThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new NoSpecimenOutputGuard(null));
            // Teardown
        }

        [Fact]
        public void BuilderIsCorrect()
        {
            // Fixture setup
            var expectedBuilder = new DelegatingSpecimenBuilder();
            var sut = new NoSpecimenOutputGuard(expectedBuilder);
            // Exercise system
            ISpecimenBuilder result = sut.Builder;
            // Verify outcome
            Assert.Equal(expectedBuilder, result);
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResult()
        {
            // Fixture setup
            var request = new object();
            var context = new DelegatingSpecimenContext();
            var expectedResult = new object();
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == request && c == context ? expectedResult : new NoSpecimen(r) };

            var sut = new NoSpecimenOutputGuard(builder);
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateThrowsWhenDecoratedBuilderReturnsNoSpecimen()
        {
            // Fixture setup
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => new NoSpecimen(r) };
            var sut = new NoSpecimenOutputGuard(builder);
            // Exercise system and verify outcome
            var dummyRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();
            Assert.Throws<ObjectCreationException>(() =>
                sut.Create(dummyRequest, dummyContext));
            // Teardown
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Dsl;

namespace Ploeh.AutoFixtureUnitTest
{
    public class SpecimenFactoryTest
    {
        [Fact]
        public void CreateAnonymousOnContainerReturnsCorrectResult()
        {
            // Fixture setup
            object expectedResult = 1;
            var container = new DelegatingSpecimenContainer { OnResolve = r => r == typeof(int) ? expectedResult : new NoSpecimen(r) };
            // Exercise system
            var result = container.CreateAnonymous<int>();
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnSpecimenBuilderComposerReturnsCorrectResult()
        {
            // Fixture setup
            var expectedResult = new DateTime(2010, 5, 31, 14, 52, 19);
            var specimenBuilder = new DelegatingSpecimenBuilder();
            specimenBuilder.OnCreate = (r, c) =>
            {
                Assert.NotNull(c);
                Assert.Equal(typeof(DateTime), r);
                return expectedResult;
            };

            var composer = new DelegatingSpecimenBuilderComposer { OnCompose = () => specimenBuilder };
            // Exercise system
            var result = composer.CreateAnonymous<DateTime>();
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateSeededAnonymousOnContainerReturnsCorrectResult()
        {
            // Fixture setup
            var seed = TimeSpan.FromMinutes(8);
            object expectedResult = TimeSpan.FromHours(2);
            var container = new DelegatingSpecimenContainer { OnResolve = r => r.Equals(new SeededRequest(typeof(TimeSpan), seed)) ? expectedResult : new NoSpecimen(r) };
            // Exercise system
            var result = container.CreateAnonymous(seed);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateaSeededAnonymousOnSpecimenBuilderComposerReturnsCorrectResult()
        {
            // Fixture setup
            var seed = new Version(1, 1);
            var expectedResult = new Version(2, 0);
            var specimenBuilder = new DelegatingSpecimenBuilder();
            specimenBuilder.OnCreate = (r, c) =>
                {
                    Assert.NotNull(c);
                    Assert.Equal(new SeededRequest(typeof(Version), seed), r);
                    return expectedResult;
                };

            var composer = new DelegatingSpecimenBuilderComposer { OnCompose = () => specimenBuilder };
            // Exercise system
            var result = composer.CreateAnonymous(seed);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact(Skip = "Waiting for slight redesign of the ManyRequest/ManyUnwrapper API to handle uncounted requests for Many.")]
        public void CreateManyOnContainerReturnsCorrectResult()
        {
            // Fixture setup
            var count = 3;
            object expectedResult = Enumerable.Range(1, count);
            var container = new DelegatingSpecimenContainer { OnResolve = r => r.Equals(new FiniteSequenceRequest(typeof(int), count)) ? expectedResult : new NoSpecimen(r) };
            // Exercise system
            var result = container.CreateMany<int>();
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}

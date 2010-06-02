using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class ManyTranslatorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new ManyTranslator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CountIsProperWritableProperty()
        {
            // Fixture setup
            var sut = new ManyTranslator();
            var expectedCount = 1;
            // Exercise system
            sut.Count = expectedCount;
            int result = sut.Count;
            // Verify outcome
            Assert.Equal(expectedCount, result);
            // Teardown
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void SettingInvalidCountThrows(int count)
        {
            // Fixture setup
            var sut = new ManyTranslator();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                sut.Count = count);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContainerThrows()
        {
            // Fixture setup
            var sut = new ManyTranslator();
            var dummyRequest = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
            // Teardown
        }

        [Fact]
        public void CreateWithAnonymousRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new ManyTranslator();
            var request = new object();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(request, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData("foo")]
        public void CreateWithInvalidRequestReturnsCorrectResult(object request)
        {
            // Fixture setup
            var sut = new ManyTranslator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(request, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithManyRequestReturnsCorrectResult()
        {
            // Fixture setup
            var request = new ManyRequest(new object());
            var count = 7;
            var expectedTranslation = new FiniteSequenceRequest(request.Request, 7);
            var expectedResult = new object();
            var container = new DelegatingSpecimenContainer { OnResolve = r => expectedTranslation.Equals(r) ? expectedResult : new NoSpecimen(r) };

            var sut = new ManyTranslator { Count = count };
            // Exercise system
            var result = sut.Create(request, container);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}

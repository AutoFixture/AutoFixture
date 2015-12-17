using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class OmitterTests
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new Omitter();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestThrows()
        {
            // Fixture setup
            var sut = new Omitter();
            // Exercise system and verify outcome
            var dummyContext = new DelegatingSpecimenContext();
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(null, dummyContext));
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new Omitter();
            // Exercise system
            var dummyRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();
            var actual = sut.Create(dummyRequest, dummyContext);
            // Verify outcome
            Assert.IsAssignableFrom<OmitSpecimen>(actual);
            // Teardown
        }

        [Fact]
        public void SpecificationMatchesConstructorArgument()
        {
            // Fixture setup
            var expected = new DelegatingRequestSpecification();
            var sut = new Omitter(expected);
            // Exercise system
            IRequestSpecification actual = sut.Specification;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void ConstructWithNullSpecificationThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new Omitter(null));
            // Teardown
        }

        [Fact]
        public void CreateWhenSpecificationIsFalseReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new Omitter(new FalseRequestSpecification());
            var request = new object();
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var actual = sut.Create(request, dummyContext);
            // Verify outcome
            var expected = new NoSpecimen();
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void CreateWhenSpecificationMatchesRequestReturnsCorrectResult()
        {
            // Fixture setup
            var request = new object();
            var specification = new DelegatingRequestSpecification
            {
                OnIsSatisfiedBy = request.Equals
            };
            var sut = new Omitter(specification);
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var actual = sut.Create(request, dummyContext);
            // Verify outcome
            Assert.IsAssignableFrom<OmitSpecimen>(actual);
            // Teardown
        }
    }
}

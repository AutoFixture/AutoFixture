using System;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class ParameterNameSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Fixture setup
            // Exercise system
            var sut = new ParameterNameSpecification("someName");
            // Verify outcome
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithParameterNameShouldSetCorrespondingProperty()
        {
            // Fixture setup
            var parameterName = "someName";
            // Exercise system
            var sut = new ParameterNameSpecification(parameterName);
            // Verify outcome
            Assert.Equal(parameterName, sut.ParameterName);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullParameterNameShouldThrowArgumentNullException()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new ParameterNameSpecification(null));
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithNullRequestShouldThrowArgumentNullException()
        {
            // Fixture setup
            var sut = new ParameterNameSpecification("someName");
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.IsSatisfiedBy(null));
            // Teardown
        }

        [Theory]
        [InlineData("parameter", "parameter", true)]
        [InlineData("someName", "parameter", false)]
        [InlineData("Parameter", "parameter", false)]
        public void IsSatisfiedByWithRequestForParameterShouldReturnCorrectResult(
            string parameterName,
            string requestedName,
            bool expectedResult)
        {
            // Fixture setup
            var sut = new ParameterNameSpecification(parameterName);
            var request = typeof(SingleParameterType<object>)
                          .GetConstructor(new[] { typeof(object) })
                          .GetParameters()
                          .Single(p => p.Name == requestedName);
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithRequestForMemberOtherThanParameterWithSameNameShouldReturnFalse()
        {
            // Fixture setup
            var parameterName = "Parameter";
            var requestedName = "Parameter";
            var sut = new ParameterNameSpecification(parameterName);
            var request = typeof(SingleParameterType<object>)
                          .GetProperty(requestedName);
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Theory]
        [InlineData("aString")]
        [InlineData(9)]
        [InlineData(true)]
        [InlineData(typeof(object))]
        public void IsSatisfiedByWithInvalidRequestShouldReturnFalse(object request)
        {
            // Fixture setup
            var sut = new ParameterNameSpecification("someName");
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }
    }
}

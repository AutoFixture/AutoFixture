using System;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class FieldNameSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Fixture setup
            // Exercise system
            var sut = new FieldNameSpecification("someName");
            // Verify outcome
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithFieldNameShouldSetCorrespondingProperty()
        {
            // Fixture setup
            var fieldName = "someName";
            // Exercise system
            var sut = new FieldNameSpecification(fieldName);
            // Verify outcome
            Assert.Equal(fieldName, sut.FieldName);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullFieldNameShouldThrowArgumentNullException()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new FieldNameSpecification(null));
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithNullRequestShouldThrowArgumentNullException()
        {
            // Fixture setup
            var sut = new FieldNameSpecification("someName");
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.IsSatisfiedBy(null));
            // Teardown
        }

        [Theory]
        [InlineData("Field", "Field", true)]
        [InlineData("someName", "Field", false)]
        [InlineData("field", "Field", false)]
        public void IsSatisfiedByWithRequestForFieldShouldReturnCorrectResult(
            string fieldName,
            string requestedName,
            bool expectedResult)
        {
            // Fixture setup
            var sut = new FieldNameSpecification(fieldName);
            var request = typeof(FieldHolder<object>).GetField(requestedName);
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithRequestForMemberOtherThanFieldWithSameNameShouldReturnFalse()
        {
            // Fixture setup
            var sut = new FieldNameSpecification("Property");
            var request = typeof(PropertyHolder<object>).GetProperty("Property");
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
            var sut = new FieldNameSpecification("someName");
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }
    }
}

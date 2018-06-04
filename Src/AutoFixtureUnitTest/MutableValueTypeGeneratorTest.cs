using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class MutableValueTypeGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new MutableValueTypeGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Arrange
            var sut = new MutableValueTypeGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateWithNullContainerDoesNotThrow()
        {
            // Arrange
            var sut = new MutableValueTypeGenerator();
            // Act
            var dummyRequest = new object();

            // Assert (no exception indicates success)
            Assert.Null(Record.Exception(() => sut.Create(dummyRequest, null)));
        }

        [Fact]
        public void CreateWithNonValueTypeRequestWillReturnCorrectResult()
        {
            // Arrange
            var nonValueTypeRequest = typeof(NoSpecimen);
            var sut = new MutableValueTypeGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonValueTypeRequest, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithNotTypeRequestWillReturnCorrectResult()
        {
            // Arrange
            var nonValueTypeRequest = new object();
            var sut = new MutableValueTypeGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonValueTypeRequest, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithValueTypeWithoutConstructorRequestWillReturnCorrectResult()
        {
            // Arrange
            var valueTypeRequest = typeof(MutableValueTypeWithoutConstructor);
            var sut = new MutableValueTypeGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(valueTypeRequest, dummyContainer);
            // Assert
            Assert.IsType<MutableValueTypeWithoutConstructor>(result);
        }

        [Fact]
        public void CreateWithValueTypeRequestWillReturnCorrectResult()
        {
            // Arrange
            var valueTypeRequest = typeof(MutableValueType);
            var sut = new MutableValueTypeGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(valueTypeRequest, dummyContainer);
            // Assert
            Assert.IsType<NoSpecimen>(result);
        }
    }
}

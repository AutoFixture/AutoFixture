using System;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class DelegateGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new DelegateGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void NullDelegateSpecificationThrowsException()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new DelegateGenerator(null));
        }

        [Fact]
        public void DefaultDelegateSpecificationIsCorrect()
        {
            // Arrange
            // Act
            var sut = new DelegateGenerator();
            // Assert
            Assert.IsType<DelegateSpecification>(sut.Specification);
        }

        [Fact]
        public void CreateWithNullRequestReturnsNoSpecimen()
        {
            // Arrange
            var sut = new DelegateGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CustomSpecificationIsPreserved()
        {
            // Arrange
            var specification = new TrueRequestSpecification();
            // Act
            var sut = new DelegateGenerator(specification);
            // Assert
            Assert.Equal(specification, sut.Specification);
        }

        [Fact]
        public void CreateWithNullContainerThrowsArgumentNullException()
        {
            // Arrange
            var dummyRequest = new object();
            var sut = new DelegateGenerator();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => sut.Create(dummyRequest, null));
        }

        [Fact]
        public void CreateWithNonDelegateRequestReturnsNoSpecimen()
        {
            // Arrange
            var nonDelegateRequest = new object();
            var sut = new DelegateGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonDelegateRequest, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithVoidParameterlessDelegateRequestReturnsCorrectResult()
        {
            // Arrange
            var delegateRequest = typeof(Action);
            var sut = new DelegateGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(delegateRequest, dummyContainer);
            // Assert
            Assert.IsType<Action>(result);
        }

        [Fact]
        public void CreateWithVoidSingleObjectParameterDelegateRequestReturnsCorrectResult()
        {
            // Arrange
            var delegateRequest = typeof(Action<object>);
            var sut = new DelegateGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(delegateRequest, dummyContainer);
            // Assert
            Assert.IsType<Action<object>>(result);
        }

        [Fact]
        public void CreateWithVoidDoubleObjectParametersDelegateRequestReturnsCorrectResult()
        {
            // Arrange
            var delegateRequest = typeof(Action<object, object>);
            var sut = new DelegateGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(delegateRequest, dummyContainer);
            // Assert
            Assert.IsType<Action<object, object>>(result);
        }

        [Fact]
        public void CreateWithVoidSingleValueParameterDelegateRequestReturnsCorrectResult()
        {
            // Arrange
            var delegateRequest = typeof(Action<int>);
            var sut = new DelegateGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(delegateRequest, dummyContainer);
            // Assert
            Assert.IsType<Action<int>>(result);
        }

        [Fact]
        public void CreateWithVoidDoubleValueParametersDelegateRequestReturnsCorrectResult()
        {
            // Arrange
            var delegateRequest = typeof(Action<int, bool>);
            var sut = new DelegateGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(delegateRequest, dummyContainer);
            // Assert
            Assert.IsType<Action<int, bool>>(result);
        }

        [Fact]
        public void CreateWithVoidParameterlessDelegateRequestReturnsDelegateNotThrowing()
        {
            // Arrange
            var delegateRequest = typeof(Action);
            var sut = new DelegateGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(delegateRequest, dummyContainer);
            // Assert
            Assert.Null(Record.Exception(() => ((Action)result).Invoke()));
        }

        [Fact]
        public void CreateWithReturnObjectParameterlessDelegateRequestReturnsCorrectResult()
        {
            // Arrange
            var delegateRequest = typeof(Func<object>);
            var sut = new DelegateGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(delegateRequest, dummyContainer);
            // Assert
            Assert.IsType<Func<object>>(result);
        }

        [Fact]
        public void CreateWithReturnObjectSingleObjectParameterDelegateRequestReturnsCorrectResult()
        {
            // Arrange
            var delegateRequest = typeof(Func<object, object>);
            var sut = new DelegateGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(delegateRequest, dummyContainer);
            // Assert
            Assert.IsType<Func<object, object>>(result);
        }

        [Fact]
        public void CreateWithReturnObjectDoubleObjectParametersDelegateRequestReturnsCorrectResult()
        {
            // Arrange
            var delegateRequest = typeof(Func<object, object, object>);
            var sut = new DelegateGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(delegateRequest, dummyContainer);
            // Assert
            Assert.IsType<Func<object, object, object>>(result);
        }

        [Fact]
        public void CreateWithReturnObjectParameterlessDelegateRequestReturnsDelegateReturningObjectSpecimen()
        {
            // Arrange
            var delegateRequest = typeof(Func<object>);
            var expectedResult = new object();
            var sut = new DelegateGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext
            {
                OnResolve = arg => expectedResult
            };
            var result = sut.Create(delegateRequest, dummyContainer);
            // Assert
            var actualResult = ((Func<object>)result).Invoke();
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void CreateWithReturnValueParameterlessDelegateRequestReturnsCorrectResult()
        {
            // Arrange
            var delegateRequest = typeof(Func<int>);
            var sut = new DelegateGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(delegateRequest, dummyContainer);
            // Assert
            Assert.IsType<Func<int>>(result);
        }

        [Fact]
        public void CreateWithReturnValueParameterlessDelegateRequestReturnsDelegateReturningValueSpecimen()
        {
            // Arrange
            var delegateRequest = typeof(Func<int>);
            var expectedResult = 3;
            var sut = new DelegateGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext
            {
                OnResolve = arg => expectedResult
            };
            var result = sut.Create(delegateRequest, dummyContainer);
            // Assert
            var actualResult = ((Func<int>)result).Invoke();
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
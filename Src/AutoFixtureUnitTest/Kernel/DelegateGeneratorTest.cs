using System;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class DelegateGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new DelegateGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestReturnsNoSpecimen()
        {
            // Fixture setup
            var sut = new DelegateGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContainerThrowsArgumentNullException()
        {
            // Fixture setup
            var dummyRequest = new object();
            var sut = new DelegateGenerator();
            // Exercise system and verify outcome
            Assert.Throws(typeof(ArgumentNullException), () => sut.Create(dummyRequest, null));
            // Teardown
        }

        [Fact]
        public void CreateWithNonDelegateRequestReturnsNoSpecimen()
        {
            // Fixture setup
            var nonDelegateRequest = new object();
            var sut = new DelegateGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonDelegateRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithVoidParameterlessDelegateRequestReturnsCorrectResult()
        {
            // Fixture setup
            var delegateRequest = typeof(Action);
            var sut = new DelegateGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(delegateRequest, dummyContainer);
            // Verify outcome
            Assert.IsType<Action>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithVoidSingleObjectParameterDelegateRequestReturnsCorrectResult()
        {
            // Fixture setup
            var delegateRequest = typeof(Action<object>);
            var sut = new DelegateGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(delegateRequest, dummyContainer);
            // Verify outcome
            Assert.IsType<Action<object>>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithVoidDoubleObjectParametersDelegateRequestReturnsCorrectResult()
        {
            // Fixture setup
            var delegateRequest = typeof(Action<object, object>);
            var sut = new DelegateGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(delegateRequest, dummyContainer);
            // Verify outcome
            Assert.IsType<Action<object, object>>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithVoidSingleValueParameterDelegateRequestReturnsCorrectResult()
        {
            // Fixture setup
            var delegateRequest = typeof(Action<int>);
            var sut = new DelegateGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(delegateRequest, dummyContainer);
            // Verify outcome
            Assert.IsType<Action<int>>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithVoidDoubleValueParametersDelegateRequestReturnsCorrectResult()
        {
            // Fixture setup
            var delegateRequest = typeof(Action<int, bool>);
            var sut = new DelegateGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(delegateRequest, dummyContainer);
            // Verify outcome
            Assert.IsType<Action<int, bool>>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithVoidParameterlessDelegateRequestReturnsDelegateNotThrowing()
        {
            // Fixture setup
            var delegateRequest = typeof(Action);
            var sut = new DelegateGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(delegateRequest, dummyContainer);
            // Verify outcome
            Assert.Null(Record.Exception(() => ((Action)result).Invoke()));
            // Teardown
        }

        [Fact]
        public void CreateWithReturnObjectParameterlessDelegateRequestReturnsCorrectResult()
        {
            // Fixture setup
            var delegateRequest = typeof(Func<object>);
            var sut = new DelegateGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(delegateRequest, dummyContainer);
            // Verify outcome
            Assert.IsType<Func<object>>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithReturnObjectSingleObjectParameterDelegateRequestReturnsCorrectResult()
        {
            // Fixture setup
            var delegateRequest = typeof(Func<object, object>);
            var sut = new DelegateGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(delegateRequest, dummyContainer);
            // Verify outcome
            Assert.IsType<Func<object, object>>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithReturnObjectDoubleObjectParametersDelegateRequestReturnsCorrectResult()
        {
            // Fixture setup
            var delegateRequest = typeof(Func<object, object, object>);
            var sut = new DelegateGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(delegateRequest, dummyContainer);
            // Verify outcome
            Assert.IsType<Func<object, object, object>>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithReturnObjectParameterlessDelegateRequestReturnsDelegateReturningObjectSpecimen()
        {
            // Fixture setup
            var delegateRequest = typeof(Func<object>);
            var expectedResult = new object();
            var sut = new DelegateGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext
            {
                OnResolve = arg => expectedResult
            };
            var result = sut.Create(delegateRequest, dummyContainer);
            // Verify outcome
            var actualResult = ((Func<object>)result).Invoke();
            Assert.Equal(expectedResult, actualResult);
            // Teardown
        }

        [Fact]
        public void CreateWithReturnValueParameterlessDelegateRequestReturnsCorrectResult()
        {
            // Fixture setup
            var delegateRequest = typeof(Func<int>);
            var sut = new DelegateGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(delegateRequest, dummyContainer);
            // Verify outcome
            Assert.IsType<Func<int>>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithReturnValueParameterlessDelegateRequestReturnsDelegateReturningValueSpecimen()
        {
            // Fixture setup
            var delegateRequest = typeof(Func<int>);
            var expectedResult = 3;
            var sut = new DelegateGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext
            {
                OnResolve = arg => expectedResult
            };
            var result = sut.Create(delegateRequest, dummyContainer);
            // Verify outcome
            var actualResult = ((Func<int>)result).Invoke();
            Assert.Equal(expectedResult, actualResult);
            // Teardown
        }
    }
}
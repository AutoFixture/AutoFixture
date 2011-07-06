using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
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
        public void CreateWithNullContainerDoesNotThrow()
        {
            // Fixture setup
            var sut = new DelegateGenerator();
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Verify outcome (no exception indicates success)
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
            var expectedResult = new NoSpecimen(nonDelegateRequest);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithParameterlessActionDelegateRequestReturnsCorrectResult()
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
        public void CreateWithSingleObjectParameterActionDelegateRequestReturnsCorrectResult()
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
        public void CreateWithDoubleObjectParametersActionDelegateRequestReturnsCorrectResult()
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
        public void CreateWithSingleValueParameterActionDelegateRequestReturnsCorrectResult()
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
        public void CreateWithDoubleValueParametersActionDelegateRequestReturnsCorrectResult()
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
        public void CreateWithParameterlessObjectResultFunctionDelegateRequestReturnsCorrectResult()
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
        public void CreateWithSingleObjectParameterObjectResultFunctionDelegateRequestReturnsCorrectResult()
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
        public void CreateWithDoubleObjectParametersObjectResultFunctionDelegateRequestReturnsCorrectResult()
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
        public void CreateWithParameterlessObjectResultFunctionDelegateRequestReturnsDelegateReturningObjectSpecimen()
        {
            // Fixture setup
            var delegateRequest = typeof(Func<object>);
            var expectedResult = new object();
            var sut = new DelegateGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext()
            {
                OnResolve = arg => expectedResult
            };
            var result = sut.Create(delegateRequest, dummyContainer);
            // Verify outcome
            var actualResult = ((Func<object>)result).Invoke();
            Assert.Equal(expectedResult, actualResult);
            // Teardown
        }

        [Fact(Skip = "Functionality not yet implemented.")]
        public void CreateWithParameterlessValueResultFunctionDelegateRequestReturnsCorrectResult()
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
    }
}
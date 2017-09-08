using System;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class FieldRequestRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new FieldRequestRelay();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new FieldRequestRelay();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContainerWillThrow()
        {
            // Fixture setup
            var sut = new FieldRequestRelay();
            var dummyRequest = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
            // Teardown
        }

        [Fact]
        public void CreateFromNonFieldRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var nonFieldRequest = new object();
            var sut = new FieldRequestRelay();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonFieldRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateFromFieldRequestWillReturnCorrectResultWhenContainerCannotSatisfyRequest()
        {
            // Fixture setup
            var fieldInfo = typeof(FieldHolder<object>).GetField("Field");
            var container = new DelegatingSpecimenContext { OnResolve = r => new NoSpecimen() };
            var sut = new FieldRequestRelay();
            // Exercise system
            var result = sut.Create(fieldInfo, container);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateFromFieldRequestWillReturnCorrectResultWhenContainerCanSatisfyRequest()
        {
            // Fixture setup
            var expectedSpecimen = new object();
            var fieldInfo = typeof(FieldHolder<object>).GetField("Field");
            var container = new DelegatingSpecimenContext { OnResolve = r => expectedSpecimen };
            var sut = new FieldRequestRelay();
            // Exercise system
            var result = sut.Create(fieldInfo, container);
            // Verify outcome
            Assert.Equal(expectedSpecimen, result);
            // Teardown
        }

        [Fact]
        public void CreateFromFieldRequestWillCorrectlyInvokeContainer()
        {
            // Fixture setup
            var sut = new FieldRequestRelay();
            var fieldInfo = typeof(FieldHolder<object>).GetField("Field");
            var expectedRequest = new SeededRequest(fieldInfo.FieldType, fieldInfo.Name);

            var mockVerified = false;
            var containerMock = new DelegatingSpecimenContext();
            containerMock.OnResolve = r => mockVerified = expectedRequest.Equals(r);
            // Exercise system
            sut.Create(fieldInfo, containerMock);
            // Verify outcome
            Assert.True(mockVerified, "Mock verification");
            // Teardown
        }
    }
}

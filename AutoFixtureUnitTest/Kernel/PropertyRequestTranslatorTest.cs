using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class PropertyRequestTranslatorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new PropertyRequestTranslator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new PropertyRequestTranslator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
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
            var sut = new PropertyRequestTranslator();
            var dummyRequest = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
            // Teardown
        }

        [Fact]
        public void CreateFromNonPropertyRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var nonParameterRequest = new object();
            var sut = new PropertyRequestTranslator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(nonParameterRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(nonParameterRequest);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateFromPropertyRequestWillReturnCorrectResultWhenContainerCannotSatisfyRequest()
        {
            // Fixture setup
            var propertyInfo = typeof(PropertyHolder<object>).GetProperty("Property");
            var container = new DelegatingSpecimenContainer { OnResolve = r => new NoSpecimen(propertyInfo) };
            var sut = new PropertyRequestTranslator();
            // Exercise system
            var result = sut.Create(propertyInfo, container);
            // Verify outcome
            var expectedResult = new NoSpecimen(propertyInfo);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateFromPropertyRequestWillReturnCorrectResultWhenContainerCanSatisfyRequest()
        {
            // Fixture setup
            var expectedSpecimen = new object();
            var propertyInfo = typeof(PropertyHolder<object>).GetProperty("Property");
            var container = new DelegatingSpecimenContainer { OnResolve = r => expectedSpecimen };
            var sut = new PropertyRequestTranslator();
            // Exercise system
            var result = sut.Create(propertyInfo, container);
            // Verify outcome
            Assert.Equal(expectedSpecimen, result);
            // Teardown
        }

        [Fact]
        public void CreateFromParameterRequestWillCorrectlyInvokeContainer()
        {
            // Fixture setup
            var sut = new PropertyRequestTranslator();
            var propertyInfo = typeof(PropertyHolder<object>).GetProperty("Property");
            var expectedRequest = new SeededRequest(propertyInfo.PropertyType, propertyInfo.Name);

            var mockVerified = false;
            var containerMock = new DelegatingSpecimenContainer();
            containerMock.OnResolve = r =>
            {
                Assert.Equal(expectedRequest, r);
                mockVerified = true;
                return null;
            };
            // Exercise system
            sut.Create(propertyInfo, containerMock);
            // Verify outcome
            Assert.True(mockVerified, "Mock verification");
            // Teardown
        }
    }
}

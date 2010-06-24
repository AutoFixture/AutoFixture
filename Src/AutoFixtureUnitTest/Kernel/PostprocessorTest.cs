using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class PostprocessorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            Action<object> dummyAction = s => { };
            // Exercise system
            var sut = new Postprocessor(dummyBuilder, dummyAction);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsPostProcessor()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            Action<object> dummyAction = s => { };
            // Exercise system
            var sut = new Postprocessor(dummyBuilder, dummyAction);
            // Verify outcome
            Assert.IsAssignableFrom<Postprocessor<object>>(sut);
            // Teardown
        }

        [Fact]
        public void SutCanBeInitializedWithDoubleAction()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            Action<object, ISpecimenContext> dummyAction = (s, c) => { };
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => new Postprocessor(dummyBuilder, dummyAction));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullBuilderThrows()
        {
            // Fixture setup
            Action<object> dummyAction = s => { };
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new Postprocessor(null, dummyAction));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullActionThrows()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new Postprocessor(dummyBuilder, (Action<object>)null));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullSpecificationThrows()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            Action<object, ISpecimenContext> dummyAction = (s, c) => { };
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new Postprocessor(dummyBuilder, dummyAction, null));
            // Teardown
        }

        [Fact]
        public void BuilderIsCorrect()
        {
            // Fixture setup
            var expectedBuilder = new DelegatingSpecimenBuilder();
            Action<object> dummyAction = s => { };

            var sut = new Postprocessor<object>(expectedBuilder, dummyAction);
            // Exercise system
            ISpecimenBuilder result = sut.Builder;
            // Verify outcome
            Assert.Equal(expectedBuilder, result);
            // Teardown
        }

        [Fact]
        public void ActionIsCorrect()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            Action<object, ISpecimenContext> expectedAction = (s, c) => { };

            var sut = new Postprocessor<object>(dummyBuilder, expectedAction);
            // Exercise system
            Action<object, ISpecimenContext> result = sut.Action;
            // Verify outcome
            Assert.Equal(expectedAction, result);
            // Teardown
        }

        [Fact]
        public void SpecificationIsCorrect()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            Action<object, ISpecimenContext> dummyAction = (s, c) => { };
            var expectedSpec = new DelegatingRequestSpecification();

            var sut = new Postprocessor<object>(dummyBuilder, dummyAction, expectedSpec);
            // Exercise system
            IRequestSpecification result = sut.Specification;
            // Verify outcome
            Assert.Equal(expectedSpec, result);
            // Teardown
        }

        [Fact]
        public void CreateInvokesDecoratedBuilderWithCorrectParameters()
        {
            // Fixture setup
            var expectedRequest = new object();
            var expectedContainer = new DelegatingSpecimenContext();

            var verified = false;
            var builderMock = new DelegatingSpecimenBuilder { OnCreate = (r, c) => verified = r == expectedRequest && c == expectedContainer };

            Action<object> dummyAction = s => { };
            var sut = new Postprocessor(builderMock, dummyAction);
            // Exercise system
            sut.Create(expectedRequest, expectedContainer);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResult()
        {
            // Fixture setup
            var expectedResult = new object();
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedResult };
            Action<object> dummyAction = s => { };
            var sut = new Postprocessor(builder, dummyAction);
            // Exercise system
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContainer);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateInvokesActionWithCreatedSpecimen()
        {
            // Fixture setup
            var expectedSpecimen = new object();
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedSpecimen };

            var verified = false;
            Action<object> mock = s => verified = s == expectedSpecimen;

            var sut = new Postprocessor(builder, mock);
            // Exercise system
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(dummyRequest, dummyContainer);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }
    }
}

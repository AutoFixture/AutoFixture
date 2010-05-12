using System;
using Ploeh.AutoFixture;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixtureUnitTest
{
    using System.Collections.Generic;
    using Xunit;

    public class LatentObjectBuilderTest
    {
        [Fact]
        public void CreateAnonymousWillCreateObject()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            object result = sut.Build<object>().CreateAnonymous();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousAfterDefiningConstructorWithZeroParametersWillReturnDefinedObject()
        {
            // Fixture setup
            var sut = new Fixture();
            object expectedObject = new object();
            // Exercise system
            var result = sut.Build<object>()
                .WithConstructor(() => expectedObject)
                .CreateAnonymous();
            // Verify outcome
            Assert.Equal<object>(expectedObject, result);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousAfterDefiningConstructorWithOneParameterWillReturnDefinedObject()
        {
            // Fixture setup
            var sut = new Fixture();
            SingleParameterType<object> expectedObject = new SingleParameterType<object>(new object());
            // Exercise system
            var result = sut.Build<SingleParameterType<object>>()
                .WithConstructor<object>(obj => expectedObject)
                .CreateAnonymous();
            // Verify outcome
            Assert.Equal<SingleParameterType<object>>(expectedObject, result);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousAfterDefiningConstructorWithTwoParametersWillReturnDefinedObject()
        {
            // Fixture setup
            var sut = new Fixture();
            DoubleParameterType<object, object> expectedObject = new DoubleParameterType<object, object>(new object(), new object());
            // Exercise system
            var result = sut.Build<DoubleParameterType<object, object>>()
                .WithConstructor<object, object>((o1, o2) => expectedObject)
                .CreateAnonymous();
            // Verify outcome
            Assert.Equal<DoubleParameterType<object, object>>(expectedObject, result);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousAfterDefiningConstructorWithThreeParametersWillReturnDefinedObject()
        {
            // Fixture setup
            var sut = new Fixture();
            TripleParameterType<object, object, object> expectedObject = new TripleParameterType<object, object, object>(new object(), new object(), new object());
            // Exercise system
            var result = sut.Build<TripleParameterType<object, object, object>>()
                .WithConstructor<object, object, object>((o1, o2, o3) => expectedObject)
                .CreateAnonymous();
            // Verify outcome
            Assert.Equal<TripleParameterType<object, object, object>>(expectedObject, result);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousAfterDefiningConstructorWithFourParametersWillReturnDefinedObject()
        {
            // Fixture setup
            var sut = new Fixture();
            QuadrupleParameterType<object, object, object, object> expectedObject = new QuadrupleParameterType<object, object, object, object>(new object(), new object(), new object(), new object());
            // Exercise system
            var result = sut.Build<QuadrupleParameterType<object, object, object, object>>()
                .WithConstructor<object, object, object, object>((o1, o2, o3, o4) => expectedObject)
                .CreateAnonymous();
            // Verify outcome
            Assert.Equal<QuadrupleParameterType<object, object, object, object>>(expectedObject, result);
            // Teardown
        }
    }
}

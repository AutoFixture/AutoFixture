using System;
using Ploeh.AutoFixture;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixtureUnitTest
{
    using System.Collections.Generic;
    using Xunit;

    public class LatentObjectBuilderTest
    {
        public LatentObjectBuilderTest()
        {
        }

        [Fact]
        public void CreateAnonymousWillCreateObject()
        {
            // Fixture setup
            ObjectBuilder<object> sut = LatentObjectBuilderTest.CreateSut<object>();
            // Exercise system
            object result = sut.CreateAnonymous();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousAfterDefiningConstructorWithZeroParametersWillReturnDefinedObject()
        {
            // Fixture setup
            object expectedObject = new object();
            LatentObjectBuilder<object> sut = LatentObjectBuilderTest.CreateSut<object>();
            // Exercise system
            ConstructingObjectBuilder<object> result = sut.WithConstructor(() => expectedObject);
            // Verify outcome
            Assert.Equal<object>(expectedObject, result.CreateAnonymous());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousAfterDefiningConstructorWithOneParameterWillReturnDefinedObject()
        {
            // Fixture setup
            SingleParameterType<object> expectedObject = new SingleParameterType<object>(new object());
            LatentObjectBuilder<SingleParameterType<object>> sut = LatentObjectBuilderTest.CreateSut<SingleParameterType<object>>();
            // Exercise system
            ConstructingObjectBuilder<SingleParameterType<object>> result = sut.WithConstructor<object>(obj => expectedObject);
            // Verify outcome
            Assert.Equal<SingleParameterType<object>>(expectedObject, result.CreateAnonymous());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousAfterDefiningConstructorWithTwoParametersWillReturnDefinedObject()
        {
            // Fixture setup
            DoubleParameterType<object, object> expectedObject = new DoubleParameterType<object, object>(new object(), new object());
            LatentObjectBuilder<DoubleParameterType<object, object>> sut = LatentObjectBuilderTest.CreateSut<DoubleParameterType<object, object>>();
            // Exercise system
            ConstructingObjectBuilder<DoubleParameterType<object, object>> result = sut.WithConstructor<object, object>((o1, o2) => expectedObject);
            // Verify outcome
            Assert.Equal<DoubleParameterType<object, object>>(expectedObject, result.CreateAnonymous());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousAfterDefiningConstructorWithThreeParametersWillReturnDefinedObject()
        {
            // Fixture setup
            TripleParameterType<object, object, object> expectedObject = new TripleParameterType<object, object, object>(new object(), new object(), new object());
            LatentObjectBuilder<TripleParameterType<object, object, object>> sut = LatentObjectBuilderTest.CreateSut<TripleParameterType<object, object, object>>();
            // Exercise system
            ConstructingObjectBuilder<TripleParameterType<object, object, object>> result = sut.WithConstructor<object, object, object>((o1, o2, o3) => expectedObject);
            // Verify outcome
            Assert.Equal<TripleParameterType<object, object, object>>(expectedObject, result.CreateAnonymous());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousAfterDefiningConstructorWithFourParametersWillReturnDefinedObject()
        {
            // Fixture setup
            QuadrupleParameterType<object, object, object, object> expectedObject = new QuadrupleParameterType<object, object, object, object>(new object(), new object(), new object(), new object());
            LatentObjectBuilder<QuadrupleParameterType<object, object, object, object>> sut = LatentObjectBuilderTest.CreateSut<QuadrupleParameterType<object, object, object, object>>();
            // Exercise system
            ConstructingObjectBuilder<QuadrupleParameterType<object, object, object, object>> result = sut.WithConstructor<object, object, object, object>((o1, o2, o3, o4) => expectedObject);
            // Verify outcome
            Assert.Equal<QuadrupleParameterType<object, object, object, object>>(expectedObject, result.CreateAnonymous());
            // Teardown
        }

        private static LatentObjectBuilder<T> CreateSut<T>()
        {
            return new Fixture().Build<T>();
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixtureUnitTest
{
    using System.Collections.Generic;

    [TestClass]
    public class LatentObjectBuilderTest
    {
        public LatentObjectBuilderTest()
        {
        }

        [TestMethod]
        public void CreateAnonymousWillCreateObject()
        {
            // Fixture setup
            ObjectBuilder<object> sut = LatentObjectBuilderTest.CreateSut<object>();
            // Exercise system
            object result = sut.CreateAnonymous();
            // Verify outcome
            Assert.IsNotNull(result, "Created object");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousAfterDefiningConstructorWithZeroParametersWillReturnDefinedObject()
        {
            // Fixture setup
            object expectedObject = new object();
            LatentObjectBuilder<object> sut = LatentObjectBuilderTest.CreateSut<object>();
            // Exercise system
            ConstructingObjectBuilder<object> result = sut.WithConstructor(() => expectedObject);
            // Verify outcome
            Assert.AreEqual<object>(expectedObject, result.CreateAnonymous(), "Created object");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousAfterDefiningConstructorWithOneParameterWillReturnDefinedObject()
        {
            // Fixture setup
            SingleParameterType<object> expectedObject = new SingleParameterType<object>(new object());
            LatentObjectBuilder<SingleParameterType<object>> sut = LatentObjectBuilderTest.CreateSut<SingleParameterType<object>>();
            // Exercise system
            ConstructingObjectBuilder<SingleParameterType<object>> result = sut.WithConstructor<object>(obj => expectedObject);
            // Verify outcome
            Assert.AreEqual<SingleParameterType<object>>(expectedObject, result.CreateAnonymous(), "Created object");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousAfterDefiningConstructorWithTwoParametersWillReturnDefinedObject()
        {
            // Fixture setup
            DoubleParameterType<object, object> expectedObject = new DoubleParameterType<object, object>(new object(), new object());
            LatentObjectBuilder<DoubleParameterType<object, object>> sut = LatentObjectBuilderTest.CreateSut<DoubleParameterType<object, object>>();
            // Exercise system
            ConstructingObjectBuilder<DoubleParameterType<object, object>> result = sut.WithConstructor<object, object>((o1, o2) => expectedObject);
            // Verify outcome
            Assert.AreEqual<DoubleParameterType<object, object>>(expectedObject, result.CreateAnonymous(), "Created object");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousAfterDefiningConstructorWithThreeParametersWillReturnDefinedObject()
        {
            // Fixture setup
            TripleParameterType<object, object, object> expectedObject = new TripleParameterType<object, object, object>(new object(), new object(), new object());
            LatentObjectBuilder<TripleParameterType<object, object, object>> sut = LatentObjectBuilderTest.CreateSut<TripleParameterType<object, object, object>>();
            // Exercise system
            ConstructingObjectBuilder<TripleParameterType<object, object, object>> result = sut.WithConstructor<object, object, object>((o1, o2, o3) => expectedObject);
            // Verify outcome
            Assert.AreEqual<TripleParameterType<object, object, object>>(expectedObject, result.CreateAnonymous(), "Created object");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousAfterDefiningConstructorWithFourParametersWillReturnDefinedObject()
        {
            // Fixture setup
            QuadrupleParameterType<object, object, object, object> expectedObject = new QuadrupleParameterType<object, object, object, object>(new object(), new object(), new object(), new object());
            LatentObjectBuilder<QuadrupleParameterType<object, object, object, object>> sut = LatentObjectBuilderTest.CreateSut<QuadrupleParameterType<object, object, object, object>>();
            // Exercise system
            ConstructingObjectBuilder<QuadrupleParameterType<object, object, object, object>> result = sut.WithConstructor<object, object, object, object>((o1, o2, o3, o4) => expectedObject);
            // Verify outcome
            Assert.AreEqual<QuadrupleParameterType<object, object, object, object>>(expectedObject, result.CreateAnonymous(), "Created object");
            // Teardown
        }

        private static LatentObjectBuilder<T> CreateSut<T>()
        {
            Fixture f = new Fixture();
#pragma warning disable 618
            return new LatentObjectBuilder<T>(f.TypeMappings, new ThrowingRecursionHandler(), f.RepeatCount, f.OmitAutoProperties, t => null);
#pragma warning restore 618
        }
    }
}

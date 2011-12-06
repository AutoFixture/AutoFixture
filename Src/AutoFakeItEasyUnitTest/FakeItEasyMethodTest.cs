using System;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixture.AutoFakeItEasy.UnitTest
{
    public class FakeItEasyMethodTest
    {
        [Fact]
        public void SutImplementsISpecimenBuilder()
        {
            // Exercise system
            Action dummyDelegate = delegate { };
            var sut = new FakeItEasyMethod(dummyDelegate.Method, Enumerable.Empty<ParameterInfo>().ToArray());
            // Verify outcome
            Assert.IsAssignableFrom<IMethod>(sut);
            // Teardown
        }

        [Fact]
        public void ConstructorWithNullConstructorMethodThrows()
        {
            // Exercise system
            Assert.Throws<ArgumentNullException>(() =>
                new FakeItEasyMethod(null, Enumerable.Empty<ParameterInfo>().ToArray()));
        }

        [Fact]
        public void ConstructorWithNullParameterInfoArray()
        {
            // Exercise system
            Action dummyDelegate = delegate { };
            Assert.Throws<ArgumentNullException>(() =>
                new FakeItEasyMethod(dummyDelegate.Method, null));
        }

        [Fact]
        public void ParametersIsCorrect()
        {
            // Fixture setup
            Action dummyDelegate = delegate { };
            var parameters = Enumerable.Empty<ParameterInfo>();
            var sut = new FakeItEasyMethod(dummyDelegate.Method, parameters.ToArray());
            // Exercise system
            var result = sut.Parameters;
            // Verify outcome
            Assert.Equal(parameters, result);
            // Teardown
        }

        [Fact]
        public void InvokeReturnsCorrectResult()
        {
            // Fixture setup
            Func<object[], object[]> func = p => p;
            var sut = new FakeItEasyMethod(func.Method, Enumerable.Empty<ParameterInfo>().ToArray());
            // Exercise system
            var args = new object[] { "1984" };
            var result = (object[])sut.Invoke(args);
            // Verify outcome
            var expected = func(args);
            Assert.True(expected.SequenceEqual(result));
            // Teardown
        }
    }
}

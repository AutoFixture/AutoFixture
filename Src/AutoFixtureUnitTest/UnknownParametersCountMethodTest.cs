using System;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class UnknownParametersCountMethodTest
    {
        [Fact]
        public void SutIsMethod()
        {
            // Fixture setup
            Action<object> dummy = x => { };
            // Exercise system
            var sut = new UnknownParametersCountMethod(new StaticMethod(dummy.Method), dummy.Method.GetParameters());
            // Verify outcome
            Assert.IsAssignableFrom<IMethod>(sut);
        }

        [Fact]
        public void InitializeWithNullMethodThrows()
        {
            // Fixture setup
            Action dummy = delegate { };
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new UnknownParametersCountMethod(null, dummy.Method.GetParameters()));
        }

        [Fact]
        public void InitializeWithNullMethodParametersThrows()
        {
            // Fixture setup
            Action<object> dummy = x => { };
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new UnknownParametersCountMethod(new StaticMethod(dummy.Method), null));
        }

        [Fact]
        public void InitializeWithMethodWithNoParameterThrows()
        {
            // Fixture setup
            Action dummy = () => { };
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() =>
                new UnknownParametersCountMethod(new StaticMethod(dummy.Method), dummy.Method.GetParameters()));
        }

        [Fact]
        public void InitializeWithMethodWithMultipleParametersThrows()
        {
            // Fixture setup
            Action<object, object> dummy = (x, y) => { };
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() =>
                new UnknownParametersCountMethod(new StaticMethod(dummy.Method), dummy.Method.GetParameters()));
        }

        [Fact]
        public void InitializeWithMethodWithOneParameterThrows_WhenTheParameterIsOfWrongType()
        {
            // Fixture setup
            Action<string> dummy = x => { };
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() =>
                new UnknownParametersCountMethod(new StaticMethod(dummy.Method), dummy.Method.GetParameters()));
        }

        [Fact]
        public void ParametersIsCorrect()
        {
            // Fixture setup
            Action<object> decorated = x => { };
            Action<string, string, int> dummy = (x, y, z) => { };
            var expectedParameters = dummy.Method.GetParameters();
            var sut = new UnknownParametersCountMethod(new StaticMethod(decorated.Method), expectedParameters);
            // Exercise system
            var result = sut.Parameters;
            // Verify outcome
            Assert.True(expectedParameters.SequenceEqual(result));
        }

        [Fact]
        public void InvokingMethodPassesAllParametersAsObjectArrayToDecoratedMethod()
        {
            // Fixture setup
            object passedParameter = null;
            Action<object> decorated = x => { passedParameter = x; };
            Action<string, string, int> dummy = (x, y, z) => { };
            var sut = new UnknownParametersCountMethod(new InstanceMethod(decorated.Method, decorated.Target), dummy.Method.GetParameters());
            // Exercise system
            var parameters = new object[] { "Lorem", "ipsum", 42 };
            sut.Invoke(parameters);
            // Verify outcome
            Assert.Equal(parameters, passedParameter);
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Fixture setup
            var method = ((Action<object>)delegate { }).Method;
            // Exercise system
            var sut = new UnknownParametersCountMethod(new StaticMethod(method), method.GetParameters());
            // Verify outcome
            Assert.IsAssignableFrom<IEquatable<UnknownParametersCountMethod>>(sut);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Fixture setup
            var method = ((Action<object>)delegate { }).Method;
            var sut = new UnknownParametersCountMethod(new StaticMethod(method), method.GetParameters());
            // Exercise system
            var result = sut.Equals((object)null);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualNullSut()
        {
            // Fixture setup
            var method = ((Action<object>)delegate { }).Method;
            var sut = new UnknownParametersCountMethod(new StaticMethod(method), method.GetParameters());
            // Exercise system
            var result = sut.Equals((UnknownParametersCountMethod)null);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualSomeOtherObject()
        {
            // Fixture setup
            var method = ((Action<object>)delegate { }).Method;
            var sut = new UnknownParametersCountMethod(new StaticMethod(method), method.GetParameters());
            // Exercise system
            var result = sut.Equals(new object());
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWithDifferentMethod()
        {
            // Fixture setup
            var method = ((Action<object>)delegate { }).Method;
            var sut = new StaticMethod(method);

            var otherMethod = ((Action<int>)delegate { }).Method;
            object other = new StaticMethod(otherMethod);

            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWithDifferentParameters()
        {
            // Fixture setup
            var method = ((Action<object>)delegate { }).Method;
            var sut = new UnknownParametersCountMethod(new StaticMethod(method), method.GetParameters());

            var otherParameters = ((Action<int>)delegate { }).Method.GetParameters();
            object other = new UnknownParametersCountMethod(new StaticMethod(method), otherParameters);

            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherObjectWithSameMethodAndSameParameters()
        {
            var method = ((Action<object>)delegate { }).Method;
            var parameters = method.GetParameters();
            var sut = new UnknownParametersCountMethod(new StaticMethod(method), parameters);
            object other = new UnknownParametersCountMethod(new StaticMethod(method), parameters);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void GetHashCodeReturnsCorrectResult()
        {
            // Fixture setup
            var method = ((Action<object>)delegate { }).Method;
            var decorated = new StaticMethod(method);
            var parameterInfos = method.GetParameters();
            var sut = new UnknownParametersCountMethod(decorated, parameterInfos);
            // Exercise system
            var result = sut.GetHashCode();
            // Verify outcome
            var expectedHasCode = decorated.GetHashCode() ^ parameterInfos.Aggregate(0, (current, parameter) => current + parameter.GetHashCode());
            Assert.Equal(expectedHasCode, result);
            // Teardown
        }
    }
}
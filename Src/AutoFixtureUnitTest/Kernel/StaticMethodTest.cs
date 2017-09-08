using System;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class StaticMethodTest
    {
        [Fact]
        public void SutIsMethod()
        {
            // Fixture setup
            Action dummy = delegate { };
            // Exercise system
            var sut = new StaticMethod(dummy.GetMethodInfo());
            // Verify outcome
            Assert.IsAssignableFrom<IMethod>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeOneParamsConstructorWithNullMethodInfoThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new StaticMethod(null));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullMethodInfoThrows()
        {
            // Fixture setup
            Action dummy = delegate { };
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new StaticMethod(null, dummy.GetMethodInfo().GetParameters()));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullMethodParametersThrows()
        {
            // Fixture setup
            Action dummy = delegate { };
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new StaticMethod(dummy.GetMethodInfo(), null));
            // Teardown
        }

        [Fact]
        public void MethodIsCorrect()
        {
            // Fixture setup
            var expectedMethod = ((Action)delegate { }).GetMethodInfo();
            var sut = new StaticMethod(expectedMethod);
            // Exercise system
            var result = sut.Method;
            // Verify outcome
            Assert.Equal(expectedMethod, result);
            // Teardown
        }

        [Fact]
        public void ParametersIsCorrectWhenPassedOnOneParamsConstructor()
        {
            // Fixture setup
            Action<object> dummy = delegate { };
            var expectedParameters = dummy.GetMethodInfo().GetParameters();
            var sut = new StaticMethod(dummy.GetMethodInfo());
            // Exercise system
            var result = sut.Parameters;
            // Verify outcome
            Assert.True(expectedParameters.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void ParametersIsCorrect()
        {
            // Fixture setup
            Action<int, double> dummy = delegate { };
            var expectedParameters = dummy.GetMethodInfo().GetParameters();
            var sut = new StaticMethod(dummy.GetMethodInfo(), expectedParameters);
            // Exercise system
            var result = sut.Parameters;
            // Verify outcome
            Assert.True(expectedParameters.SequenceEqual(result));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(TypeWithFactoryMethod), 0, new object[] { })]
        [InlineData(typeof(TypeWithFactoryMethod), 1, new object[] { "abc" })]
        [InlineData(typeof(TypeWithFactoryMethod), 2, new object[] { new[] { "ab", "c" } })]
        public void InvokeWithFactoryMethodReturnsCorrectResult(Type targetType, int index, object values)
        {
            // Fixture setup
            var method = (from mi in targetType
                              .GetMethods(BindingFlags.Static | BindingFlags.Public)
                          where mi.ReturnType == targetType
                          select mi).ElementAt(index);
            var sut = new StaticMethod(method);

            // Exercise system
            var result = sut.Invoke((object[])values);
            // Verify outcome
            Assert.IsAssignableFrom(targetType, result);
            // Teardown
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Fixture setup
            var method = ((Action<object>)delegate { }).GetMethodInfo();
            // Exercise system
            var sut = new StaticMethod(method);
            // Verify outcome
            Assert.IsAssignableFrom<IEquatable<StaticMethod>>(sut);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Fixture setup
            var method = ((Action<object>)delegate { }).GetMethodInfo();
            var sut = new StaticMethod(method);
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
            var method = ((Action<object>)delegate { }).GetMethodInfo();
            var sut = new StaticMethod(method);
            // Exercise system
            var result = sut.Equals((StaticMethod)null);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualSomeOtherObject()
        {
            // Fixture setup
            var method = ((Action<object>)delegate { }).GetMethodInfo();
            var sut = new StaticMethod(method);
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
            var method = ((Action<object>)delegate { }).GetMethodInfo();
            var sut = new StaticMethod(method);

            var otherMethod = ((Action<int>)delegate { }).GetMethodInfo();
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
            var method = ((Action<object>)delegate { }).GetMethodInfo();
            var sut = new StaticMethod(method, method.GetParameters());

            var otherParameters = ((Action<int>)delegate { }).GetMethodInfo().GetParameters();
            object other = new StaticMethod(method, otherParameters);

            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherObjectWithSameMethod()
        {
            var method = ((Action<object>)delegate { }).GetMethodInfo();
            var sut = new StaticMethod(method);
            object other = new StaticMethod(method);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherObjectWithSameMethodAndSameParameters()
        {
            var method = ((Action<object>)delegate { }).GetMethodInfo();
            var parameters = method.GetParameters();
            var sut = new StaticMethod(method, parameters);
            object other = new StaticMethod(method, parameters);
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
            var method = ((Action<object>)delegate { }).GetMethodInfo();
            var sut = new StaticMethod(method);
            // Exercise system
            var result = sut.GetHashCode();
            // Verify outcome
            var expectedHasCode = method.GetHashCode() ^ method.GetParameters().Aggregate(0, (current, parameter) => current + parameter.GetHashCode());
            Assert.Equal(expectedHasCode, result);
            // Teardown
        }
    }
}

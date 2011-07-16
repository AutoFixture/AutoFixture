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
        [Theory]
        [InlineData(typeof(TypeWithFactoryMethod))]
        public void SutIsMethod(Type targetType)
        {
            // Fixture setup
            var method = (from mi in targetType
                              .GetMethods(BindingFlags.Static | BindingFlags.Public)
                          where mi.ReturnType == targetType 
                          select mi).First();
            // Exercise system
            var sut = new StaticMethod(method);
            // Verify outcome
            Assert.IsAssignableFrom<IMethod>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullMethodInfoThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new StaticMethod(null));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(TypeWithFactoryMethod))]
        public void MethodIsCorrect(Type targetType)
        {
            // Fixture setup
            var expectedMethod = (from mi in targetType
                                      .GetMethods(BindingFlags.Static | BindingFlags.Public)
                                  where mi.ReturnType == targetType
                                  select mi).First();
            var sut = new StaticMethod(expectedMethod);
            // Exercise system
            MethodInfo result = sut.Method;
            // Verify outcome
            Assert.Equal(expectedMethod, result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(TypeWithFactoryMethod), 0)]
        [InlineData(typeof(TypeWithFactoryMethod), 1)]
        public void ParametersReturnsCorrectResult(Type targetType, int index)
        {
            // Fixture setup
            var method = (from mi in targetType
                              .GetMethods(BindingFlags.Static | BindingFlags.Public)
                          where mi.ReturnType == targetType
                          select mi).ElementAt(index);
            var expectedParameters = method.GetParameters();
            var sut = new StaticMethod(method);
            // Exercise system
            var result = sut.Parameters;
            // Verify outcome
            Assert.True(expectedParameters.SequenceEqual(result));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(TypeWithFactoryMethod))]
        public void InvokeWithParameterlessFactoryMethodReturnsCorrectResult(Type targetType)
        {
            // Fixture setup
            var method = (from mi in targetType
                              .GetMethods(BindingFlags.Static | BindingFlags.Public)
                          where mi.ReturnType == targetType
                          select mi).First();
            var sut = new StaticMethod(method);
            // Exercise system
            var result = sut.Invoke(Enumerable.Empty<object>());
            // Verify outcome
            Assert.IsAssignableFrom(targetType, result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(TypeWithFactoryMethod), "")]
        public void InvokeWithSingleParameterReturnsCorrectResult(Type targetType, object parameter)
        {
            // Fixture setup
            var method = (from mi in targetType
                              .GetMethods(BindingFlags.Static | BindingFlags.Public)
                          where mi.ReturnType == targetType
                          select mi).ElementAt(1); // Index of method with single parameter.
            var sut = new StaticMethod(method);
            // Exercise system
            var result = sut.Invoke(new[] { parameter });
            // Verify outcome
            Assert.IsAssignableFrom(targetType, result);
            // Teardown
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Fixture setup
            var method = (from mi in typeof(TypeWithFactoryMethod)
                              .GetMethods(BindingFlags.Static | BindingFlags.Public)
                          select mi).First();
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
            var method = (from mi in typeof(TypeWithFactoryMethod)
                              .GetMethods(BindingFlags.Static | BindingFlags.Public)
                          select mi).First();
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
            var method = (from mi in typeof(TypeWithFactoryMethod)
                              .GetMethods(BindingFlags.Static | BindingFlags.Public)
                          select mi).First();
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
            var method = (from mi in typeof(TypeWithFactoryMethod) 
                              .GetMethods(BindingFlags.Static | BindingFlags.Public)
                          select mi).First();
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
            var method1 = (from mi in typeof(TypeWithFactoryMethod)
                               .GetMethods(BindingFlags.Static | BindingFlags.Public)
                           select mi).ElementAt(0);
            var sut = new StaticMethod(method1);

            var method2 = (from mi in typeof(TypeWithFactoryMethod)
                               .GetMethods(BindingFlags.Static | BindingFlags.Public)
                           select mi).ElementAt(1);
            object other = new StaticMethod(method2);

            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherObjectWithSameMethod()
        {
            var method = (from mi in typeof(TypeWithFactoryMethod)
                              .GetMethods(BindingFlags.Static | BindingFlags.Public)
                          select mi).First();
            var sut = new StaticMethod(method);
            object other = new StaticMethod(method);
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
            var method = (from mi in typeof(TypeWithFactoryMethod)
                              .GetMethods(BindingFlags.Static | BindingFlags.Public)
                          select mi).First();
            var sut = new StaticMethod(method);
            // Exercise system
            var result = sut.GetHashCode();
            // Verify outcome
            var expectedHasCode = method.GetHashCode();
            Assert.Equal(expectedHasCode, result);
            // Teardown
        }
    }
}

using System;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class ConstructorMethodTest
    {
        [Fact]
        public void SutIsMethod()
        {
            // Fixture setup
            var dummyCtor = typeof(ConcreteType).GetConstructor(Type.EmptyTypes);
            // Exercise system
            var sut = new ConstructorMethod(dummyCtor);
            // Verify outcome
            Assert.IsAssignableFrom<IMethod>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullConstructorInfoThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new ConstructorMethod(null));
            // Teardown
        }

        [Fact]
        public void ConstructorIsCorrect()
        {
            // Fixture setup
            var expectedCtor = typeof(ConcreteType).GetConstructor(Type.EmptyTypes);
            var sut = new ConstructorMethod(expectedCtor);
            // Exercise system
            ConstructorInfo result = sut.Constructor;
            // Verify outcome
            Assert.Equal(expectedCtor, result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(ConcreteType), 0)]
        [InlineData(typeof(ConcreteType), 1)]
        [InlineData(typeof(ConcreteType), 2)]
        [InlineData(typeof(ConcreteType), 3)]
        [InlineData(typeof(SingleParameterType<int>), 0)]
        [InlineData(typeof(SingleParameterType<string>), 0)]
        public void ParametersReturnsCorrectResult(Type targetType, int index)
        {
            // Fixture setup
            var ctor = targetType.GetConstructors().ElementAt(index);
            var expectedParameters = ctor.GetParameters();

            var sut = new ConstructorMethod(ctor);
            // Exercise system
            var result = sut.Parameters;
            // Verify outcome
            Assert.True(expectedParameters.SequenceEqual(result));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(PropertyHolder<int>))]
        [InlineData(typeof(PropertyHolder<string>))]
        public void InvokeWithDefaultConstructorReturnsCorrectResult(Type targetType)
        {
            // Fixture setup
            var ctor = targetType.GetConstructor(Type.EmptyTypes);
            var sut = new ConstructorMethod(ctor);
            // Exercise system
            var result = sut.Invoke(Enumerable.Empty<object>());
            // Verify outcome
            Assert.IsAssignableFrom(targetType, result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(SingleParameterType<int>), 0)]
        [InlineData(typeof(SingleParameterType<string>), "")]
        public void InvokeWithSingleParameterReturnsCorrectResult(Type targetType, object parameter)
        {
            // Fixture setup
            var ctor = targetType.GetConstructor(targetType.GetTypeInfo().GetGenericArguments().ToArray());
            var sut = new ConstructorMethod(ctor);
            // Exercise system
            var result = sut.Invoke(new[] { parameter });
            // Verify outcome
            Assert.IsAssignableFrom(targetType, result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(DoubleParameterType<int, int>), 0, 1)]
        [InlineData(typeof(DoubleParameterType<string, string>), "", "foo")]
        [InlineData(typeof(DoubleParameterType<string, int>), "", 2)]
        public void InvokeWithTwoParametersReturnsCorrectResult(Type targetType, object parameter1, object parameter2)
        {
            // Fixture setup
            var ctor = targetType.GetConstructor(targetType.GetTypeInfo().GetGenericArguments().ToArray());
            var sut = new ConstructorMethod(ctor);
            // Exercise system
            var result = sut.Invoke(new[] { parameter1, parameter2 });
            // Verify outcome
            Assert.IsAssignableFrom(targetType, result);
            // Teardown
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Fixture setup
            var dummyCtor = typeof(ConcreteType).GetConstructor(Type.EmptyTypes);
            // Exercise system
            var sut = new ConstructorMethod(dummyCtor);
            // Verify outcome
            Assert.IsAssignableFrom<IEquatable<ConstructorMethod>>(sut);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Fixture setup
            var dummyCtor = typeof(ConcreteType).GetConstructor(Type.EmptyTypes);
            var sut = new ConstructorMethod(dummyCtor);
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
            var dummyCtor = typeof(ConcreteType).GetConstructor(Type.EmptyTypes);
            var sut = new ConstructorMethod(dummyCtor);
            // Exercise system
            var result = sut.Equals((ConstructorMethod)null);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualSomeOtherObject()
        {
            // Fixture setup
            var dummyCtor = typeof(ConcreteType).GetConstructor(Type.EmptyTypes);
            var sut = new ConstructorMethod(dummyCtor);
            // Exercise system
            var result = sut.Equals(new object());
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWithDifferentCtor()
        {
            // Fixture setup
            var ctor1 = typeof(ConcreteType).GetConstructor(Type.EmptyTypes);
            var sut = new ConstructorMethod(ctor1);

            var ctor2 = typeof(PropertyHolder<string>).GetConstructor(Type.EmptyTypes);
            object other = new ConstructorMethod(ctor2);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWithDifferentCtor()
        {
            // Fixture setup
            var ctor1 = typeof(ConcreteType).GetConstructor(Type.EmptyTypes);
            var sut = new ConstructorMethod(ctor1);

            var ctor2 = typeof(PropertyHolder<string>).GetConstructor(Type.EmptyTypes);
            var other = new ConstructorMethod(ctor2);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherObjectWithSameCtor()
        {
            // Fixture setup
            var ctor = typeof(ConcreteType).GetConstructor(Type.EmptyTypes);
            var sut = new ConstructorMethod(ctor);
            object other = new ConstructorMethod(ctor);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherSutWithSameCtor()
        {
            // Fixture setup
            var ctor = typeof(ConcreteType).GetConstructor(Type.EmptyTypes);
            var sut = new ConstructorMethod(ctor);
            var other = new ConstructorMethod(ctor);
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
            var ctor = typeof(ConcreteType).GetConstructor(Type.EmptyTypes);
            var sut = new ConstructorMethod(ctor);
            // Exercise system
            var result = sut.GetHashCode();
            // Verify outcome
            var expectedHasCode = ctor.GetHashCode();
            Assert.Equal(expectedHasCode, result);
            // Teardown
        }
    }
}

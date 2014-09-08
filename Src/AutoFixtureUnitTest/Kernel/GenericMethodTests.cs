using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class GenericMethodTests
    {
        [Fact]
        public void SutIsIMethod()
        {
            Action dummy = delegate { };
            var sut = new GenericMethod(dummy.Method);
            Assert.IsAssignableFrom<IMethod>(sut);
        }

        [Fact]
        public void InitializeFromFirstCtorNullMethodInfoThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new GenericMethod(null));
        }

        [Fact]
        public void InitializeFromSecondCtorWithNullMethodInfoThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new GenericMethod(null, m => null));
        }

        [Fact]
        public void InitializeFromThirdCtorWithNullMethodInfoThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new GenericMethod(null, new object()));
        }

        [Fact]
        public void InitializeWithNullFactoryThrows()
        {
            Action dummy = delegate { };

            Assert.Throws<ArgumentNullException>(() =>
                new GenericMethod(dummy.Method, null));
        }

        [Fact]
        public void InitializeWithNullOwnerThrows()
        {
            Action dummy = delegate { };

            Assert.Throws<ArgumentNullException>(() =>
                new GenericMethod(dummy.Method, (object)null));
        }

        [Fact]
        public void InitializeCreatesADefaultFactory()
        {
            Action dummy = delegate { };
            var sut = new GenericMethod(dummy.Method);

            var result = sut.Factory;

            Assert.NotNull(result);
        }

        [Fact]
        public void InitializeWithInstanceCreatesADefaultFactory()
        {
            Action dummy = delegate { };
            object instance = new object();
            var sut = new GenericMethod(dummy.Method, instance);

            var result = sut.Factory;

            Assert.NotNull(result);
        }

        [Fact]
        public void MethodIsCorrect()
        {
            var expectedMethod = ((Action)delegate { }).Method;
            var sut = new GenericMethod(expectedMethod);

            var result = sut.Method;

            Assert.Equal(expectedMethod, result);
        }

        [Fact]
        public void FactoryIsCorrect()
        {
            var anonymousMethod = ((Action)delegate { }).Method;
            Func<MethodInfo, IMethod> expectedFactory = m => null;
            var sut = new GenericMethod(anonymousMethod, expectedFactory);

            var result = sut.Factory;

            Assert.Equal(expectedFactory, result);
        }

        [Fact]
        public void ParametersIsCorrect()
        {
            Action<int, double> dummy = delegate { };
            var expectedParameters = dummy.Method.GetParameters();
            var sut = new GenericMethod(dummy.Method);

            var result = sut.Parameters;

            Assert.True(expectedParameters.SequenceEqual(result));
        }

        [Theory]
        [InlineData(typeof(TypeWithFactoryMethod), 0, new object[] { })]
        [InlineData(typeof(TypeWithFactoryMethod), 1, new object[] { "abc" })]
        [InlineData(typeof(TypeWithFactoryMethod), 2, new object[] { new[] { "ab", "c" } })]
        public void InvokeWithNonGenericMethodReturnsCorrectResult(Type targetType, int index, object values)
        {
            var method = (from mi in targetType
                              .GetMethods(BindingFlags.Static | BindingFlags.Public)
                          where mi.ReturnType == targetType
                          select mi).ElementAt(index);
            var sut = new GenericMethod(method);

            var result = sut.Invoke((object[])values);

            Assert.IsAssignableFrom(targetType, result);
        }

        [Theory]
        [InlineData(typeof(TypeWithInstanceFactoryMethod), 0, new object[] { })]
        [InlineData(typeof(TypeWithInstanceFactoryMethod), 1, new object[] { "abc" })]
        [InlineData(typeof(TypeWithInstanceFactoryMethod), 2, new object[] { new[] { "ab", "c" } })]
        public void InvokeWithInstanceMethodThrows(Type targetType, int index, object values)
        {
            var method = (from mi in targetType
                              .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                          where mi.ReturnType == targetType
                          select mi).ElementAt(index);
            var sut = new GenericMethod(method);

            Assert.Throws<TargetException>(() => sut.Invoke((object[]) values));
        }

        [Theory]
        [InlineData(typeof(TypeWithInstanceFactoryMethod), 0, new object[] { })]
        [InlineData(typeof(TypeWithInstanceFactoryMethod), 1, new object[] { "abc" })]
        [InlineData(typeof(TypeWithInstanceFactoryMethod), 2, new object[] { new[] { "ab", "c" } })]
        public void InvokeWithInstanceMethodReturnsCorrectResult(Type targetType, int index, object values)
        {
            var method = (from mi in targetType
                              .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                          where mi.ReturnType == targetType
                          select mi).ElementAt(index);
            var sut = new GenericMethod(method, new TypeWithInstanceFactoryMethod());

            var result = sut.Invoke((object[])values);

            Assert.IsAssignableFrom(targetType, result);
        }

        [Theory]
        [InlineData(typeof(TypeWithFactoryMethod), 0, new object[] { })]
        [InlineData(typeof(TypeWithFactoryMethod), 1, new object[] { "abc" })]
        [InlineData(typeof(TypeWithFactoryMethod), 2, new object[] { new[] { "ab", "c" } })]
        public void InvokeWithStaticMethodReturnsCorrectResult(Type targetType, int index, object values)
        {
            var method = (from mi in targetType
                              .GetMethods(BindingFlags.Static | BindingFlags.Public)
                          where mi.ReturnType == targetType
                          select mi).ElementAt(index);
            var sut = new GenericMethod(method, new TypeWithInstanceFactoryMethod());

            var result = sut.Invoke((object[])values);

            Assert.IsAssignableFrom(targetType, result);
        }

        [Theory]
        [ClassData(typeof(GenericMethodTestCase))]
        public void InvokeWithGenericMethodReturnsCorrectResult(Type targetType, int index, object values)
        {
            var method = (from mi in targetType
                              .GetMethods(BindingFlags.Static | BindingFlags.Public)
                          select mi).ElementAt(index);
            var sut = new GenericMethod(method);

            var result = sut.Invoke((object[])values);

            Assert.IsAssignableFrom(targetType, result);
        }

        [Theory]
        [InlineData(typeof(TypeWithGenericMethod), 0, new object[] { })]
        [InlineData(typeof(TypeWithGenericMethod), 1, new object[] { "a" })]
        [InlineData(typeof(TypeWithGenericMethod), 2, new object[] { "a" })]
        [InlineData(typeof(TypeWithGenericMethod), 3, new object[] { "a", "b" })]
        [InlineData(typeof(TypeWithGenericMethod), 4, new object[] { 1, "a" })]
        [InlineData(typeof(TypeWithGenericMethod), 5, new object[] { })]
        public void InvokeWithGenericMethodThatCannotBeInferedThrows(Type targetType, int index, object values)
        {
            var method = (from mi in targetType
                              .GetMethods(BindingFlags.Static | BindingFlags.Public)
                          select mi).ElementAt(index);
            var sut = new GenericMethod(method);

            var exception = Assert.Throws<TypeArgumentsCannotBeInferredException>(
                () => sut.Invoke((object[])values));
            Assert.Contains(method.Name, exception.Message);
        }

        [Fact]
        public void InvokeWithCustomFactoryReturnsCorrectResult()
        {
            var values = new[] { new object() };
            var expected = new object();
            var delegatingMethod = new DelegatingMethod();
            delegatingMethod.OnInvoke = obj => obj.SequenceEqual(values) ? expected : null;
            Func<MethodInfo, IMethod> factory = x => delegatingMethod;
            Action dummy = delegate { }; 
            var sut = new GenericMethod(dummy.Method, factory);

            var result = sut.Invoke(values);

            Assert.Equal(expected, result);
        }

    }

    public class GenericMethodTestCase : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { typeof(TypeWithGenericMethod), 0, new object[] { "abc" } };
            yield return new object[] { typeof(TypeWithGenericMethod), 1, new object[] { new string[] { "ab", "c" } } };
            yield return new object[] { typeof(TypeWithGenericMethod), 2, new object[] { "ab", 2 } };
            yield return new object[] { typeof(TypeWithGenericMethod), 3, new object[] { "ab", new Func<string, int>(x => x.Length) } };
            yield return new object[] { typeof(TypeWithGenericMethod), 4, new object[] { new int[] { 1, 2 }, new string[] { "ab", "c" } } };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class TypeWithGenericMethod
    {
        private TypeWithGenericMethod()
        {
        }

        public static TypeWithGenericMethod Create<T>(T argument)
        {
            return new TypeWithGenericMethod();
        }

        public static TypeWithGenericMethod Create<T>(IEnumerable<T> arguments)
        {
            return new TypeWithGenericMethod();
        }

        public static TypeWithGenericMethod Create<T, U>(T argument1, U argument2)
        {
            return new TypeWithGenericMethod();
        }

        public static TypeWithGenericMethod Create<T, U>(T argument1, Func<T, U> argument2)
        {
            return new TypeWithGenericMethod();
        }

        public static TypeWithGenericMethod Create<T>(T[] argument1, string[] argument2)
        {
            return new TypeWithGenericMethod();
        }

        public static T Create<T>()
        {
            return default(T);
        }
    }
    
    public class TypeWithInstanceFactoryMethod
    {
        public TypeWithInstanceFactoryMethod InstanceCreate()
        {
            return new TypeWithInstanceFactoryMethod();
        }

        public TypeWithInstanceFactoryMethod InstanceCreate(object argument)
        {
            return new TypeWithInstanceFactoryMethod();
        }

        public TypeWithInstanceFactoryMethod InstanceCreate(IEnumerable<object> arguments)
        {
            return new TypeWithInstanceFactoryMethod();
        }
    }
}

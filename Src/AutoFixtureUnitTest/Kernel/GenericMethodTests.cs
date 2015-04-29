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
            var anonymousFactory = new DelegatingMethodFactory();
            var sut = new GenericMethod(dummy.Method, anonymousFactory);
            Assert.IsAssignableFrom<IMethod>(sut);
        }

        [Fact]
        public void InitializeWithNullMethodInfoThrows()
        {
            var anonymousFactory = new DelegatingMethodFactory();
            Assert.Throws<ArgumentNullException>(() =>
                new GenericMethod(null, anonymousFactory));
        }

        [Fact]
        public void InitializeWithNullFactoryThrows()
        {
            Action dummy = delegate { };
            
            Assert.Throws<ArgumentNullException>(() =>
                new GenericMethod(dummy.Method, null));
        }

        [Fact]
        public void MethodIsCorrect()
        {
            var expectedMethod = ((Action)delegate { }).Method;
            var anonymousFactory = new DelegatingMethodFactory();
            var sut = new GenericMethod(expectedMethod, anonymousFactory);

            var result = sut.Method;

            Assert.Equal(expectedMethod, result);
        }

        [Fact]
        public void FactoryIsCorrect()
        {
            var anonymousMethod = ((Action)delegate { }).Method;
            var expectedFactory = new DelegatingMethodFactory();
            var sut = new GenericMethod(anonymousMethod, expectedFactory);

            var result = sut.Factory;

            Assert.Equal(expectedFactory, result);
        }

        [Fact]
        public void ParametersIsCorrect()
        {
            Action<int, double> dummy = delegate { };
            var anonymousFactory = new DelegatingMethodFactory();
            var expectedParameters = dummy.Method.GetParameters();
            var sut = new GenericMethod(dummy.Method, anonymousFactory);

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
            var factory = new DelegatingMethodFactory();
            factory.OnCreate = m => new StaticMethod(m);
            var sut = new GenericMethod(method, factory);

            var result = sut.Invoke((object[])values);

            Assert.IsAssignableFrom(targetType, result);
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
            var factory = new DelegatingMethodFactory();
            factory.OnCreate = m => new InstanceMethod(m, new TypeWithInstanceFactoryMethod());
            var sut = new GenericMethod(method, factory);

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
            var factory = new DelegatingMethodFactory();
            factory.OnCreate = m => new InstanceMethod(m, new TypeWithInstanceFactoryMethod());
            var sut = new GenericMethod(method, factory);

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
            var factory = new DelegatingMethodFactory();
            factory.OnCreate = m => new StaticMethod(m);
            var sut = new GenericMethod(method, factory);

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
        public void InvokeWithGenericMethodThatCannotBeInferredThrows(Type targetType, int index, object values)
        {
            var method = (from mi in targetType
                              .GetMethods(BindingFlags.Static | BindingFlags.Public)
                          select mi).ElementAt(index);
            var factory = new DelegatingMethodFactory();
            factory.OnCreate = m => new StaticMethod(m);
            var sut = new GenericMethod(method, factory);

            var exception = Assert.Throws<TypeArgumentsCannotBeInferredException>(
                () => sut.Invoke((object[])values));
            Assert.Contains(method.Name, exception.Message);
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

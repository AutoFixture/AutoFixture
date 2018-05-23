using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class RequestMemberTypeResolverTests
    {
        [Fact]
        public void CannotGetTypeFromUnsupportedRequests()
        {
            //Arrange
            var request = new object();
            var sut = new RequestMemberTypeResolver();

            //Act
            var canGetType = sut.TryGetMemberType(request, out var _);

            //Assert
            Assert.False(canGetType);
        }

        [Fact]
        public void CanGetTypeFromProperty()
        {
            //Arrange
            var request = typeof(TestClass)
                .GetTypeInfo()
                .GetProperty(nameof(TestClass.Property));

            var sut = new RequestMemberTypeResolver();

            //Act
            var canGetType = sut.TryGetMemberType(request, out var memberType);

            //Assert
            Assert.True(canGetType);
            Assert.Equal(typeof(int), memberType);
        }

        [Fact]
        public void CanGetTypeFromNullableProperty()
        {
            //Arrange
            var request = typeof(TestClass)
                .GetTypeInfo()
                .GetProperty(nameof(TestClass.NullableProperty));

            var sut = new RequestMemberTypeResolver();

            //Act
            var canGetType = sut.TryGetMemberType(request, out var memberType);

            //Assert
            Assert.True(canGetType);
            Assert.Equal(typeof(int), memberType);
        }

        [Fact]
        public void CanGetTypeFromField()
        {
            //Arrange
            var request = typeof(TestClass)
                .GetTypeInfo()
                .GetField(nameof(TestClass.Field));

            var sut = new RequestMemberTypeResolver();

            //Act
            var canGetType = sut.TryGetMemberType(request, out var memberType);

            //Assert
            Assert.True(canGetType);
            Assert.Equal(typeof(int), memberType);
        }

        [Fact]
        public void CanGetTypeFromNullableField()
        {
            //Arrange
            var request = typeof(TestClass)
                .GetTypeInfo()
                .GetField(nameof(TestClass.NullableField));

            var sut = new RequestMemberTypeResolver();

            //Act
            var canGetType = sut.TryGetMemberType(request, out var memberType);

            //Assert
            Assert.True(canGetType);
            Assert.Equal(typeof(int), memberType);
        }

        [Fact]
        public void CanGetTypeFromParameter()
        {
            //Arrange
            var request = typeof(TestClass)
                .GetTypeInfo()
                .GetMethod(nameof(TestClass.MethodWithParameter))
                .GetParameters()[0];

            var sut = new RequestMemberTypeResolver();

            //Act
            var canGetType = sut.TryGetMemberType(request, out var memberType);

            //Assert
            Assert.True(canGetType);
            Assert.Equal(typeof(int), memberType);
        }

        [Fact]
        public void CanGetTypeFromNullableParameter()
        {
            //Arrange
            var request = typeof(TestClass)
                .GetTypeInfo()
                .GetMethod(nameof(TestClass.MethodWithNullableParameter))
                .GetParameters()[0];

            var sut = new RequestMemberTypeResolver();

            //Act
            var canGetType = sut.TryGetMemberType(request, out var memberType);

            //Assert
            Assert.True(canGetType);
            Assert.Equal(typeof(int), memberType);
        }

        private class TestClass
        {
            public int Property { get; set; }
            public int? NullableProperty { get; set; }

#pragma warning disable 649
            public int Field;
            public int? NullableField;
#pragma warning restore 649

            public void MethodWithParameter(int parameter)
            {
            }

            public void MethodWithNullableParameter(int? nullableParameter)
            {
            }
        }
    }
}

using System;
using System.Linq;
using System.Reflection;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    public class MethodInvokeCommandTest
    {
        [Fact]
        public void SutIsGuardClauseCommand()
        {
            // Arrange
            var dummyMethod = new DelegatingMethod();
            var dummyExpansion = new DelegatingExpansion<object>();
            var dummyParameter = MethodInvokeCommandTest.CreateAnonymousParameterInfo();
            // Act
            var sut = new MethodInvokeCommand(dummyMethod, dummyExpansion, dummyParameter);
            // Assert
            Assert.IsAssignableFrom<IGuardClauseCommand>(sut);
        }

        [Fact]
        public void MethodIsCorrect()
        {
            // Arrange
            var method = new DelegatingMethod();
            var dummyExpansion = new DelegatingExpansion<object>();
            var dummyParameter = MethodInvokeCommandTest.CreateAnonymousParameterInfo();
            var sut = new MethodInvokeCommand(method, dummyExpansion, dummyParameter);
            // Act
            IMethod result = sut.Method;
            // Assert
            Assert.Equal(method, result);
        }

        [Fact]
        public void ExpansionIsCorrect()
        {
            // Arrange
            var dummyMethod = new DelegatingMethod();
            var expansion = new DelegatingExpansion<object>();
            var dummyParameter = MethodInvokeCommandTest.CreateAnonymousParameterInfo();
            var sut = new MethodInvokeCommand(dummyMethod, expansion, dummyParameter);
            // Act
            IExpansion<object> result = sut.Expansion;
            // Assert
            Assert.Equal(expansion, result);
        }

        [Fact]
        public void ParameterInfoIsCorrect()
        {
            // Arrange
            var dummyMethod = new DelegatingMethod();
            var dummyExpansion = new DelegatingExpansion<object>();
            var parameter = MethodInvokeCommandTest.CreateAnonymousParameterInfo();
            var sut = new MethodInvokeCommand(dummyMethod, dummyExpansion, parameter);
            // Act
            ParameterInfo result = sut.ParameterInfo;
            // Assert
            Assert.Equal(parameter, result);
        }

        [Fact]
        public void ExecuteCorrectlyInvokesMethod()
        {
            // Arrange
            var value = new object();
            var arguments = new[] { new object(), new object(), new object() };
            var expansion = new DelegatingExpansion<object> { OnExpand = v => v == value ? arguments : new object[0] };
            var dummyParameter = MethodInvokeCommandTest.CreateAnonymousParameterInfo();

            var mockVerified = false;
            var method = new DelegatingMethod { OnInvoke = a => mockVerified = a == arguments };

            var sut = new MethodInvokeCommand(method, expansion, dummyParameter);
            // Act
            sut.Execute(value);
            // Assert
            Assert.True(mockVerified);
        }

        [Fact]
        public void RequestedTypeIsCorrect()
        {
            // Arrange
            var dummyMethod = new DelegatingMethod();
            var dummyExpansion = new DelegatingExpansion<object>();
            var parameter = MethodInvokeCommandTest.CreateAnonymousParameterInfo();
            var sut = new MethodInvokeCommand(dummyMethod, dummyExpansion, parameter);
            // Act
            var result = sut.RequestedType;
            // Assert
            Assert.Equal(parameter.ParameterType, result);
        }

        [Fact]
        public void RequestedParameterNameIsCorrect()
        {
            var dummyMethod = new DelegatingMethod();
            var dummyExpansion = new DelegatingExpansion<object>();
            var parameter = MethodInvokeCommandTest.CreateAnonymousParameterInfo();
            var sut = new MethodInvokeCommand(dummyMethod, dummyExpansion, parameter);

            var actual = sut.RequestedParameterName;

            Assert.Equal(parameter.Name, actual);
        }

        [Fact]
        public void CreateExceptionReturnsExceptionWithCorrectMessage()
        {
            // Arrange
            var dummyMethod = new DelegatingMethod();
            var dummyExpansion = new DelegatingExpansion<object>();
            var parameter = MethodInvokeCommandTest.CreateAnonymousParameterInfo();
            var sut = new MethodInvokeCommand(dummyMethod, dummyExpansion, parameter);
            // Act
            var message = Guid.NewGuid().ToString();
            var result = sut.CreateException(message);
            // Assert
            var e = Assert.IsAssignableFrom<GuardClauseException>(result);
            Assert.Contains(message, e.Message);
        }

        [Fact]
        public void CreateExceptionWithInnerReturnsExceptionWithCorrectMessage()
        {
            // Arrange
            var dummyMethod = new DelegatingMethod();
            var dummyExpansion = new DelegatingExpansion<object>();
            var parameter = MethodInvokeCommandTest.CreateAnonymousParameterInfo();
            var sut = new MethodInvokeCommand(dummyMethod, dummyExpansion, parameter);
            // Act
            var message = Guid.NewGuid().ToString();
            var inner = new Exception();
            var result = sut.CreateException(message, inner);
            // Assert
            var e = Assert.IsAssignableFrom<GuardClauseException>(result);
            Assert.Contains(message, e.Message);
        }

        [Fact]
        public void CreateExceptionWithInnerReturnsExceptionWithCorrectInnerException()
        {
            // Arrange
            var dummyMethod = new DelegatingMethod();
            var dummyExpansion = new DelegatingExpansion<object>();
            var parameter = MethodInvokeCommandTest.CreateAnonymousParameterInfo();
            var sut = new MethodInvokeCommand(dummyMethod, dummyExpansion, parameter);
            // Act
            var message = Guid.NewGuid().ToString();
            var inner = new Exception();
            var result = sut.CreateException(message, inner);
            // Assert
            var e = Assert.IsAssignableFrom<GuardClauseException>(result);
            Assert.Equal(inner, e.InnerException);
        }

        private static ParameterInfo CreateAnonymousParameterInfo()
        {
            var parameter = (from m in typeof(object).GetMethods()
                             let p = m.GetParameters()
                             orderby p.Length descending
                             select p.First()).First();
            return parameter;
        }
    }
}

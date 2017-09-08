using System;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class MethodInvokeCommandTest
    {
        [Fact]
        public void SutIsGuardClauseCommand()
        {
            // Fixture setup
            var dummyMethod = new DelegatingMethod();
            var dummyExpansion = new DelegatingExpansion<object>();
            var dummyParameter = MethodInvokeCommandTest.CreateAnonymousParameterInfo();
            // Exercise system
            var sut = new MethodInvokeCommand(dummyMethod, dummyExpansion, dummyParameter);
            // Verify outcome
            Assert.IsAssignableFrom<IGuardClauseCommand>(sut);
            // Teardown
        }

        [Fact]
        public void MethodIsCorrect()
        {
            // Fixture setup
            var method = new DelegatingMethod();
            var dummyExpansion = new DelegatingExpansion<object>();
            var dummyParameter = MethodInvokeCommandTest.CreateAnonymousParameterInfo();
            var sut = new MethodInvokeCommand(method, dummyExpansion, dummyParameter);
            // Exercise system
            IMethod result = sut.Method;
            // Verify outcome
            Assert.Equal(method, result);
            // Teardown
        }

        [Fact]
        public void ExpansionIsCorrect()
        {
            // Fixture setup
            var dummyMethod = new DelegatingMethod();
            var expansion = new DelegatingExpansion<object>();
            var dummyParameter = MethodInvokeCommandTest.CreateAnonymousParameterInfo();
            var sut = new MethodInvokeCommand(dummyMethod, expansion, dummyParameter);
            // Exercise system
            IExpansion<object> result = sut.Expansion;
            // Verify outcome
            Assert.Equal(expansion, result);
            // Teardown
        }

        [Fact]
        public void ParameterInfoIsCorrect()
        {
            // Fixture setup
            var dummyMethod = new DelegatingMethod();
            var dummyExpansion = new DelegatingExpansion<object>();
            var parameter = MethodInvokeCommandTest.CreateAnonymousParameterInfo();
            var sut = new MethodInvokeCommand(dummyMethod, dummyExpansion, parameter);
            // Exercise system
            ParameterInfo result = sut.ParameterInfo;
            // Verify outcome
            Assert.Equal(parameter, result);
            // Teardown
        }

        [Fact]
        public void ExecuteCorrectlyInvokesMethod()
        {
            // Fixture setup
            var value = new object();
            var arguments = new[] { new object(), new object(), new object() };
            var expansion = new DelegatingExpansion<object> { OnExpand = v => v == value ? arguments : new object[0] };
            var dummyParameter = MethodInvokeCommandTest.CreateAnonymousParameterInfo();

            var mockVerified = false;
            var method = new DelegatingMethod { OnInvoke = a => mockVerified = a == arguments };

            var sut = new MethodInvokeCommand(method, expansion, dummyParameter);
            // Exercise system
            sut.Execute(value);
            // Verify outcome
            Assert.True(mockVerified);
            // Teardown
        }

        [Fact]
        public void RequestedTypeIsCorrect()
        {
            // Fixture setup
            var dummyMethod = new DelegatingMethod();
            var dummyExpansion = new DelegatingExpansion<object>();
            var parameter = MethodInvokeCommandTest.CreateAnonymousParameterInfo();
            var sut = new MethodInvokeCommand(dummyMethod, dummyExpansion, parameter);
            // Exercise system
            var result = sut.RequestedType;
            // Verify outcome
            Assert.Equal(parameter.ParameterType, result);
            // Teardown
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
            // Fixture setup
            var dummyMethod = new DelegatingMethod();
            var dummyExpansion = new DelegatingExpansion<object>();
            var parameter = MethodInvokeCommandTest.CreateAnonymousParameterInfo();
            var sut = new MethodInvokeCommand(dummyMethod, dummyExpansion, parameter);
            // Exercise system
            var message = Guid.NewGuid().ToString();
            var result = sut.CreateException(message);
            // Verify outcome
            var e = Assert.IsAssignableFrom<GuardClauseException>(result);
            Assert.Contains(message, e.Message);
            // Teardown
        }

        [Fact]
        public void CreateExceptionWithInnerReturnsExceptionWithCorrectMessage()
        {
            // Fixture setup
            var dummyMethod = new DelegatingMethod();
            var dummyExpansion = new DelegatingExpansion<object>();
            var parameter = MethodInvokeCommandTest.CreateAnonymousParameterInfo();
            var sut = new MethodInvokeCommand(dummyMethod, dummyExpansion, parameter);
            // Exercise system
            var message = Guid.NewGuid().ToString();
            var inner = new Exception();
            var result = sut.CreateException(message, inner);
            // Verify outcome
            var e = Assert.IsAssignableFrom<GuardClauseException>(result);
            Assert.Contains(message, e.Message);
            // Teardown
        }

        [Fact]
        public void CreateExceptionWithInnerReturnsExceptionWithCorrectInnerException()
        {
            // Fixture setup
            var dummyMethod = new DelegatingMethod();
            var dummyExpansion = new DelegatingExpansion<object>();
            var parameter = MethodInvokeCommandTest.CreateAnonymousParameterInfo();
            var sut = new MethodInvokeCommand(dummyMethod, dummyExpansion, parameter);
            // Exercise system
            var message = Guid.NewGuid().ToString();
            var inner = new Exception();
            var result = sut.CreateException(message, inner);
            // Verify outcome
            var e = Assert.IsAssignableFrom<GuardClauseException>(result);
            Assert.Equal(inner, e.InnerException);
            // Teardown
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

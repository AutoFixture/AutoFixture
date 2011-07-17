using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;
using Xunit.Extensions;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class MethodInvokeCommandTest
    {
        [Fact]
        public void SutIsGuardClauseCommand()
        {
            // Fixture setup
            var dummyMethod = new DelegatingMethod();
            var dummyExpansion = new DelegatingExpansion();
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
            var dummyExpansion = new DelegatingExpansion();
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
            var expansion = new DelegatingExpansion();
            var dummyParameter = MethodInvokeCommandTest.CreateAnonymousParameterInfo();
            var sut = new MethodInvokeCommand(dummyMethod, expansion, dummyParameter);
            // Exercise system
            IExpansion result = sut.Expansion;
            // Verify outcome
            Assert.Equal(expansion, result);
            // Teardown
        }

        [Fact]
        public void ParameterInfoIsCorrect()
        {
            // Fixture setup
            var dummyMethod = new DelegatingMethod();
            var dummyExpansion = new DelegatingExpansion();
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
            var expansion = new DelegatingExpansion { OnExpand = v => v == value ? arguments : new object[0] };
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
            var dummyExpansion = new DelegatingExpansion();
            var parameter = MethodInvokeCommandTest.CreateAnonymousParameterInfo();
            var sut = new MethodInvokeCommand(dummyMethod, dummyExpansion, parameter);
            // Exercise system
            var result = sut.RequestedType;
            // Verify outcome
            Assert.Equal(parameter.ParameterType, result);
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

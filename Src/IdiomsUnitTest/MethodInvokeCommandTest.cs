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
            var dummyParameter = typeof(Version).GetConstructors().OrderBy(ci => ci.GetParameters().Length).Last().GetParameters().First();
            var dummyArguments = new Dictionary<ParameterInfo, object>();
            // Exercise system
            var sut = new MethodInvokeCommand(dummyMethod, dummyParameter, dummyArguments);
            // Verify outcome
            Assert.IsAssignableFrom<IGuardClauseCommand>(sut);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(Version))]
        public void MethodIsCorrect(Type type)
        {
            // Fixture setup
            var method = new DelegatingMethod();
            var dummyParameter = typeof(Version).GetConstructors().OrderBy(ci => ci.GetParameters().Length).Last().GetParameters().First();
            var dummyArguments = new Dictionary<ParameterInfo, object>();
            var sut = new MethodInvokeCommand(method, dummyParameter, dummyArguments);
            // Exercise system
            IMethod result = sut.Method;
            // Verify outcome
            Assert.Equal(method, result);
            // Teardown
        }

        [Fact]
        public void TargetParameterIsCorrect()
        {
            // Fixture setup
            var dummyMethod = new DelegatingMethod();
            var targetParameter = typeof(Version).GetConstructors().OrderBy(ci => ci.GetParameters().Length).Last().GetParameters().First();
            var dummyArguments = new Dictionary<ParameterInfo, object>();
            var sut = new MethodInvokeCommand(dummyMethod, targetParameter, dummyArguments);
            // Exercise system
            ParameterInfo result = sut.TargetParameter;
            // Verify outcome
            Assert.Equal(targetParameter, result);
            // Teardown
        }

        [Fact]
        public void DefaultArgumentsIsCorrect()
        {
            // Fixture setup
            var dummyMethod = new DelegatingMethod();
            var dummyParameter = typeof(Version).GetConstructors().OrderBy(ci => ci.GetParameters().Length).Last().GetParameters().First();
            var arguments = new Dictionary<ParameterInfo, object>();
            var sut = new MethodInvokeCommand(dummyMethod, dummyParameter, arguments);
            // Exercise system
            IDictionary<ParameterInfo, object> result = sut.DefaultArguments;
            // Verify outcome
            Assert.Equal(arguments, result);
            // Teardown
        }
    }
}

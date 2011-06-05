using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;
using Xunit.Extensions;
using System.Reflection;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class ConstructorInvokeCommandTest
    {
        [Fact]
        public void SutIsGuardClauseCommand()
        {
            // Fixture setup
            var dummyCtor = typeof(object).GetConstructors().First();
            var dummyParameter = typeof(Version).GetConstructors().OrderBy(ci => ci.GetParameters().Length).Last().GetParameters().First();
            var dummyArguments = new Dictionary<ParameterInfo, object>();
            // Exercise system
            var sut = new ConstructorInvokeCommand(dummyCtor, dummyParameter, dummyArguments);
            // Verify outcome
            Assert.IsAssignableFrom<IGuardClauseCommand>(sut);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(Version))]
        public void ConstructorInfoIsCorrect(Type type)
        {
            // Fixture setup
            var constructorInfo = type.GetConstructors().First();
            var dummyParameter = typeof(Version).GetConstructors().OrderBy(ci => ci.GetParameters().Length).Last().GetParameters().First();
            var dummyArguments = new Dictionary<ParameterInfo, object>();
            var sut = new ConstructorInvokeCommand(constructorInfo, dummyParameter, dummyArguments);
            // Exercise system
            ConstructorInfo result = sut.ConstructorInfo;
            // Verify outcome
            Assert.Equal(constructorInfo, result);
            // Teardown
        }

        [Fact]
        public void TargetParameterIsCorrect()
        {
            // Fixture setup
            var dummyCtor = typeof(Version).GetConstructors().OrderBy(ci => ci.GetParameters().Length).Last();
            var targetParameter = typeof(Version).GetConstructors().OrderBy(ci => ci.GetParameters().Length).Last().GetParameters().First();
            var dummyArguments = new Dictionary<ParameterInfo, object>();
            var sut = new ConstructorInvokeCommand(dummyCtor, targetParameter, dummyArguments);
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
            var dummyCtor = typeof(object).GetConstructors().First();
            var dummyParameter = typeof(Version).GetConstructors().OrderBy(ci => ci.GetParameters().Length).Last().GetParameters().First();
            var arguments = new Dictionary<ParameterInfo, object>();
            var sut = new ConstructorInvokeCommand(dummyCtor, dummyParameter, arguments);
            // Exercise system
            IDictionary<ParameterInfo, object> result = sut.DefaultArguments;
            // Verify outcome
            Assert.Equal(arguments, result);
            // Teardown
        }
    }
}

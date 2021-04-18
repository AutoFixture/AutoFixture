using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace AutoFixture.NUnit3
{
    internal class AutoTestCaseParameters
    {
        private readonly IReadOnlyList<object> arguments;
        private readonly Func<IFixture> fixtureFactory;
        private readonly IReadOnlyList<IParameterInfo> methodParameters;
        private readonly FixedNameArgumentsPatcher argumentsPatcher;

        public AutoTestCaseParameters(
            Func<IFixture> fixtureFactory,
            IReadOnlyList<IParameterInfo> methodParameters,
            IReadOnlyList<object> arguments)
        {
            this.fixtureFactory = fixtureFactory;
            this.arguments = arguments;
            this.methodParameters = methodParameters;

            var autoDataStartIndex = methodParameters.Count - arguments.Count;
            this.argumentsPatcher = new FixedNameArgumentsPatcher(methodParameters, autoDataStartIndex);
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "This method is always expected to return an instance of the TestCaseParameters class.")]
        private TestCaseParameters Convert()
        {
            try
            {
                var fixture = this.fixtureFactory();
                var missingParameters = this.methodParameters.Skip(this.arguments.Count).ToList();
                missingParameters.Customize(fixture);
                var missingValues = missingParameters.Select(fixture.Resolve);
                var parameters = new TestCaseParameters(this.arguments.Concat(missingValues).ToArray());
                this.argumentsPatcher.Patch(parameters);
                return parameters;
            }
            catch (Exception e)
            {
                return new TestCaseParameters(e);
            }
        }

        public static implicit operator TestCaseParameters(AutoTestCaseParameters parameters)
        {
            return parameters.Convert();
        }
    }
}
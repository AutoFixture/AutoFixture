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
        private readonly Func<IFixture> fixtureFactory;
        private readonly IReadOnlyList<object> arguments;
        private IPatchParameters patcher = new FixedNameArgumentsPatcher();

        public AutoTestCaseParameters(Func<IFixture> fixtureFactory, IReadOnlyList<object> arguments)
        {
            this.fixtureFactory = fixtureFactory ?? throw new ArgumentNullException(nameof(fixtureFactory));
            this.arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
        }

        public string Category { get; set; }

        public IPatchParameters Patcher
        {
            get => this.patcher;
            set => this.patcher = value;
        }

        public TestCaseParameters GetParameters(IMethodInfo method)
        {
            var parameters = this.CreateParameters(method);

            if (!string.IsNullOrWhiteSpace(this.Category))
            {
                parameters.Properties.Add("Category", this.Category);
            }

            return parameters;
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "This method is always expected to return an instance of the TestCaseParameters class.")]
        private TestCaseParameters CreateParameters(IMethodInfo method)
        {
            var methodParameters = method.GetParameters();

            var missingParameters = methodParameters
                .Skip(this.arguments.Count).ToList();

            if (missingParameters.Count == 0)
                return new TestCaseParameters(this.arguments.ToArray());

            try
            {
                var fixture = this.fixtureFactory.Invoke();

                missingParameters
                    .SelectMany(x => x.GetCustomizations())
                    .Aggregate().Customize(fixture);

                var missingValues = missingParameters.Select(fixture.Resolve);
                var arguments = this.arguments.Concat(missingValues).ToArray();

                var parameters = new TestCaseParameters(arguments);
                this.patcher.Patch(parameters, method);
                return parameters;
            }
            catch (Exception e)
            {
                return new TestCaseParameters(e);
            }
        }
    }
}

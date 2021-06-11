using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Xunit2
{
    internal class AutoTestCaseData : ITestCaseSource
    {
        public AutoTestCaseData(Func<IFixture> createFixture, ITestCaseSource source)
        {
            this.CreateFixture = createFixture;
            this.Source = source;
        }

        public Func<IFixture> CreateFixture { get; private set; }

        public ITestCaseSource Source { get; private set; }

        public IEnumerable<IEnumerable<object>> GetTestCases(MethodInfo method)
        {
            foreach (var testCase in this.Source.GetTestCases(method))
            {
                var values = testCase.ToList();
                var fixture = this.CreateFixture();
                var missingParameters = method.GetParameters()
                    .Skip(values.Count).ToList();

                missingParameters
                    .SelectMany(x => x.GetCustomizations())
                    .Aggregate(new CustomizationBuilder(), (b, x) => b.Append(x))
                    .ToCustomization().Customize(fixture);

                var missingValues = missingParameters.Select(fixture.Resolve);

                yield return values.Concat(missingValues);
            }
        }
    }
}
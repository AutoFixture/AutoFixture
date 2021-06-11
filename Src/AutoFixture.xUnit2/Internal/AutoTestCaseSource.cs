#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Xunit2.Internal
{
    internal class AutoTestCaseSource : ITestCaseSource
    {
        public AutoTestCaseSource(Func<IFixture> createFixture, ITestCaseSource? source = default)
        {
            this.CreateFixture = createFixture ?? throw new ArgumentNullException(nameof(createFixture));
            this.Source = source;
        }

        public Func<IFixture> CreateFixture { get; }

        public ITestCaseSource? Source { get; }

        public IEnumerable<IEnumerable<object>> GetTestCases(MethodInfo method)
        {
            return this.Source is null
                ? this.GenerateValues(method)
                : this.CombineValues(method, this.Source);
        }

        private IEnumerable<IEnumerable<object>> GenerateValues(MethodBase methodInfo)
        {
            var parameters = methodInfo.GetParameters()
                .Select(TestParameter.From).ToList();

            var fixture = this.CreateFixture();

            yield return parameters
                .Select(parameter => GenerateAutoValue(parameter, fixture))
                .ToList();
        }

        private IEnumerable<IEnumerable<object>> CombineValues(MethodInfo methodInfo, ITestCaseSource source)
        {
            var parameters = methodInfo.GetParameters()
                .Select(TestParameter.From).ToList();

            foreach (var testCase in source.GetTestCases(methodInfo))
            {
                var values = testCase.ToArray();
                var fixture = this.CreateFixture();
                parameters.Take(values.Length)
                    .Zip(values, (parameter, value) => new Argument(parameter, value))
                    .Select(argument => argument.GetCustomization()).ToList()
                    .ForEach(x => fixture.Customize(x));

                var missingValues = parameters
                    .Skip(values.Length)
                    .Select(parameter => GenerateAutoValue(parameter, fixture))
                    .ToList();

                yield return values.Concat(missingValues);
            }
        }

        private static object GenerateAutoValue(TestParameter parameter, IFixture fixture)
        {
            fixture.Customize(parameter.GetCustomization());
            return fixture.Resolve(parameter.ParameterInfo);
        }
    }
}
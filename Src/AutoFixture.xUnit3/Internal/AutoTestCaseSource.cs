#nullable enable
namespace AutoFixture.Xunit3.Internal
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
            var parameters = Array.ConvertAll(methodInfo.GetParameters(), TestParameter.From);
            var fixture = this.CreateFixture();
            yield return Array.ConvertAll(parameters, parameter => GenerateAutoValue(parameter, fixture));
        }

        private IEnumerable<IEnumerable<object>> CombineValues(MethodInfo methodInfo, ITestCaseSource source)
        {
            var parameters = Array.ConvertAll(methodInfo.GetParameters(), TestParameter.From);
            var testCases = source.GetTestCases(methodInfo);

            foreach (var testCase in testCases)
            {
                var values = testCase.ToArray();
                var fixture = this.CreateFixture();

                var customizations = parameters.Take(values.Length)
                                               .Zip(values, (parameter, value) => new Argument(parameter, value))
                                               .Select(argument => argument.GetCustomization());

                foreach (var customization in customizations)
                {
                    fixture.Customize(customization);
                }

                var missingValues = parameters
                                    .Skip(values.Length)
                                    .Select(parameter => GenerateAutoValue(parameter, fixture))
                                    .ToArray();

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
#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.TUnit.Extensions;

namespace AutoFixture.TUnit.Internal
{
    /// <summary>
    /// Combines the values from a source with auto-generated values.
    /// </summary>
    public class AutoDataSource : DataSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoDataSource"/> class.
        /// </summary>
        /// <param name="createFixture">The factory method for creating a fixture.</param>
        /// <param name="source">The source of test data to combine with auto-generated values.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="createFixture"/> is <see langword="null"/>.
        /// </exception>
        public AutoDataSource(Func<IFixture> createFixture, IDataSource? source = default)
        {
            this.CreateFixture = createFixture ?? throw new ArgumentNullException(nameof(createFixture));
            this.Source = source;
        }

        /// <summary>
        /// Gets the factory method for creating a fixture.
        /// </summary>
        public Func<IFixture> CreateFixture { get; }

        /// <summary>
        /// Gets the source of test data to combine with auto-generated values.
        /// </summary>
        public IDataSource? Source { get; }

        /// <summary>
        /// Returns the combined test data provided by the source and auto-generated values.
        /// </summary>
        /// <param name="method">The target method for which to provide the arguments.</param>
        /// <returns>Returns a sequence of argument collections.</returns>
        public override IEnumerable<Func<object[]>> GenerateDataSources(DataGeneratorMetadata metadata)
        {
            return this.Source is null
                ? this.GenerateValues(metadata)
                : this.CombineValues(metadata, this.Source);
        }

        private IEnumerable<Func<object[]>> GenerateValues(DataGeneratorMetadata metadata)
        {
            var parameters = Array.ConvertAll(metadata.GetMethod().GetParameters(), TestParameter.From);
            var fixture = this.CreateFixture();
            yield return () => Array.ConvertAll(parameters, parameter => GenerateAutoValue(parameter, fixture));
        }

        private IEnumerable<Func<object[]>> CombineValues(DataGeneratorMetadata metadata, IDataSource source)
        {
            var method = metadata.GetMethod();

            var parameters = Array.ConvertAll(method.GetParameters(), TestParameter.From);

            foreach (var testDataFunc in source.GenerateDataSources(metadata))
            {
                var testData = testDataFunc();

                var customizations = parameters.Take(testData.Length)
                    .Zip(testData, (parameter, value) => new Argument(parameter, value))
                    .Select(argument => argument.GetCustomization())
                    .Where(x => x is not NullCustomization);

                var fixture = this.CreateFixture();

                foreach (var customization in customizations)
                {
                    fixture.Customize(customization);
                }

                var missingValues = parameters.Skip(testData.Length)
                    .Select(parameter => GenerateAutoValue(parameter, fixture))
                    .ToArray();

                yield return () => testData.Concat(missingValues).ToArray();
            }
        }

        private static object GenerateAutoValue(TestParameter parameter, IFixture fixture)
        {
            var customization = parameter.GetCustomization();
            
            if (customization is not NullCustomization)
            {
                fixture.Customize(customization);
            }

            return fixture.Resolve(parameter.ParameterInfo);
        }
    }
}
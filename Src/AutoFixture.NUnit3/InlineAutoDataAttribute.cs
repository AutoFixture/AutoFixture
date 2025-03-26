using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading;
using AutoFixture.Kernel;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace AutoFixture.NUnit3
{
    /// <summary>
    /// This attribute acts as a TestCaseAttribute but allow incomplete parameter values,
    /// which will be provided by AutoFixture.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    [CLSCompliant(false)]
    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Justification = "This attribute is the root of a potential attribute hierarchy.")]
    public class InlineAutoDataAttribute : Attribute, ITestBuilder
    {
        private readonly object[] existingParameterValues;
        private readonly Func<IFixture> fixtureFactory;

        private ITestMethodBuilder testMethodBuilder = new FixedNameTestMethodBuilder();

        /// <summary>
        /// Gets or sets the current <see cref="ITestMethodBuilder"/> strategy.
        /// </summary>
        public ITestMethodBuilder TestMethodBuilder
        {
            get => this.testMethodBuilder;
            set => this.testMethodBuilder = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Construct a <see cref="InlineAutoDataAttribute"/> with parameter values for test method.
        /// </summary>
        public InlineAutoDataAttribute(params object[] arguments)
            : this(() => new Fixture(), arguments)
        {
        }

        /// <summary>
        /// Construct a <see cref="InlineAutoDataAttribute"/> with an <see cref="IFixture"/>
        /// and parameter values for test method.
        /// </summary>
        [Obsolete("This constructor overload is deprecated because it offers poor performance, and will be removed in a future version. " +
                  "Please use the overload with a factory method, so fixture will be constructed only if needed.")]
        protected InlineAutoDataAttribute(IFixture fixture, params object[] arguments)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            this.fixtureFactory = () => fixture;
            this.existingParameterValues = arguments ?? new object[] { null };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoDataAttribute"/> class
        /// with the supplied <paramref name="fixtureFactory"/>. Fixture will be created
        /// on demand using the provided factory.
        /// </summary>
        protected InlineAutoDataAttribute(Func<IFixture> fixtureFactory, params object[] arguments)
        {
            this.fixtureFactory = fixtureFactory ?? throw new ArgumentNullException(nameof(fixtureFactory));
            this.existingParameterValues = arguments ?? new object[] { null };
        }

        /// <summary>
        /// Gets the parameter values for the test method.
        /// </summary>
        public IReadOnlyList<object> Arguments => this.existingParameterValues;

        /// <summary>
        ///     Construct one or more TestMethods from a given MethodInfo,
        ///     using available parameter data.
        /// </summary>
        /// <param name="method">The MethodInfo for which tests are to be constructed.</param>
        /// <param name="suite">The suite to which the tests will be added.</param>
        /// <returns>One or more TestMethods.</returns>
        public IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));

            foreach (var parameters in this.GetTestParameters(method))
            {
                var test = this.TestMethodBuilder.Build(method, suite, parameters, this.existingParameterValues.Length);

                yield return test;
            }
        }

        private IEnumerable<IReadOnlyList<TestParameterInfo>> GetTestParameters(IMethodInfo method)
        {
            var combinations = GetCombinations(method.GetParameters());

            foreach (var set in combinations)
            {
                var result = new List<TestParameterInfo>(set.Count);

                var fixture = this.fixtureFactory.Invoke();

                var currentParameterIndex = 0;

                foreach (var item in set)
                {
                    ApplyCustomizations(fixture, item);

                    if (item.HasValue)
                    {
                        result.Add(new TestParameterInfo
                        {
                            Parameter = item.Parameter,
                            HasValue = item.HasValue,
                            Value = true,
                            CustomizationSources = item.CustomizationSources
                        });
                    }
                    else
                    {
                        if (currentParameterIndex < this.existingParameterValues.Length)
                        {
                            result.Add(new TestParameterInfo
                            {
                                Parameter = item.Parameter,
                                HasValue = true,
                                Value = this.existingParameterValues[currentParameterIndex],
                                CustomizationSources = item.CustomizationSources
                            });
                        }
                        else
                        {
                            result.Add(new TestParameterInfo
                            {
                                Parameter = item.Parameter,
                                HasValue = false,
                                Value = Resolve(fixture, item),
                                CustomizationSources = item.CustomizationSources
                            });
                        }

                        currentParameterIndex++;
                    }
                }

                yield return result;
            }
        }

        private static void ApplyCustomizations(IFixture fixture, TestParameterInfo parameterInfo)
        {
            foreach (var ca in parameterInfo.CustomizationSources)
                fixture.Customize(ca.GetCustomization(parameterInfo.Parameter.ParameterInfo));

            if (parameterInfo.HasValue && parameterInfo.Parameter.GetCustomAttributes<FrozenAttribute>(false).Any())
            {
                var frozenAttribute = parameterInfo.CustomizationSources.OfType<FrozenAttribute>().First();
                var customization = frozenAttribute.GetCustomization(parameterInfo.Parameter.ParameterInfo) as FreezeOnMatchCustomization;

                fixture.Customizations.Insert(0, new FilteringSpecimenBuilder(new FixedBuilder(parameterInfo.Value), customization.Matcher));
            }
        }

        private static object Resolve(IFixture fixture, TestParameterInfo parameterInfo)
        {
            return new SpecimenContext(fixture).Resolve(parameterInfo.Parameter.ParameterInfo);
        }


        private static IReadOnlyList<IReadOnlyList<TestParameterInfo>> GetCombinations(IParameterInfo[] parameters)
        {
            var parametersWithValue = parameters.Select(RenderAllConstantValues).Select(UnwrapValues).ToArray();

            return Combine(parametersWithValue);

            static ParameterWithAllValues RenderAllConstantValues(IParameterInfo parameter)
            {
                var customizationSources = parameter.GetCustomAttributes<IParameterCustomizationSource>(false).OrderBy(x => x, new CustomizeAttributeComparer()).ToArray();

                if (parameter.GetCustomAttributes<IParameterDataSource>(false).Any())
                {
                    var data = new List<object>();

                    foreach (var source in parameter.GetCustomAttributes<IParameterDataSource>(false))
                    {
                        foreach (var item in source.GetData(parameter))
                            data.Add(item);
                    }

                    return new ParameterWithAllValues
                    {
                        ParameterInfo = parameter,
                        CustomizationSources = customizationSources,
                        Values = data,
                        HasValue = true
                    };
                }
                else
                {
                    return new ParameterWithAllValues
                    {
                        ParameterInfo = parameter,
                        CustomizationSources = customizationSources,
                        Values = new object[0],
                        HasValue = false
                    };
                }
            }

            static IEnumerable<TestParameterInfo> UnwrapValues(ParameterWithAllValues parameter)
            {
                if (parameter.HasValue)
                {
                    foreach (var value in parameter.Values)
                    {
                        yield return new TestParameterInfo
                        {
                            Parameter = parameter.ParameterInfo,
                            HasValue = parameter.HasValue,
                            CustomizationSources = parameter.CustomizationSources,
                            Value = value
                        };
                    }
                }
                else
                {
                    yield return new TestParameterInfo
                    {
                        Parameter = parameter.ParameterInfo,
                        HasValue = parameter.HasValue,
                        CustomizationSources = parameter.CustomizationSources,
                        Value = null
                    };
                }
            }

            static IReadOnlyList<IReadOnlyList<TestParameterInfo>> Combine(IReadOnlyList<IEnumerable<TestParameterInfo>> sources)
            {
                var result = new List<IReadOnlyList<TestParameterInfo>>();
                var enumerators = new IEnumerator<TestParameterInfo>[sources.Count];
                var index = -1;

                for (; ; )
                {
                    while (++index < sources.Count)
                    {
                        enumerators[index] = sources[index].GetEnumerator();

                        if (!enumerators[index].MoveNext())
                        {
                            return result;
                        }
                    }

                    var testData = new TestParameterInfo[sources.Count];

                    for (int i = 0; i < sources.Count; i++)
                    {
                        testData[i] = enumerators[i].Current;
                    }

                    result.Add(testData);

                    index = sources.Count;

                    while (--index >= 0 && !enumerators[index].MoveNext()) ;

                    if (index < 0) break;
                }

                return result;
            }
        }

        private struct ParameterWithAllValues
        {
            public IParameterInfo ParameterInfo { get; set; }

            public IEnumerable<object?> Values { get; set; }

            public IEnumerable<IParameterCustomizationSource> CustomizationSources { get; set; }

            public bool HasValue { get; set; }
        }

        ///// <summary>
        ///// Get values for a collection of <see cref="IParameterInfo"/>.
        ///// </summary>
        //private IEnumerable<TestParameterInfo> GetParameterValues(IMethodInfo method)
        //{
        //    var parameters = method.GetParameters();
        //    //return this.existingParameterValues.Concat(this.GetMissingValues(parameters));

        //    return new TestParameterInfo[0];
        //}

        //private IEnumerable<object> GetMissingValues(IEnumerable<IParameterInfo> parameters)
        //{
        //    var parametersWithoutValues = parameters.Skip(this.existingParameterValues.Length);

        //    return parametersWithoutValues.Select(this.GetValueForParameter);
        //}

        ///// <summary>
        ///// Get value for an <see cref="IParameterInfo"/>.
        ///// </summary>
        //private object GetValueForParameter(IParameterInfo parameterInfo)
        //{
        //    this.CustomizeFixtureByParameter(parameterInfo);

        //    return new SpecimenContext(this.Fixture)
        //        .Resolve(parameterInfo.ParameterInfo);
        //}

        //private void CustomizeFixtureByParameter(IParameterInfo parameter)
        //{
        //    var customizeAttributes = parameter.GetCustomAttributes<Attribute>(false)
        //        .OfType<IParameterCustomizationSource>()
        //        .OrderBy(x => x, new CustomizeAttributeComparer());

        //    foreach (var ca in customizeAttributes)
        //    {
        //        var customization = ca.GetCustomization(parameter.ParameterInfo);
        //        this.Fixture.Customize(customization);
        //    }
        //}
    }
}
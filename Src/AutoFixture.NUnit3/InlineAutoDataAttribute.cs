using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using AutoFixture.Kernel;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace AutoFixture.NUnit3;

/// <summary>
/// This attribute acts as a TestCaseAttribute but allow incomplete parameter values,
/// which will be provided by AutoFixture.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
[CLSCompliant(false)]
[SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Justification = "This attribute is the root of a potential attribute hierarchy.")]
public class InlineAutoDataAttribute : Attribute, ITestBuilder
{
    private readonly object[] _existingParameterValues;
    private readonly Lazy<IFixture> _fixtureLazy;

    private IFixture Fixture => _fixtureLazy.Value;

    private ITestMethodBuilder _testMethodBuilder = new FixedNameTestMethodBuilder();

    /// <summary>
    /// Gets or sets the current <see cref="ITestMethodBuilder"/> strategy.
    /// </summary>
    public ITestMethodBuilder TestMethodBuilder
    {
        get => _testMethodBuilder;
        set => _testMethodBuilder = value ?? throw new ArgumentNullException(nameof(value));
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

        _fixtureLazy = new Lazy<IFixture>(() => fixture, LazyThreadSafetyMode.None);
        _existingParameterValues = arguments ?? new object[] { null };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AutoDataAttribute"/> class
    /// with the supplied <paramref name="fixtureFactory"/>. Fixture will be created
    /// on demand using the provided factory.
    /// </summary>
    protected InlineAutoDataAttribute(Func<IFixture> fixtureFactory, params object[] arguments)
    {
        if (fixtureFactory == null) throw new ArgumentNullException(nameof(fixtureFactory));

        _fixtureLazy = new Lazy<IFixture>(fixtureFactory, LazyThreadSafetyMode.PublicationOnly);
        _existingParameterValues = arguments ?? new object[] { null };
    }

    /// <summary>
    /// Gets the parameter values for the test method.
    /// </summary>
    public IEnumerable<object> Arguments => _existingParameterValues;

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

        var test = TestMethodBuilder.Build(
            method, suite, GetParameterValues(method), _existingParameterValues.Length);

        yield return test;
    }

    /// <summary>
    /// Get values for a collection of <see cref="IParameterInfo"/>.
    /// </summary>
    private IEnumerable<object> GetParameterValues(IMethodInfo method)
    {
        var parameters = method.GetParameters();
        return _existingParameterValues.Concat(GetMissingValues(parameters));
    }

    private IEnumerable<object> GetMissingValues(IEnumerable<IParameterInfo> parameters)
    {
        var parametersWithoutValues = parameters.Skip(_existingParameterValues.Length);

        return parametersWithoutValues.Select(GetValueForParameter);
    }

    /// <summary>
    /// Get value for an <see cref="IParameterInfo"/>.
    /// </summary>
    private object GetValueForParameter(IParameterInfo parameterInfo)
    {
        CustomizeFixtureByParameter(parameterInfo);

        return new SpecimenContext(Fixture)
            .Resolve(parameterInfo.ParameterInfo);
    }

    private void CustomizeFixtureByParameter(IParameterInfo parameter)
    {
        var customizeAttributes = parameter.GetCustomAttributes<Attribute>(false)
            .OfType<IParameterCustomizationSource>()
            .OrderBy(x => x, new CustomizeAttributeComparer());

        foreach (var ca in customizeAttributes)
        {
            var customization = ca.GetCustomization(parameter.ParameterInfo);
            Fixture.Customize(customization);
        }
    }
}
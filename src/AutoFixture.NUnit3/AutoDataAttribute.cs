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
/// This attribute uses AutoFixture to generate values for unit test parameters.
/// This implementation is based on TestCaseAttribute of NUnit3.
/// </summary>
[SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes",
    Justification = "This attribute is the root of a potential attribute hierarchy.")]
[AttributeUsage(AttributeTargets.Method)]
public class AutoDataAttribute : Attribute, ITestBuilder
{
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
    /// Construct a <see cref="AutoDataAttribute"/>.
    /// </summary>
    public AutoDataAttribute()
        : this(() => new Fixture())
    {
    }

    /// <summary>
    /// Construct a <see cref="AutoDataAttribute"/> with an <see cref="IFixture"/>.
    /// </summary>
    [Obsolete("This constructor overload is deprecated because it offers poor performance, and will be removed in a future version. " +
              "Please use the AutoDataAttribute(Func<IFixture> fixtureFactory) overload, so fixture will be constructed only if needed.")]
    protected AutoDataAttribute(IFixture fixture)
    {
        if (fixture == null)
        {
            throw new ArgumentNullException(nameof(fixture));
        }

        _fixtureLazy = new Lazy<IFixture>(() => fixture, LazyThreadSafetyMode.None);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AutoDataAttribute"/> class
    /// with the supplied <paramref name="fixtureFactory"/>. Fixture will be created
    /// on demand using the provided factory.
    /// </summary>
    /// <param name="fixtureFactory">The fixture factory used to construct the fixture.</param>
    protected AutoDataAttribute(Func<IFixture> fixtureFactory)
    {
        if (fixtureFactory == null) throw new ArgumentNullException(nameof(fixtureFactory));

        _fixtureLazy = new Lazy<IFixture>(fixtureFactory, LazyThreadSafetyMode.PublicationOnly);
    }

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

        var test = TestMethodBuilder.Build(method, suite, GetParameterValues(method), 0);

        yield return test;
    }

    private IEnumerable<object> GetParameterValues(IMethodInfo method)
    {
        return method.GetParameters().Select(Resolve);
    }

    private object Resolve(IParameterInfo parameterInfo)
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
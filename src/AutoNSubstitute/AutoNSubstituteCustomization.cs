using System;
using AutoFixture.Kernel;
using NSubstitute.Core;

namespace AutoFixture.AutoNSubstitute;

/// <summary>Enables auto-mocking with NSubstitute.</summary>
/// <remarks>
/// NOTICE! You can assign the customization properties to tweak the features you would like to enable. See example.
/// <br />
/// <code>new AutoNSubstituteCustomization { ConfigureMembers = true }</code>
/// </remarks>
public class AutoNSubstituteCustomization : ICustomization
{
    private ISpecimenBuilder _relay;

    /// <summary>Initializes a new instance of the <see cref="AutoNSubstituteCustomization"/> class.
    /// <para>
    /// NOTICE! You can assign the customization properties to tweak the features you would like to enable. Example:
    /// <br />
    /// <code>new AutoNSubstituteCustomization { ConfigureMembers = true }</code>
    /// </para>
    /// </summary>
    public AutoNSubstituteCustomization()
    {
        _relay = new SubstituteRelay();
    }

    /// <summary>Gets or sets the relay that will be added to <see cref="IFixture.ResidueCollectors"/> when <see cref="Customize"/> is invoked.</summary>
    public ISpecimenBuilder Relay
    {
        get => _relay;
        set => _relay = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Specifies whether members of a substitute will be automatically configured to retrieve the return values from a fixture.
    /// </summary>
    public bool ConfigureMembers { get; set; }

    /// <summary>
    /// If value is <c>true</c>, delegate requests are intercepted and created by NSubstitute.
    /// Otherwise, if value is <c>false</c>, delegates are created by the AutoFixture kernel.
    /// </summary>
    public bool GenerateDelegates { get; set; }

    /// <summary>Customizes an <see cref="IFixture"/> to enable auto-mocking with NSubstitute.</summary>
    public void Customize(IFixture fixture)
    {
        if (fixture == null) throw new ArgumentNullException(nameof(fixture));

        ISpecimenBuilder substituteBuilder = new SubstituteRequestHandler(
            new MethodInvoker(
                new NSubstituteMethodQuery()));

        if (ConfigureMembers)
        {
            substituteBuilder = new Postprocessor(
                substituteBuilder,
                new CompositeSpecimenCommand(
                    new NSubstituteRegisterCallHandlerCommand(SubstitutionContext.Current),
                    new NSubstituteSealedPropertiesCommand()));
        }

        fixture.Customizations.Insert(0, substituteBuilder);
        fixture.Customizations.Insert(0, new SubstituteAttributeRelay());
        fixture.ResidueCollectors.Add(Relay);

        if (GenerateDelegates)
        {
            fixture.Customizations.Add(new SubstituteRelay(new DelegateSpecification()));
        }
    }
}
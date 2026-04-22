using System;
using AutoFixture.Kernel;

namespace AutoFixture.AutoFakeItEasy;

/// <summary>
/// Enables auto-mocking with FakeItEasy.
/// </summary>
/// <remarks>
/// NOTICE! You can assign the customization properties to tweak the features you would like to enable. See example.
/// <br />
/// <code>new AutoFakeItEasyCustomization { GenerateDelegates = true }</code>
/// </remarks>
public class AutoFakeItEasyCustomization : ICustomization
{
    private ISpecimenBuilder _relay;

    /// <summary>
    /// Initializes a new instance of the <see cref="AutoFakeItEasyCustomization"/> class.
    /// <para>
    /// NOTICE! You can assign the customization properties to tweak the features you would like to enable. Example:
    /// <br />
    /// <code>new AutoFakeItEasyCustomization { GenerateDelegates = true }</code>
    /// </para>
    /// </summary>
    public AutoFakeItEasyCustomization()
    {
        _relay = new FakeItEasyRelay();
    }

    /// <summary>
    /// Gets or sets the relay that will be added to <see cref="IFixture.ResidueCollectors"/> when
    /// <see cref="Customize"/> is invoked.
    /// </summary>
    public ISpecimenBuilder Relay
    {
        get => _relay;
        set => _relay = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// When <see langword="true"/>, configures the fixture to automatically generate Fakes when a delegate is requested.
    /// </summary>
    public bool GenerateDelegates { get; set; }

    /// <summary>
    /// Specifies whether members of a Fake will be automatically setup to retrieve the return values from a fixture.
    /// </summary>
    public bool ConfigureMembers { get; set; }

    /// <summary>
    /// Customizes an <see cref="IFixture"/> to enable auto-mocking with FakeItEasy.
    /// </summary>
    /// <param name="fixture">The fixture upon which to enable auto-mocking.</param>
    public void Customize(IFixture fixture)
    {
        if (fixture is null) throw new ArgumentNullException(nameof(fixture));

        if (GenerateDelegates)
        {
            fixture.Customizations.Add(new FakeItEasyRelay(new DelegateSpecification()));
        }

        ISpecimenBuilder fakeBuilder = new FakeItEasyBuilder(new MethodInvoker(new FakeItEasyMethodQuery()));

        if (ConfigureMembers)
        {
            fakeBuilder = new Postprocessor(
                builder: fakeBuilder,
                command: new CompositeSpecimenCommand(
                    new ConfigureSealedMembersCommand(),
                    new ConfigureFakeMembersCommand()));
        }

        fixture.Customizations.Add(fakeBuilder);
        fixture.ResidueCollectors.Add(Relay);
    }
}
using System;

namespace AutoFixture.Xunit2.Internal;

internal sealed class NullCustomization : ICustomization
{
    private NullCustomization()
    {
        // prevent external instantiation
    }

    private static readonly Lazy<NullCustomization> s_lazyInstance = new(
        () => new NullCustomization(), isThreadSafe: true);

    public static NullCustomization Instance => s_lazyInstance.Value;

    public void Customize(IFixture fixture)
    {
        // intentionally left blank
    }
}
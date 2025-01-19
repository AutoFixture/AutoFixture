using System;

namespace AutoFixture.Xunit3.Internal
{
    internal sealed class NullCustomization : ICustomization
    {
        private NullCustomization()
        {
            // prevent external instantiation
        }

        private static readonly Lazy<NullCustomization> LazyInstance = new(
            () => new NullCustomization(), isThreadSafe: true);

        public static NullCustomization Instance => LazyInstance.Value;

        public void Customize(IFixture fixture)
        {
            // intentionally left blank
        }
    }
}
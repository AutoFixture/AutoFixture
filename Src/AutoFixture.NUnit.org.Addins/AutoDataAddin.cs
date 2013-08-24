using NUnit.Core.Extensibility;
using Ploeh.AutoFixture.NUnit.org.Addins.Builders;

namespace Ploeh.AutoFixture.NUnit.org.Addins
{
    [NUnitAddin(Description = "AutoFixture.NUnit.org.Addin.AutoData Plugin")]
    public class AutoDataAddin : IAddin
    {
        public bool Install(IExtensionHost host)
        {
            var autoDataBuilders = host.GetExtensionPoint("TestCaseBuilders");
            autoDataBuilders.Install(new AutoDataTestCaseBuilder());

            return true;
        }
    }
}
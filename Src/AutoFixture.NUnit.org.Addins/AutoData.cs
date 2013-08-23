using NUnit.Core.Extensibility;
using Ploeh.AutoFixture.NUnit.org.Addins.Builders;

namespace Ploeh.AutoFixture.NUnit.org.Addins
{
    [NUnitAddin(Description = "AutoFixture.NUnit.org.Addin.AutoData Plugin")]
    public class AutoTestCase : IAddin
    {
        public bool Install(IExtensionHost host)
        {
            var autoDataProviders = host.GetExtensionPoint("TestCaseProviders");
            autoDataProviders.Install(new AutoDataProvider());

            var autoDataBuilders = host.GetExtensionPoint("TestCaseBuilders");
            autoDataBuilders.Install(new AutoDataBuilder());

            return true;
        }
    }
}
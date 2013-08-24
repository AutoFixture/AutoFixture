using NUnit.Core.Extensibility;
using Ploeh.AutoFixture.NUnit.org.Addins.Builders;

namespace Ploeh.AutoFixture.NUnit.org.Addins
{
    [NUnitAddin(Description = "AutoFixture.NUnit.org.Addin.AutoData Plugin")]
    public class AutoTestCaseAddin : IAddin
    {
        public bool Install(IExtensionHost host)
        {
            var autoDataBuilders = host.GetExtensionPoint("TestCaseBuilders");
            autoDataBuilders.Install(new AutoTestCaseBuilder());

            return true;
        }
    }
}
using NUnit.Core.Extensibility;
using Ploeh.AutoFixture.NUnit.Builders;
using Ploeh.AutoFixture.NUnit.Listeners;

namespace Ploeh.AutoFixture.NUnit
{
    [NUnitAddin(Name = "AutoTestCaseExtension", Description = "AutoTestCase Plugin")]
    public class Addin : IAddin
    {
        public bool Install(IExtensionHost host)
        {
            var listeners = host.GetExtensionPoint("EventListeners");
            if (listeners == null) 
                return false;

            listeners.Install(new AutoTestCaseEventListener());
            
            var providers = host.GetExtensionPoint("TestCaseProviders");
            if (providers == null) 
                return false;

            providers.Install(new AutoTestCaseProvider());
            return true;
        }
    }
}
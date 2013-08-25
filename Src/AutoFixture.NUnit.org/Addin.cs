using NUnit.Core.Extensibility;
using Ploeh.AutoFixture.NUnit.org.Builders;
using Ploeh.AutoFixture.NUnit.org.Listeners;

namespace Ploeh.AutoFixture.NUnit.org
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

            return true;
        }
    }
}
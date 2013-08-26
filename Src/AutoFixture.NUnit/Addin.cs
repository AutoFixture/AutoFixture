using NUnit.Core.Extensibility;
using Ploeh.AutoFixture.NUnit.Builders;
using Ploeh.AutoFixture.NUnit.Decorators;

namespace Ploeh.AutoFixture.NUnit
{
    [NUnitAddin(Name = "AutoTestCaseExtension", Description = "AutoTestCase Plugin", Type = ExtensionType.Core | ExtensionType.Gui | ExtensionType.Client)]
    public class Addin : IAddin
    {
        public bool Install(IExtensionHost host)
        {
            var decorators = host.GetExtensionPoint("TestDecorators");
            if (decorators == null) 
                return false;
            
            decorators.Install(new CustomDecorator());

            var providers = host.GetExtensionPoint("TestCaseProviders");
            if (providers == null) 
                return false;

            providers.Install(new AutoTestCaseProvider());

            return true;
        }
    }
}
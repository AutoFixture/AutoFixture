using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    [TestClass]
    public class Scenario
    {
        [TestMethod]
        public void CreateSingleStringParameterizedType()
        {
            // Fixture setup
            var container = Scenario.CreateContainer();
            // Exercise system
            var result = (SingleParameterType<string>)container.Create(typeof(SingleParameterType<string>));
            // Verify outcome
            var name = new TextGuidRegex().GetText(result.Parameter);
            string guidString = new TextGuidRegex().GetGuid(result.Parameter);
            Guid g = new Guid(guidString);
            Assert.AreEqual("parameter", name, "Create");
            Assert.AreNotEqual<Guid>(Guid.Empty, g, "Guid part");
            // Teardown
        }

        [TestMethod]
        public void CreateDoubleStringParamterizedType()
        {
            // Fixture setup
            var container = Scenario.CreateContainer();
            // Exercise system
            var result = (DoubleParameterType<string, string>)container.Create(typeof(DoubleParameterType<string, string>));
            // Verify outcome
            Assert.IsFalse(string.IsNullOrEmpty(result.Parameter1), "Parameter1");
            Assert.IsFalse(string.IsNullOrEmpty(result.Parameter2), "Parameter2");
            // Teardown
        }

        private static DefaultSpecimenContainer CreateContainer()
        {
            var builder = new CompositeSpecimenBuilder(
                new GuidStringGenerator(),
                new ModestConstructorInvoker(),
                new ParameterRequestTranslator(),
                new StringSeedUnwrapper(),
                new ValueIgnoringSeedUnwrapper());
            var container = new DefaultSpecimenContainer(builder);
            return container;
        }
    }
}

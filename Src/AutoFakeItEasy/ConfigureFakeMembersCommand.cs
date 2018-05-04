using System;
using System.Reflection;

using AutoFixture.Kernel;
using FakeItEasy;
using FakeItEasy.Core;

namespace AutoFixture.AutoFakeItEasy
{
    /// <summary> 
    /// Sets up a Fake's members so that the return, out, and ref values of virtual members will 
    /// be retrieved from the fixture, instead of being created directly by FakeItEasy. 
    ///  
    /// This will setup virtual methods and properties. 
    /// </summary> 
    /// <remarks> 
    /// This will setup any virtual methods and properties.
    /// This includes: 
    ///  - interface's methods/properties; 
    ///  - class's abstract/virtual/overridden/non-sealed methods/properties. 
    /// </remarks>     
    public class ConfigureFakeMembersCommand : ISpecimenCommand
    {
        /// <summary> 
        /// Sets up a Fake's members so that the return, out, and ref values will be retrieved
        /// from the fixture, instead of being created directly by FakeItEasy. 
        /// </summary> 
        /// <param name="specimen">The Fake to setup.</param> 
        /// <param name="context">The context of the Fake.</param> 
        public void Execute(object specimen, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var fakeManager = GetFakeManager(specimen);
            if (fakeManager == null) return;

            var resultCache = new CallResultCache();
            fakeManager.AddRuleLast(new PropertySetterRule(resultCache));
            fakeManager.AddRuleLast(new MethodRule(context, resultCache));
        }

        private static FakeManager GetFakeManager(object specimen)
        {
            var specimenType = specimen?.GetType();
            if (specimenType == null || !specimenType.IsFake()) return null;

            var fakedObject = specimenType.GetProperty("FakedObject")?.GetValue(specimen, null);
            return fakedObject == null ? null : Fake.GetFakeManager(fakedObject);
        }
    }
}
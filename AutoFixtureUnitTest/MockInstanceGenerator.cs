using System;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    internal class MockInstanceGenerator : IInstanceGenerator
    {
        public MockInstanceGenerator()
        {
            this.CanGenerateCallback = r => false;
            this.GenerateCallback = r => new object();
        }

        public bool CanGenerate(ICustomAttributeProvider request)
        {
            return this.CanGenerateCallback(request);
        }

        public object Generate(ICustomAttributeProvider request)
        {
            return this.GenerateCallback(request);
        }

        internal Func<ICustomAttributeProvider, bool> CanGenerateCallback { get; set; }

        internal Func<ICustomAttributeProvider, object> GenerateCallback { get; set; }
    }
}

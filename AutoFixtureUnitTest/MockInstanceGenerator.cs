using System;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    internal class MockInstanceGenerator : IInstanceGenerator
    {
        public MockInstanceGenerator()
        {
            this.CanGenerateCallback = ap => false;
            this.GenerateCallback = ap => new object();
        }

        public bool CanGenerate(ICustomAttributeProvider attributeProvider)
        {
            return this.CanGenerateCallback(attributeProvider);
        }

        public object Generate(ICustomAttributeProvider attributeProvider)
        {
            return this.GenerateCallback(attributeProvider);
        }

        internal Func<ICustomAttributeProvider, bool> CanGenerateCallback { get; set; }

        internal Func<ICustomAttributeProvider, object> GenerateCallback { get; set; }
    }
}

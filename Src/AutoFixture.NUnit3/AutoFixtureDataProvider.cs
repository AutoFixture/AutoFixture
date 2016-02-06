using System;
using System.Reflection;

namespace Ploeh.AutoFixture.NUnit3
{
    public class AutoFixtureDataProvider : ITypedDataProvider
    {
        protected readonly IFixture Fixture;

        public AutoFixtureDataProvider(params ICustomization[] customizations)
        {
            Fixture = new Fixture();

            foreach (var c in customizations)
            {
                Fixture.Customize(c);
            }
        }

        public object CreateFrozenValue(Type type)
        {
            return this.CallGeneric(type, "Freeze");
        }

        public object CreateValue(Type type)
        {
            return this.CallGeneric(type, "Create");
        }

        private object CallGeneric(Type type, string methodName)
        {
            var methodInfo = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);

            var generic = methodInfo.MakeGenericMethod(type);

            return generic.Invoke(this, null);
        }

        private T Create<T>()
        {
            return this.Fixture.Create<T>();
        }
        
        private T Freeze<T>()
        {
            return this.Fixture.Freeze<T>();
        }
    }
}
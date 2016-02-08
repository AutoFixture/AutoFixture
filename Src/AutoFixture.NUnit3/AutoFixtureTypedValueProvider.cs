using System;
using System.Reflection;

namespace Ploeh.AutoFixture.NUnit3
{
    public class AutoFixtureTypedValueProvider : ITypedValueProvider
    {
        private readonly IFixture _fixture;

        public AutoFixtureTypedValueProvider(IFixture fixture)
        {
            _fixture = fixture;
        }

        public object CreateFrozenValue(Type type)
        {
            return this.CallGeneric(type, "Freeze");
        }

        public object CreateValue(Type type)
        {
            return this.CallGeneric(type, "Create");
        }

        /// <summary>
        /// IFixture.Create{T} and Freeze{T} are extension methods and a bit non-trivial to invoke with genric type parameter
        /// so we make a shortcut here: call them in local generic methods, which can be invoked easier
        /// </summary>
        private object CallGeneric(Type type, string methodName)
        {
            var methodInfo = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);

            var generic = methodInfo.MakeGenericMethod(type);

            return generic.Invoke(this, null);
        }

        private T Create<T>()
        {
            return this._fixture.Create<T>();
        }
        
        private T Freeze<T>()
        {
            return this._fixture.Freeze<T>();
        }
    }
}
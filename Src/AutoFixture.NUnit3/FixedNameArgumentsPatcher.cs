using System;
using System.Reflection;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace AutoFixture.NUnit3
{
    internal class FixedNameArgumentsPatcher : IPatchParameters
    {
        public void Patch(TestCaseParameters parameters, IMethodInfo method)
        {
            EnsureOriginalArgumentsArrayIsNotShared(parameters);

            var methodParameters = method.GetParameters();
            var autoDataStartIndex = parameters.Arguments.Length - methodParameters.Length;

            for (var i = autoDataStartIndex; i < parameters.OriginalArguments.Length; i++)
                parameters.OriginalArguments[i] = new TypeNameRenderer(methodParameters[i].ParameterType);
        }

        /// <summary>
        /// Before NUnit 3.5 the Arguments and OriginalArguments properties are referencing the same
        /// array, so we cannot safely update the OriginalArguments without touching the Arguments
        /// value. This method fixes that by making the OriginalArguments array a standalone copy.
        /// <para>When running in NUnit3.5 and later the method is supposed to do nothing.</para>
        /// </summary>
        protected static void EnsureOriginalArgumentsArrayIsNotShared(TestCaseParameters parameters)
        {
            if (!ReferenceEquals(parameters.Arguments, parameters.OriginalArguments)) return;

            var clonedArguments = new object[parameters.OriginalArguments.Length];
            Array.Copy(parameters.OriginalArguments, clonedArguments, parameters.OriginalArguments.Length);

            // Unfortunately the property has a private setter, so can be updated via reflection
            // only. Should use the type where the property is declared as otherwise the private
            // setter is not available.
            var property = typeof(TestParameters)
                .GetTypeInfo()
                .GetProperty(nameof(TestCaseParameters.OriginalArguments));

            property?.SetValue(parameters, clonedArguments, null);
        }

        private class TypeNameRenderer
        {
            public TypeNameRenderer(Type type)
            {
                this.Type = type ?? throw new ArgumentNullException(nameof(type));
            }

            private Type Type { get; }

            public override string ToString()
            {
                return $"auto<{this.Type.Name}>";
            }
        }
    }
}

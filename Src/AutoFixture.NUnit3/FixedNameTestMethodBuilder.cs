using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace AutoFixture.NUnit3
{
    /// <summary>
    /// Builder that generates <see cref="TestMethod"/> with fixed names.
    /// This might be needed by some test runners to correctly identify tests between the discovery and execution
    /// (e.g. Nunit test adaptor for Visual Studio, NCrunch).
    /// </summary>
    public class FixedNameTestMethodBuilder : ITestMethodBuilder
    {
        /// <inheritdoc />
        public virtual TestMethod Build(
            IMethodInfo method, Test suite, IEnumerable<object> parameterValues, int autoDataStartIndex)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            if (parameterValues == null)
            {
                throw new ArgumentNullException(nameof(parameterValues));
            }

            return new NUnitTestCaseBuilder()
                .BuildTestMethod(method, suite, GetParametersForMethod(method, parameterValues, autoDataStartIndex));
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "This method is always expected to return an instance of the TestCaseParameters class.")]
        private static TestCaseParameters GetParametersForMethod(
            IMethodInfo method, IEnumerable<object> parameterValues, int autoDataStartIndex)
        {
            try
            {
                return GetParametersForMethod(method, parameterValues.ToArray(), autoDataStartIndex);
            }
            catch (Exception ex)
            {
                return new TestCaseParameters(ex);
            }
        }

        private static TestCaseParameters GetParametersForMethod(IMethodInfo method, object[] args, int autoDataStartIndex)
        {
            var result = new TestCaseParameters(args);

            EnsureOriginalArgumentsArrayIsNotShared(result);

            var methodParameters = method.GetParameters();
            for (int i = autoDataStartIndex; i < result.OriginalArguments.Length; i++)
            {
                result.OriginalArguments[i] = new TypeNameRenderer(methodParameters[i].ParameterType);
            }

            return result;
        }

        /// <summary>
        /// Before NUnit 3.5 the Arguments and OriginalArguments properties are referencing the same array, so
        /// we cannot safely update the OriginalArguments without touching the Arguments value.
        /// This method fixes that by making the OriginalArguments array a standalone copy.
        /// <para>
        /// When running in NUnit3.5 and later the method is supposed to do nothing.
        /// </para>
        /// </summary>
        private static void EnsureOriginalArgumentsArrayIsNotShared(TestCaseParameters parameters)
        {
            if (ReferenceEquals(parameters.Arguments, parameters.OriginalArguments))
            {
                var clonedArguments = new object[parameters.OriginalArguments.Length];
                Array.Copy(parameters.OriginalArguments, clonedArguments, parameters.OriginalArguments.Length);
                
                // Unfortunately the property has a private setter, so can be updated via reflection only.
                // Should use the type where the property is declared as otherwise the private setter is not available.
                var property = typeof(TestParameters).GetTypeInfo().GetProperty(nameof(TestCaseParameters.OriginalArguments));
                property.SetValue(parameters, clonedArguments, null);
            }
        }

        private class TypeNameRenderer
        {
            private Type Type { get; }

            public TypeNameRenderer(Type type)
            {
                this.Type = type ?? throw new ArgumentNullException(nameof(type));
            }

            public override string ToString()
            {
                return "auto<" + this.Type.Name + ">";
            }
        }
    }
}

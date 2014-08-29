using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.Exceptions;
using Ploeh.AutoFixture.AutoNSubstitute.Extensions;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    /// <summary>
    /// Sets up a substitute object's methods so that the return values will be retrieved from a fixture,
    /// instead of being created directly by NSubstitute.
    /// 
    /// This will setup any virtual methods that are either non-void or have "out" or "ref" parameters.
    /// </summary>
    /// <remarks>
    /// This will setup any virtual methods that are either non-void or have "out" or "ref" parameters.
    /// This includes:
    ///  - interface's methods/property getters;
    ///  - class's abstract/virtual/overridden/non-sealed methods/property getters.
    /// 
    /// Notes:
    /// - Automatic mocking of generic methods isn't feasible either - we'd have to antecipate any type parameters that this method could be called with. 
    /// - Calling a method more than once with the same parameters will always return the same value, except for void methods with out parameters
    /// </remarks>
    public class NSubstituteVirtualMethodsCommand : ISpecimenCommand
    {
        private static readonly MethodInfo[] ObjectMethods = typeof(object).GetMethods();

        /// <summary>
        /// Sets up a substitute object's methods so that the return values will be retrieved from a fixture,
        /// instead of being created directly by NSubstitute.
        /// </summary>
        /// <param name="specimen">The substitute to setup.</param>
        /// <param name="context">The context of the specimen.</param>
        public void Execute(object specimen, ISpecimenContext context)
        {
            if (specimen == null) throw new ArgumentNullException("specimen");
            if (context == null) throw new ArgumentNullException("context");

            try
            {
                SubstitutionContext.Current.GetCallRouterFor(specimen);
            }
            catch (NotASubstituteException)
            {
                return;
            }

            var substituteType = specimen.GetType().GetSubstituteType();
            var methods = GetVirtualMethods(substituteType);

            var substituteSetup = new SubstituteValueFactory(specimen, context);

            foreach (var method in methods)
                substituteSetup.Setup(method);
        }

        private static IEnumerable<MethodInfo> GetVirtualMethods(Type substituteType)
        {
            return substituteType.GetMethods()
                .Where(method => method.IsOverridable() &&
                                 !method.IsGenericMethod &&
                                 (!method.IsVoid() || method.HasRefParameters())
                                 && !ObjectMethods.Contains(method.GetBaseDefinition()));
        }

        private class SubstituteValueFactory
        {
            private static readonly MethodInfo ReturnsUsingContextMethodInfo = typeof(SubstituteValueFactory).GetMethod("ReturnsUsingContext");
            private readonly object substitute;
            private readonly ISpecimenContext context;

            public SubstituteValueFactory(object substitute, ISpecimenContext context)
            {
                if (substitute == null)
                    throw new ArgumentNullException("substitute");

                if (context == null)
                    throw new ArgumentNullException("context");

                this.substitute = substitute;
                this.context = context;
            }

            public object Substitute
            {
                get { return substitute; }
            }

            public ISpecimenContext Context
            {
                get { return context; }
            }

            public void ReturnsUsingContext<T>(MethodInfo methodInfo)
            {
                bool recursive = false;
                InvokeMethod(methodInfo);
                default(T).ReturnsForAnyArgs<T>(callInfo =>
                {
                    if (recursive)
                        return default(T);

                    recursive = true;
                    var value = (T)Context.Resolve(typeof(T));
                    ReturnsFixedValue(methodInfo, callInfo, value);
                    InvokeMethod(methodInfo, callInfo.Args());
                    recursive = false;

                    return value;
                });
            }

            private void ReturnsFixedValue<T>(MethodInfo methodInfo, CallInfo callInfo, T value)
            {
                var refValues = GetFixedRefValues(methodInfo);

                object[] arguments = callInfo.Args();
                SetRefValues(callInfo, refValues);

                InvokeMethod(methodInfo, arguments);
                value.Returns<T>(x =>
                {
                    SetRefValues(x, refValues);
                    return value;
                });
            }

            private IEnumerable<Tuple<int, object>> GetFixedRefValues(MethodInfo methodInfo)
            {
                return GetRefParameters(methodInfo)
                    .Select(t => Tuple.Create(t.Item1, Context.Resolve(t.Item2.ParameterType.GetElementType())))
                    .ToList();
            }

            public void Setup(MethodInfo methodInfo)
            {
                SetupReturnValue(methodInfo);
                SetupRefReturnValues(methodInfo);
            }

            private void SetupRefReturnValues(MethodInfo methodInfo)
            {
                if (!methodInfo.IsVoid())
                    return;

                if (!methodInfo.HasRefParameters())
                    return;

                Substitute.WhenForAnyArgs(x => InvokeMethod(methodInfo))
                    .Do(callInfo => SetRefValues(callInfo, GetFixedRefValues(methodInfo)));
            }

            private static void SetRefValues(CallInfo callInfo, IEnumerable<Tuple<int, object>> values)
            {
                foreach (var value in values)
                    callInfo[value.Item1] = value.Item2;
            }

            private static IEnumerable<Tuple<int, ParameterInfo>> GetRefParameters(MethodInfo methodInfo)
            {
                return methodInfo.GetParameters()
                    .Select((p, i) => Tuple.Create(i, p))
                    .Where(t => t.Item2.ParameterType.IsByRef);
            }

            private void SetupReturnValue(MethodInfo methodInfo)
            {
                if (methodInfo.IsVoid())
                    return;

                ReturnsUsingContextMethodInfo.MakeGenericMethod(methodInfo.ReturnType).Invoke(this, new object[] { methodInfo });
            }

            private object InvokeMethod(MethodInfo methodInfo, object[] parameters = null)
            {
                return methodInfo.Invoke(Substitute, parameters ?? GetDefaultParameters(methodInfo));
            }

            private static object[] GetDefaultParameters(MethodInfo methodInfo)
            {
                return methodInfo.GetParameters()
                    .Select(p => p.ParameterType.GetDefault())
                    .ToArray();
            }
        }
    }
}
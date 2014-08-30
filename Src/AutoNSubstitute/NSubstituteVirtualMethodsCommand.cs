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
    /// - Void methods are not set up due to a limitation in NSubstitute that When..Do setups can't be overriden
    /// - Calling a method more than once with the same parameters will always return the same value
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
                                 !method.IsVoid() &&
                                 !ObjectMethods.Contains(method.GetBaseDefinition()));
        }

        internal class LateBoundMethod
        {
            private readonly Type type;
            private readonly MethodInfo signature;
            private MethodInfo methodInfo;

            public LateBoundMethod(Type type, MethodInfo signature)
            {
                if (type == null)
                    throw new ArgumentNullException("type");

                if (signature == null)
                    throw new ArgumentNullException("signature");

                this.type = type;
                this.signature = signature;
            }

            public Type Type
            {
                get { return type; }
            }

            public MethodInfo Signature
            {
                get { return signature; }
            }

            private MethodInfo MethodInfo
            {
                get
                {
                    if (this.methodInfo == null)
                        this.methodInfo = GetMethod(Type, Signature);

                    return this.methodInfo;
                }
            }

            private static object Invoke(MethodInfo methodInfo, params object[] arguments)
            {
                var parameters = methodInfo.GetParameters();
                return methodInfo.Invoke(null, parameters.Select((p, i) =>
                    arguments.Length > i ? arguments[i] : p.ParameterType.GetDefault())
                    .ToArray());
            }

            public object Invoke<T>(params object[] arguments)
            {
                return Invoke(MethodInfo.MakeGenericMethod(typeof (T)), arguments);
            }

            private static MethodInfo GetMethod(Type type, MethodInfo signature)
            {
                return (from method in type.GetMethods()
                        where method.Name == signature.Name
                        let methodParameters = method.GetParameters()
                        let signatureParameters = signature.GetParameters()
                        where methodParameters.Length >= signatureParameters.Length
                        orderby methodParameters.Length
                        where methodParameters.All(p =>
                            p.Position >= signatureParameters.Length ?
                            p.IsOptional || p.IsDefined(typeof(ParamArrayAttribute), true) :
                            p.ParameterType.ToString() == signatureParameters[p.Position].ParameterType.ToString())
                        select method)
                        .Single();
            }

        }

        private class SubstituteValueFactory
        {
            private static readonly MethodInfo ReturnsUsingContextMethodInfo = 
                typeof(SubstituteValueFactory).GetMethod("ReturnsUsingContext");
            private static readonly LateBoundMethod ReturnsForAnyArgsMethodInfo = 
                new LateBoundMethod(typeof(SubstituteExtensions), typeof(SubstituteValueFactory).GetMethod("ReturnsForAnyArgs"));
            private static readonly LateBoundMethod ReturnsMethodInfo = 
                new LateBoundMethod(typeof(SubstituteExtensions), typeof(SubstituteValueFactory).GetMethod("Returns"));
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

            public static void ReturnsForAnyArgs<T>(T value, Func<CallInfo, T> returnThis)
            {
                ReturnsForAnyArgsMethodInfo.Invoke<T>(value, returnThis);
            }

            public static void Returns<T>(T value, Func<CallInfo, T> returnThis)
            {
                ReturnsMethodInfo.Invoke<T>(value, returnThis);
            }

            public void ReturnsUsingContext<T>(MethodInfo methodInfo)
            {
                InvokeMethod(methodInfo);
                ReturnsForAnyArgs<T>(default(T), callInfo =>
                {
                    var value = new Lazy<T>(() => (T)Context.Resolve(typeof(T)));
                    object[] arguments = callInfo.Args();
                    ReturnsFixedValue(methodInfo, callInfo, value);
                    InvokeMethod(methodInfo, arguments);

                    return value.Value;
                });
            }

            private void ReturnsFixedValue<T>(MethodInfo methodInfo, CallInfo callInfo, Lazy<T> value)
            {
                var refValues = GetFixedRefValues(methodInfo);
                Returns<T>(default(T), x =>
                {
                    SetRefValues(x, refValues);
                    return value.Value;
                });
                SetRefValues(callInfo, refValues);
            }

            private IEnumerable<Tuple<int, Lazy<object>>> GetFixedRefValues(MethodInfo methodInfo)
            {
                return GetRefParameters(methodInfo)
                    .Select(t => Tuple.Create(t.Item1, new Lazy<object>(() => Context.Resolve(t.Item2.ParameterType.GetElementType()))))
                    .ToList();
            }

            public void Setup(MethodInfo methodInfo)
            {
                if (methodInfo.IsVoid())
                    return;

                ReturnsUsingContextMethodInfo.MakeGenericMethod(methodInfo.ReturnType)
                    .Invoke(this, new object[] { methodInfo });
            }

            private static void SetRefValues(CallInfo callInfo, IEnumerable<Tuple<int, Lazy<object>>> values)
            {
                foreach (var value in values)
                    callInfo[value.Item1] = value.Item2.Value;
            }

            private static IEnumerable<Tuple<int, ParameterInfo>> GetRefParameters(MethodInfo methodInfo)
            {
                return methodInfo.GetParameters()
                    .Select((p, i) => Tuple.Create(i, p))
                    .Where(t => t.Item2.ParameterType.IsByRef);
            }

            private void InvokeMethod(MethodInfo methodInfo, object[] parameters = null)
            {
                methodInfo.Invoke(Substitute, parameters ?? GetDefaultParameters(methodInfo));
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
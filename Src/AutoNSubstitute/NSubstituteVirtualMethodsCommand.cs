using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
    /// This will setup any non-void virtual methods.
    /// </summary>
    /// <remarks>
    /// This will setup any non-void virtual methods.
    /// This includes:
    ///  - interface's methods/property getters;
    ///  - class's abstract/virtual/overridden/non-sealed methods/property getters.
    /// 
    /// Notes:
    /// - Automatic mocking of generic methods isn't feasible - we'd have to antecipate any type 
    ///     parameters that this method could be called with. 
    /// - Void methods are not set up due to a limitation in NSubstitute that When..Do setups can't be overriden
    /// - Calling a method more than once with the same parameters will always return the same value
    /// - Methods inherited from <see cref="Object" /> are not set up due to a limitation in NSubstitute
    ///     (http://stackoverflow.com/a/21787891)
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
            return GetMethods(substituteType)
                .Where(method => method.IsOverridable() &&
                                 !method.IsGenericMethod &&
                                 !method.IsVoid() &&
                                 !ObjectMethods.Contains(method.GetBaseDefinition()));
        }

        private static IEnumerable<MethodInfo> GetMethods(Type substituteType)
        {
            if (substituteType.IsInterface)
                return GetInterfaceMethods(substituteType);

            return substituteType.GetMethods();
        }

        private static IEnumerable<MethodInfo> GetInterfaceMethods(Type type)
        {
            return type.GetMethods().Concat(
                type.GetInterfaces()
                    .SelectMany(@interface => @interface.GetMethods()));
        }

        private class SubstituteValueFactory
        {
            private static readonly MethodInfo ReturnsUsingContextMethodInfo =
                typeof(SubstituteValueFactory).GetMethod("ReturnsUsingContext");
            private static readonly IMethod ReturnsMethodInfo =
                GetNSubstituteMethod("Returns");

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "AutoFixture", Justification = "Workaround for a bug in CA: https://connect.microsoft.com/VisualStudio/feedback/details/521030/")]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "github", Justification = "Workaround for a bug in CA: https://connect.microsoft.com/VisualStudio/feedback/details/521030/")]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "NSubstitute", Justification = "Workaround for a bug in CA: https://connect.microsoft.com/VisualStudio/feedback/details/521030/")]
            private static IMethod GetNSubstituteMethod(string methodName)
            {
                Type substituteType = typeof(SubstituteExtensions);
                MethodInfo methodInfo = typeof(SubstituteValueFactory).GetMethod(methodName);

                try
                {
                    return new TemplateMethodQuery(methodInfo)
                        .SelectMethods(substituteType)
                        .First();
                }
                catch (InvalidOperationException)
                {
                    throw new MissingMethodException(string.Format(CultureInfo.CurrentCulture,
                        @"The method '{0} {1}.{2}{3}({4})' was not found. This can happen if you updated NSubstitute to an unsupported version; if this is the case, open an issue at http://github.com/AutoFixture/AutoFixture informing this exception message and the version you have installed.",
                        GetFriendlyName(methodInfo.ReturnType),
                        substituteType,
                        methodInfo.Name,
                        GetTypeArguments(methodInfo),
                        string.Join(", ",
                            methodInfo.GetParameters()
                            .Select(p => GetFriendlyName(p.ParameterType)))));
                }
            }

            private static string GetTypeArguments(MethodInfo methodInfo)
            {
                var typeArguments = methodInfo.GetGenericArguments();

                if (typeArguments.Length == 0)
                    return string.Empty;

                return string.Format(CultureInfo.CurrentCulture, "<{0}>",
                    string.Join(", ", methodInfo.GetGenericArguments().Select(a => a.ToString())));
            }

            private static string GetFriendlyName(Type type)
            {
                if (type.IsGenericType)
                    return string.Format(CultureInfo.CurrentCulture,
                        "{0}<{1}>",
                        type.Name.Split('`')[0],
                        string.Join(", ", type.GetGenericArguments().Select(GetFriendlyName)));

                return type.Name;
            }

            private readonly object substitute;
            private readonly ISpecimenContext context;

            private readonly List<Tuple<MethodInfo, object[]>> methodCalls =
                new List<Tuple<MethodInfo, object[]>>();

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

            public static void Returns<T>(T value, Func<CallInfo, T> returnThis)
            {
                ReturnsMethodInfo.Invoke(new object[] { value, returnThis });
            }

            public void ReturnsUsingContext<T>(MethodInfo methodInfo)
            {
                Substitute
                    .WhenForAnyArgs(_ => InvokeMethod(methodInfo))
                    .Do(callInfo =>
                    {
                        var arguments = callInfo.Args();
                        var call = Tuple.Create(methodInfo, arguments.ToArray());
                        if (methodCalls.Any(
                            x =>
                                call.Item1 == x.Item1 &&
                                call.Item2.SequenceEqual(x.Item2)))
                            return;
                        methodCalls.Add(call);

                        var value = Resolve<T>();
                        if (value is OmitSpecimen)
                            return;

                        ReturnsFixedValue(methodInfo, value);
                        InvokeMethod(methodInfo, arguments);
                    });
            }

            private object Resolve<T>()
            {
                return Task.Factory
                    .StartNew(() => Context.Resolve(typeof (T)))
                    .Result;
            }

            private void ReturnsFixedValue<T>(MethodInfo methodInfo, T value)
            {
                var refValues = GetFixedRefValues(methodInfo);
                Returns<T>(default(T), x =>
                {
                    SetRefValues(x, refValues);
                    return value;
                });
            }

            private IEnumerable<Tuple<int, Lazy<object>>> GetFixedRefValues(MethodInfo methodInfo)
            {
                return GetRefParameters(methodInfo)
                    .Select(t => Tuple.Create(t.Item1,
                        new Lazy<object>(() => Context.Resolve(t.Item2.ParameterType.GetElementType()))))
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
                    if (!(value.Item2.Value is OmitSpecimen))
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
                    .Select(p => p.ParameterType.IsValueType ? Activator.CreateInstance(p.ParameterType) : null)
                    .ToArray();
            }
        }
    }
}
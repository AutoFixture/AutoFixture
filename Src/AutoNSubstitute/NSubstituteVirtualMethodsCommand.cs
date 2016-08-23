using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.Exceptions;
using NSubstitute.Routing;
using NSubstitute.Routing.Handlers;
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

            var substituteTypes = specimen.GetType().GetSubstituteTypes();
            foreach (var substituteType in substituteTypes)
            {
                var methods = GetVirtualMethods(substituteType);

                var substituteSetup = new SubstituteValueFactory(specimen, context);

                foreach (var method in methods)
                    substituteSetup.Setup(method);
            }
        }

        private static IEnumerable<MethodInfo> GetVirtualMethods(Type substituteType)
        {
            return substituteType.GetMethods()
                .Where(method => method.IsOverridable() &&
                                 !method.IsGenericMethod &&
                                 !method.IsVoid() &&
                                 !ObjectMethods.Contains(method.GetBaseDefinition()));
        }

        private class SubstituteValueFactory
        {
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

            public void ReturnsUsingContext(MethodInfo methodInfo)
            {
                // The workflow is following:
                // 1. We setup callback that lazily configures return value.
                // 2. When callback is executed for the first time, it sets up own call handlers.
                //    Then we invoke method again to pass control to that handlers.
                // 3. NoSetupCallbackHandler uses substitute state to check whether we already have result for the call.
                //    That could happen if client already configured return value for particular property/method manually.
                //    Also, that happens if we already configured return value for the call.
                //      
                //    If value is not already present:
                //    3.1 Resolve result using the AutoFixture.
                //    3.2 Configure NSubstitute to return that value using the `Returns()` method.
                //    3.3 Invoke the method again. The points 1-3 will be repeated, 
                //           but this time the NoSetupCallbackHandler will do nothing - we have a result.
                //
                // 
                // Consider the following things during the code analysis:
                //  - Each time consumer invokes property/method, we set our custom route and pass control to it.
                //    We need that to access substitute state and check whether we already have a result for the call.
                //
                //  - Each time our route is executed, NSubstitute switches to default route.
                //  - Our route is executed twice during inital setup. Second time it does nothing.
                //  - The initial request (which comes from consumer) is in progress (on stack below) when we do all our configuration.
                //    That is because Do() {} callback is executed before known return values are returned.
                //    After our substitute is configured in When-Do, NSubstitute checks whether there is return value for current call.
                //    Return value is, of course, present and it returns that value to the consumer.

                Substitute
                    .WhenForAnyArgs(_ => InvokeMethod(methodInfo))
                    .Do(callInfo =>
                    {
                        var arguments = callInfo.Args();

                        var callRouter =
                            SubstitutionContext.Current.GetCallRouterFor(Substitute);

                        callRouter.SetRoute(state => new Route(
                            new ICallHandler[] {
                                new NoSetupCallbackHandler(state, () => {
                                    var value = Resolve(methodInfo.ReturnType);
                                    if (value is OmitSpecimen)
                                        return;

                                    ReturnsFixedValue(methodInfo, value);
                                    InvokeMethod(methodInfo, arguments);
                                }),
                                new ReturnDefaultForReturnTypeHandler(
                                    new DefaultForType())
                            }));
                        InvokeMethod(methodInfo, arguments);
                    });
            }

            private object Resolve(Type type)
            {
                // NSubstitute uses a static property SubstitutionContext.Current
                // to get and set the state of its substitutes.
                // However, it uses ThreadLocal so one thread does not
                // interfere in another.
                // For this reason, Context.Resolve needs to run in another thread,
                // otherwise, NSubstitute would not be able to set up the methods
                // that return a circular reference.
                // See discussion at https://github.com/AutoFixture/AutoFixture/pull/397

                using (var cancelableTokenSource = new CancellationTokenSource())
                {
                    var cancelableToken = cancelableTokenSource.Token;
                    // Run task on default scheduler to prevent attach to the context scheduler.
                    // User could execute this code in context of its own scheduler that performs aggressive inling
                    // and our task will be inlined by that scheduler.
                    var task = Task.Factory.StartNew(() => Context.Resolve(type), cancelableToken,
                        TaskCreationOptions.None, TaskScheduler.Default);

                    // It could happen that task above is inlined on the current thread.
                    // As result, the last NSubstitute call router could become empty.
                    // Therefore, we pass cancellation token to the Wait() method to prevent task from being inlined.
                    // See more details: http://stackoverflow.com/a/12246045/2009373
                    // This fixes the following issue: https://github.com/AutoFixture/AutoFixture/issues/630
                    task.Wait(cancelableToken);
                    return task.Result;
                }
            }

            private void ReturnsFixedValue(MethodInfo methodInfo, object value)
            {
                var refValues = GetFixedRefValues(methodInfo);
                Returns(null, x =>
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

                this.ReturnsUsingContext(methodInfo);
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
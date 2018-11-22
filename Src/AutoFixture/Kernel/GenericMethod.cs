using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Encapsulates a generic method, inferring the type parameters bases on invocation arguments.
    /// </summary>
    public class GenericMethod : IMethod
    {
        private readonly ParameterInfo[] parametersInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericMethod"/> class.
        /// </summary>
        /// <param name="method">The method info.</param>
        /// <param name="factory">
        /// The factory to create an <see cref="IMethod" /> from the <see cref="MethodInfo" />
        /// resolved by <see cref="GenericMethod" />.
        /// </param>
        public GenericMethod(MethodInfo method, IMethodFactory factory)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            this.Method = method;
            this.Factory = factory;
            this.parametersInfo = method.GetParameters();
        }

        /// <summary>
        /// Gets information about the parameters of the method.
        /// </summary>
        public IEnumerable<ParameterInfo> Parameters => this.parametersInfo;

        /// <summary>
        /// Gets information about the method.
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        /// Gets information about the factory to create <see cref="IMethod" /> from
        /// <see cref="MethodInfo" />.
        /// </summary>
        public IMethodFactory Factory { get; }

        private static IEnumerable<Tuple<Type, Type>> ResolveGenericType(Type argument, Type parameter)
        {
            if (argument == null)
                return Enumerable.Empty<Tuple<Type, Type>>();

            if (parameter == null)
                return Enumerable.Empty<Tuple<Type, Type>>();

            if (parameter.IsGenericParameter)
                return new[] { Tuple.Create(parameter, argument) };

            var argumentTypeArguments = GetTypeArguments(argument);
            var parameterTypeArguments = GetTypeArguments(parameter);

            return argumentTypeArguments
                .Zip(parameterTypeArguments, ResolveGenericType)
                .SelectMany(x => x);
        }

        private static IEnumerable<Type> GetTypeArguments(Type type)
        {
            return type.HasElementType ?
                new[] { type.GetElementType() } :
                type.GetTypeInfo().GetGenericArguments();
        }

        private static MethodInfo InferMethodInfo(MethodInfo methodInfo, IEnumerable<object> arguments)
        {
            if (methodInfo.ContainsGenericParameters)
            {
                var typeMap = arguments.Zip(methodInfo.GetParameters(),
                (argument, parameter) => ResolveGenericType(GetArgumentTypeOrObjectType(argument), parameter.ParameterType))
                .SelectMany(x => x)
                .ToLookup(x => x.Item1, x => x.Item2);

                var actuals = methodInfo.GetGenericArguments()
                    .Select(x =>
                    {
                        if (!typeMap.Contains(x))
                            throw new TypeArgumentsCannotBeInferredException(methodInfo);

                        return typeMap[x].Aggregate((t1, t2) => t1.GetTypeInfo().IsAssignableFrom(t2) ? t2 : t1);
                    });

                return methodInfo.MakeGenericMethod(actuals.ToArray());
            }

            return methodInfo;
        }

        private static Type GetArgumentTypeOrObjectType(object argument)
        {
            return argument?.GetType() ?? typeof(object);
        }

        /// <summary>
        /// Invokes the method with the supplied parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The result of the method call.</returns>
        public object Invoke(IEnumerable<object> parameters)
        {
            var arguments = parameters.ToArray();
            return this.Factory.Create(InferMethodInfo(this.Method, arguments))
                .Invoke(arguments);
        }
    }
}

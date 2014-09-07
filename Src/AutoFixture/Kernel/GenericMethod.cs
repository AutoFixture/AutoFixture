using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Encapsulates a generic method, inferring the type parameters bases on invocation arguments.
    /// </summary>
    public class GenericMethod : IMethod
    {
        private readonly MethodInfo method;
        private readonly ParameterInfo[] parametersInfo;
        private readonly Func<MethodInfo, IMethod> factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericMethod"/> class.
        /// </summary>
        /// <param name="method">The method info.</param>
        public GenericMethod(MethodInfo method)
            : this(method, m => new StaticMethod(m))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericMethod"/> class.
        /// </summary>
        /// <param name="method">The method info.</param>
        /// <param name="owner">The owner.</param>
        public GenericMethod(MethodInfo method, object owner)
            : this(method, m => new InstanceMethod(m, owner))
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericMethod"/> class.
        /// </summary>
        /// <param name="method">The method info.</param>
        /// <param name="factory">
        /// The factory to create an <see cref="IMethod" /> from the <see cref="MethodInfo" />
        /// resolved by <see cref="GenericMethod" />.
        /// </param>
        public GenericMethod(MethodInfo method, Func<MethodInfo, IMethod> factory)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            if (factory == null)
                throw new ArgumentNullException("factory");

            this.method = method;
            this.factory = factory;
            this.parametersInfo = method.GetParameters();
        }

        /// <summary>
        /// Gets information about the parameters of the method.
        /// </summary>
        public IEnumerable<ParameterInfo> Parameters
        {
            get { return this.parametersInfo; }
        }

        /// <summary>
        /// Gets information about the method.
        /// </summary>
        public MethodInfo Method
        {
            get { return method; }
        }

        /// <summary>
        /// Gets information about the factory to create <see cref="IMethod" /> from
        /// <see cref="MethodInfo" />
        /// </summary>
        public Func<MethodInfo, IMethod> Factory
        {
            get { return factory; }
        }

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
                type.GetGenericArguments();
        }

        private static MethodInfo InferMethodInfo(MethodInfo methodInfo, IEnumerable<object> arguments)
        {
            if (methodInfo.ContainsGenericParameters)
            {
                var typeMap = arguments.Zip(methodInfo.GetParameters(),
                (argument, parameter) => ResolveGenericType(GetType(argument), parameter.ParameterType))
                .SelectMany(x => x)
                .ToLookup(x => x.Item1, x => x.Item2);

                var actuals = methodInfo.GetGenericArguments()
                    .Select(x =>
                    {
                        if (!typeMap.Contains(x))
                            throw new TypeArgumentsCannotBeInferredException(methodInfo);

                        return typeMap[x].Aggregate((t1, t2) => t1.IsAssignableFrom(t2) ? t2 : t1);
                    });

                return methodInfo.MakeGenericMethod(actuals.ToArray());
            }

            return methodInfo;
        }

        private static Type GetType(object argument)
        {
            return argument == null ? typeof(object) : argument.GetType();
        }

        /// <summary>
        /// Invokes the method with the supplied parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The result of the method call.</returns>
        public object Invoke(IEnumerable<object> parameters)
        {
            var arguments = parameters.ToArray();
            return Factory(InferMethodInfo(Method, arguments))
                .Invoke(arguments);
        }
    }
}

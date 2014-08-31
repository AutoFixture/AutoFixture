using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    /// <summary>
    /// Encapsulates a generic method
    /// </summary>
    public class GenericStaticMethod : IMethod
    {
        private readonly MethodInfo method;
        private readonly ParameterInfo[] parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericStaticMethod"/> class.
        /// </summary>
        /// <param name="method">The method info.</param>
        public GenericStaticMethod(MethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException("methodInfo");

            this.method = method;
            this.parameters = method.GetParameters();
        }

        public IEnumerable<ParameterInfo> Parameters
        {
            get { return this.parameters; }
        }

        /// <summary>
        /// Gets information about the method.
        /// </summary>
        public MethodInfo Method
        {
            get { return method; }
        }

        private static IEnumerable<Tuple<Type, Type>> ResolveGenericType(Type argument, Type parameter)
        {
            if (argument == null)
                return Enumerable.Empty<Tuple<Type, Type>>();

            if (parameter == null)
                return Enumerable.Empty<Tuple<Type, Type>>();

            if (parameter.IsGenericParameter)
                return new[] { Tuple.Create(parameter, argument) };

            if (argument.HasElementType)
                return ResolveGenericType(argument.GetElementType(), parameter.GetElementType() ?? parameter.GetGenericArguments().FirstOrDefault());

            return argument.GetGenericArguments()
                .Zip(parameter.GetGenericArguments(), ResolveGenericType)
                .SelectMany(x => x);
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
                            throw new TypeArgumentsCannotBeInferedException(methodInfo);

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

        public object Invoke(IEnumerable<object> arguments)
        {
            var args = arguments.ToArray();
            return InferMethodInfo(Method, args)
                .Invoke(null, args);
        }
    }
}

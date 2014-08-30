using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    internal class GenericStaticMethod : IMethod
    {
        private readonly MethodInfo methodInfo;
        private readonly ParameterInfo[] parameters;

        public GenericStaticMethod(MethodInfo methodInfo)
        {
            if (methodInfo == null)
                throw new ArgumentNullException("methodInfo");

            this.methodInfo = methodInfo;
            this.parameters = methodInfo.GetParameters();
        }

        public IEnumerable<ParameterInfo> Parameters
        {
            get { return this.parameters; }
        }

        public MethodInfo MethodInfo
        {
            get { return methodInfo; }
        }

        private static IEnumerable<Tuple<Type, Type>> ResolveGenericType(Type argument, Type parameter)
        {
            if (parameter.IsGenericParameter)
                return new[] { Tuple.Create(parameter, argument) };

            if (argument.HasElementType)
                return ResolveGenericType(argument.GetElementType(), parameter.GetElementType());

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
                    .Select(x => typeMap[x].Aggregate((t1, t2) => t1.IsAssignableFrom(t2) ? t2 : t1));

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
            return InferMethodInfo(MethodInfo, args)
                .Invoke(null, args);
        }
    }
}

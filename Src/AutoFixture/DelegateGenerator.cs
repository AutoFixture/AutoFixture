using System;
using System.Linq.Expressions;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates new <see cref="Delegate"/> instances.
    /// </summary>
    public class DelegateGenerator : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new <see cref="Delegate"/> instance.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// A new <see cref="Delegate"/> instance, if <paramref name="request"/> is a request for a
        /// <see cref="Delegate"/>; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            var delegateType = request as Type;

            if (delegateType == null)
            {
                return new NoSpecimen(request);
            }

            if (!typeof(Delegate).IsAssignableFrom(delegateType))
            {
                return new NoSpecimen(request);
            }

            var delegateMethod = delegateType.GetMethod("Invoke");
            var specimenParams = DelegateGenerator.CreateMethodSpecimenParameters(delegateMethod);
            var specimenBody = DelegateGenerator.CreateMethodSpecimenBody(specimenParams);
            var speciment = Expression.Lambda(delegateType, specimenBody, specimenParams).Compile();

            return speciment;
        }

        private static ParameterExpression[] CreateMethodSpecimenParameters(MethodInfo targetMethod)
        {
            int paramCount = 0;
            var parameters = Array.ConvertAll(
                targetMethod.GetParameters(),
                param => Expression.Parameter(param.ParameterType, String.Concat("arg", paramCount++)));

            return parameters;
        }

        private static Expression CreateMethodSpecimenBody(ParameterExpression[] parameters)
        {
            var paramsToObjectsConversions = Array.ConvertAll(
                parameters,
                param => Expression.Convert(param, typeof(object)));
            var newObjectArray = Expression.NewArrayInit(typeof(object), paramsToObjectsConversions);

            Action<object[]> body = args => { };
            var methodCall = Expression.Call(null, body.Method, newObjectArray);

            return methodCall;
        }
    }
}
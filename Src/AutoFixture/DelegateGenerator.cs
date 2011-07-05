using System;
using System.Linq.Expressions;
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
            var targetType = request as Type;

            if (targetType == null)
            {
                return new NoSpecimen(request);
            }

            if (!typeof(Delegate).IsAssignableFrom(targetType))
            {
                return new NoSpecimen(request);
            }

            var targetMethod = targetType.GetMethod("Invoke");

            int paramCount = 0;
            var specimenParams = Array.ConvertAll(
                targetMethod.GetParameters(),
                param => Expression.Parameter(param.ParameterType, String.Concat("arg", paramCount++)));
            var specimenBody = DelegateGenerator.CreateDynamicMethodExpression(specimenParams);
            
            return Expression.Lambda(targetType, specimenBody, specimenParams).Compile();
        }

        private static Expression CreateDynamicMethodExpression(ParameterExpression[] parameters)
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
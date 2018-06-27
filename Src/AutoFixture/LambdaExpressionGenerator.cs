using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Creates new lambda expressions represented by the <see cref="Expression{TDelegate}"/> type.
    /// </summary>
    /// <remarks>
    /// Specimens are typically either of type <see>
    ///         <cref>Expression{Func{T}}</cref>
    ///     </see>
    ///     or <see cref="Expression{Action}"/>.
    /// </remarks>
    public class LambdaExpressionGenerator : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new lambda expression represented by the <see cref="Expression{TDelegate}"/> type.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// A new <see cref="Expression{TDelegate}"/> instance, if <paramref name="request"/> is a request for a
        /// <see cref="Expression{TDelegate}"/>; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var requestType = request as Type;
            if (requestType == null)
                return new NoSpecimen();

            if (!typeof(LambdaExpression).GetTypeInfo().IsAssignableFrom(requestType))
                return new NoSpecimen();

            var delegateType = requestType == typeof(LambdaExpression)
                               ? typeof(Action)
                               : requestType.GetTypeInfo().GetGenericArguments().Single();

            var delegateSignature = delegateType.GetTypeInfo().GetMethod("Invoke");
            var parameterExpressions = delegateSignature
                .GetParameters()
                .Select(p => Expression.Parameter(p.ParameterType, p.Name))
                .ToArray();

            var body = delegateSignature.ReturnType == typeof(void)
                ? (Expression)Expression.Empty()
                : Expression.Constant(context.Resolve(delegateSignature.ReturnType));

            return Expression.Lambda(delegateType, body, parameterExpressions);
        }
    }
}

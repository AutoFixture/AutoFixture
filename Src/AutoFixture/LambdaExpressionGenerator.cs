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
            {
                return new NoSpecimen();
            }

            if (requestType.BaseType() != typeof(LambdaExpression))
            {
                return new NoSpecimen();
            }

            var delegateType = requestType.GetTypeInfo().GetGenericArguments().Single();
            var genericArguments = delegateType.GetTypeInfo().GetGenericArguments().Select(Expression.Parameter).ToList();

            if (delegateType == typeof(Action))
            {
                return Expression.Lambda(Expression.Empty());
            }

            if (delegateType.FullName.StartsWith("System.Action`", StringComparison.Ordinal))
            {
                return Expression.Lambda(Expression.Empty(), genericArguments);
            }

            var body = context.Resolve(delegateType.GetTypeInfo().GetGenericArguments().Last());
            var parameters = genericArguments.Except(new[] { genericArguments.Last() });

            return Expression.Lambda(Expression.Constant(body), parameters);
        }
    }
}
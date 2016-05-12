namespace Ploeh.AutoFixture
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Kernel;

    public class LambdaExpressionGenerator : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var requestType = request as Type;
            if (requestType == null)
            {
                return new NoSpecimen();
            }

            if (requestType.BaseType != typeof(LambdaExpression))
            {
                return new NoSpecimen();
            }

            var genericArguments = requestType
                .GetGenericArguments().Single()
                .GetGenericArguments().Select(Expression.Parameter).ToList();

            var body = genericArguments.First();
            var parameters = new List<ParameterExpression>();
            if (genericArguments.Count > 1)
            {
                parameters = genericArguments.Skip(1).ToList();
            }

            var lambdaExpression = Expression.Lambda(body, parameters);
            return lambdaExpression;
        }
    }
}
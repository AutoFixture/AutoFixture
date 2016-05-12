namespace Ploeh.AutoFixture
{
    using System;
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
              .GetGenericArguments().Single();

            var parameter = Expression.Parameter(genericArguments);
            var lambdaExpression = Expression.Lambda(parameter);
            return lambdaExpression;
        }
    }
}
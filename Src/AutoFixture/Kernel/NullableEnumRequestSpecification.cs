using System;
using System.Linq;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// A specification that evaluates whether a request is a request for a nullable enum.
    /// </summary>
    public class NullableEnumRequestSpecification : IRequestSpecification
    {
        /// <summary>
        /// Evaluates a request for a specimen.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> is a request for a nullable enum;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            return (from t in request.Maybe().OfType<Type>()
                    where t.IsGenericType
                    let gtd = t.GetGenericTypeDefinition()
                    where typeof(Nullable<>).IsAssignableFrom(gtd)
                    let ga = t.GetGenericArguments()
                    where ga.Length == 1
                    select ga.Single().IsEnum).DefaultIfEmpty(false).Single();
        }
    }
}

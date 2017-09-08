using System;
using System.Reflection;
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
            // This is performance-sensitive code when used repeatedly over many requests.
            // See discussion at https://github.com/AutoFixture/AutoFixture/pull/218
            var requestType = request as Type;
            if (requestType == null) return false;
            if (!requestType.IsGenericType()) return false;
            var gtd = requestType.GetGenericTypeDefinition();
            if (!typeof(Nullable<>).GetTypeInfo().IsAssignableFrom(gtd)) return false;
            var ga = requestType.GetTypeInfo().GetGenericArguments();
            return ga.Length == 1 && ga[0].IsEnum();
        }
    }
}

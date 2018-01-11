using System;
using System.Reflection;

namespace AutoFixture.Kernel
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
            var typeRequest = request as Type;
            if (typeRequest == null)
                return false;

            if (!typeRequest.TryGetSingleGenericTypeArgument(typeof(Nullable<>), out Type nullableType))
                return false;

            return nullableType.GetTypeInfo().IsEnum;
        }
    }
}

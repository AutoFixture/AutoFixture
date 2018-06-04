using System;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// A specification that evaluates whether a request is a request for a value type such as a structure.
    /// </summary>
    public class ValueTypeSpecification : IRequestSpecification
    {
        /// <summary>
        /// Evaluates a request for a specimen.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> is a <see cref="Type"/> that represents a structure;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            return request is Type typeRequest && IsValueTypeButNotPrimitiveOrEnum(typeRequest);
        }

        /// <summary>
        /// Checks if type is a struct. This will exclude primitive types (int, char etc.) considered as IsPrimitive as
        /// well as enums but not .NET structs.
        /// </summary>
        private static bool IsValueTypeButNotPrimitiveOrEnum(Type type)
        {
            var ti = type.GetTypeInfo();
            return ti.IsValueType && !ti.IsEnum && !ti.IsPrimitive;
        }
    }
}
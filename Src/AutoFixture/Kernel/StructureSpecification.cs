using System;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// A specification that evaluates whether a request is a request for a value type such as a custom structure.
    /// </summary>
    public class StructureSpecification : IRequestSpecification
    {

        /// <summary>
        /// Evaluates a request for a specimen.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> is a <see cref="Type"/> that represents a custom structure;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            var type = request as Type;
            return type != null && IsStruct(type);
        }

        private bool IsStruct(Type type)
        {
            return type.IsValueType && !type.IsEnum && !type.IsPrimitive;
        }
    }
}
using System;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// A specification that evaluates requests for types against a target type.
    /// Also matches generic type requests agains the specified open generic.
    /// </summary>
    public class ExactTypeSpecification : IRequestSpecification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExactTypeSpecification"/> class.
        /// </summary>
        /// <param name="type">The target type.</param>
        public ExactTypeSpecification(Type type)
        {
            this.TargetType = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Gets the type targeted by this <see cref="IRequestSpecification"/>.
        /// </summary>
        public Type TargetType { get; }

        /// <summary>
        /// Evaluates a request for a specimen.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> matches the target type;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var typeRequest = request as Type;
            if (typeRequest == null)
                return false;

            if (this.TargetType == typeRequest)
                return true;

            if (this.IsOpenGenericDefinitionEqual(typeRequest))
                return true;

            return false;
        }

        private bool IsOpenGenericDefinitionEqual(Type request)
        {
            return this.TargetType.GetTypeInfo().IsGenericTypeDefinition
                   && request.GetTypeInfo().IsGenericType
                   && request.GetTypeInfo().GetGenericTypeDefinition() == this.TargetType;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// A specification that determines whether the request is a request
    /// for a <see cref="ParameterInfo"/> matching the specified name and <see cref="Type"/>.
    /// </summary>
    public class ParameterSpecification : IRequestSpecification
    {
        private readonly IEquatable<ParameterInfo> target;
        private readonly string targetName;
        private readonly Type targetType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterSpecification"/> class.
        /// </summary>
        /// <param name="targetType">
        /// The <see cref="Type"/> with which the requested
        /// <see cref="ParameterInfo"/> type should be compatible.
        /// </param>
        /// <param name="targetName">
        /// The name which the requested <see cref="ParameterInfo"/> name
        /// should match exactly.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="targetType"/> or
        /// <paramref name="targetName"/> is <see langword="null"/>.
        /// </exception>
        public ParameterSpecification(Type targetType, string targetName)
            : this(CreateDefaultTarget(targetType, targetName))
        {
            this.targetType = targetType;
            this.targetName = targetName;
        }

        private static IEquatable<ParameterInfo> CreateDefaultTarget(
            Type targetType,
            string targetName)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));
            if (targetName == null)
                throw new ArgumentNullException(nameof(targetName));

            return new ParameterTypeAndNameCriterion(
                new Criterion<Type>(
                    targetType,
                    new DerivesFromTypeComparer()),
                new Criterion<string>(
                    targetName,
                    EqualityComparer<string>.Default));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterSpecification"/> class.
        /// </summary>
        /// <param name="target">
        /// The criteria used to match the requested
        /// <see cref="ParameterInfo"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target"/> is <see langword="null"/>.
        /// </exception>
        public ParameterSpecification(
            IEquatable<ParameterInfo> target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            this.target = target;
        }

        /// <summary>
        /// The <see cref="Type"/> with which the requested
        /// <see cref="ParameterInfo"/> type should be compatible.
        /// </summary>
        /// <remarks>
        /// This property is not a getter-only auto-property as Release 
        /// builds will generate an error due to the Obsolete attribute.
        /// </remarks>
        [Obsolete("This value is only available if the constructor taking a target type and name is used. Otherwise, it'll be null. Use with caution. This property will be removed in a future version of AutoFixture.", false)]
        public Type TargetType => targetType;

        /// <summary>
        /// The name which the requested <see cref="ParameterInfo"/> name
        /// should match exactly.
        /// </summary>
        /// <remarks>
        /// This property is not a getter-only auto-property as Release 
        /// builds will generate an error due to the Obsolete attribute.
        /// </remarks>
        [Obsolete("This value is only available if the constructor taking a target type and name is used. Otherwise, it'll be null. Use with caution. This property will be removed in a future version of AutoFixture.", false)]
        public string TargetName => targetName;

        /// <summary>
        /// Evaluates a request for a specimen.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> is satisfied by the Specification;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var p = request as ParameterInfo;
            if (p == null)
                return false;

            return this.target.Equals(p);
        }

        private class DerivesFromTypeComparer : IEqualityComparer<Type>
        {
            public bool Equals(Type x, Type y)
            {
                if (y == null && x == null)
                    return true;
                if (y == null)
                    return false;
                return y.IsAssignableFrom(x);
            }

            public int GetHashCode(Type obj)
            {
                return 0;
            }
        }
    }
}

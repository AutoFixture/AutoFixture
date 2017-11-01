using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// A specification that determines whether the request is a request
    /// for a <see cref="PropertyInfo"/> matching a particular property,
    /// according to the supplied comparison rules.
    /// </summary>
    public class PropertySpecification : IRequestSpecification
    {
        private readonly Type targetType;
        private readonly string targetName;
        private readonly IEquatable<PropertyInfo> target;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySpecification"/> class.
        /// </summary>
        /// <param name="targetType">
        /// The <see cref="Type"/> with which the requested
        /// <see cref="PropertyInfo"/> type should be compatible.
        /// </param>
        /// <param name="targetName">
        /// The name which the requested <see cref="PropertyInfo"/> name
        /// should match exactly.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="targetType"/> or
        /// <paramref name="targetName"/> is <see langword="null"/>.
        /// </exception>
        public PropertySpecification(Type targetType, string targetName)
            : this(CreateDefaultTarget(targetType, targetName))
        {
            this.targetType = targetType;
            this.targetName = targetName;
        }

        private static IEquatable<PropertyInfo> CreateDefaultTarget(
            Type targetType,
            string targetName)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));
            if (targetName == null)
                throw new ArgumentNullException(nameof(targetName));

            return new PropertyTypeAndNameCriterion(
                new Criterion<Type>(
                    targetType,
                    new DerivesFromTypeComparer()),
                new Criterion<string>(
                    targetName,
                    EqualityComparer<string>.Default));
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="PropertySpecification"/> class.
        /// </summary>
        /// <param name="target">
        /// The criterion used to match the requested
        /// <see cref="PropertyInfo"/> with the specified value.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target"/> is <see langword="null"/>.
        /// </exception>
        public PropertySpecification(IEquatable<PropertyInfo> target)
        {
            this.target = target ?? throw new ArgumentNullException(nameof(target));
        }

        /// <summary>
        /// The <see cref="Type"/> with which the requested
        /// <see cref="PropertyInfo"/> type should be compatible.
        /// </summary>
        [Obsolete("This value is only available if the constructor taking a target type and name is used. Otherwise, it'll be null. Use with caution. This property will be removed in a future version of AutoFixture.", true)]
        public Type TargetType => this.targetType;

        /// <summary>
        /// The name which the requested <see cref="PropertyInfo"/> name
        /// should match exactly.
        /// </summary>
        [Obsolete("This value is only available if the constructor taking a target type and name is used. Otherwise, it'll be null. Use with caution. This property will be removed in a future version of AutoFixture.", true)]
        public string TargetName => this.targetName;

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

            var p = request as PropertyInfo;
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
                return y.GetTypeInfo().IsAssignableFrom(x);
            }

            public int GetHashCode(Type obj)
            {
                return 0;
            }
        }
    }
}

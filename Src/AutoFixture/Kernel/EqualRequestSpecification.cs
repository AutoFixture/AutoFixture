using System;
using System.Collections;
using System.Collections.Generic;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Evaluates whether a request is equal to an expected target.
    /// </summary>
    public class EqualRequestSpecification : IRequestSpecification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EqualRequestSpecification" /> class.
        /// </summary>
        public EqualRequestSpecification(object target)
            : this(target, EqualityComparer<object>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualRequestSpecification" /> class.
        /// </summary>
        public EqualRequestSpecification(object target, IEqualityComparer comparer)
        {
            this.Target = target ?? throw new ArgumentNullException(nameof(target));
            this.Comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }

        /// <summary>
        /// Evaluates whether a request for a specimen is equal to the expected target.
        /// </summary>
        public bool IsSatisfiedBy(object request)
        {
            return this.Comparer.Equals(this.Target, request);
        }

        /// <summary>
        /// Gets the expected target.
        /// </summary>
        public object Target { get; }

        /// <summary>
        /// Gets the equality comparer used to compare a request to the expected target.
        /// </summary>
        public IEqualityComparer Comparer { get; }
    }
}

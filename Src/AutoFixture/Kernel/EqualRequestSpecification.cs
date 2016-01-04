using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Evaluates whether a request is equal to an expected target.
    /// </summary>
    public class EqualRequestSpecification : IRequestSpecification
    {
        private readonly object target;
        private readonly IEqualityComparer comparer;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="EqualRequestSpecification" /> class.
        /// </summary>
        /// <param name="target">The expected target.</param>
        /// <seealso cref="EqualRequestSpecification(object, IEqualityComparer)" />
        /// <seealso cref="Target" />
        /// <exception cref="System.ArgumentNullException">target</exception>
        public EqualRequestSpecification(object target)
            : this(target, EqualityComparer<object>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="EqualRequestSpecification" /> class.
        /// </summary>
        /// <param name="target">The expected target.</param>
        /// <param name="comparer">
        /// The comparer used to evaluate whether a request is equal to
        /// <paramref name="target" />.
        /// </param>
        /// <seealso cref="EqualRequestSpecification(object)" />
        /// <seealso cref="Target" />
        /// <exception cref="System.ArgumentNullException">target</exception>
        /// <exception cref="System.ArgumentNullException">comparer</exception>
        public EqualRequestSpecification(
            object target,
            IEqualityComparer comparer)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            this.target = target;
            this.comparer = comparer;
        }

        /// <summary>
        /// Evaluates whether a request for a specimen is equal to the expected
        /// target.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="request" /> is equal to
        /// <see cref="Target" />; otherwise, <see langword="false" />.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            return this.comparer.Equals(this.target, request);
        }

        /// <summary>
        /// Gets the expected target.
        /// </summary>
        /// <value>
        /// The expected target, originally supplied via the constructor.
        /// </value>
        /// <seealso cref="EqualRequestSpecification(object)" />
        /// <seealso cref="EqualRequestSpecification(object, IEqualityComparer)" />
        public object Target
        {
            get { return this.target; }
        }

        /// <summary>
        /// Gets the equality comparer used to compare a request to the
        /// expected target.
        /// </summary>
        /// <value>
        /// The equality comparer, optionally supplied via a constructor.
        /// </value>
        /// <seealso cref="EqualRequestSpecification(object, IEqualityComparer)" />
        public IEqualityComparer Comparer 
        {
            get { return this.comparer; }
        }
    }
}

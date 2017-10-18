using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Supplies <see cref="ISpecimenBuilder"/> instances that can be used to implement the default
    /// engine of AutoFixture.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "This is not a 'collection' - it can't be modified.")]
    public class DefaultEngineParts : DefaultRelays
    {
        private readonly IEnumerable<ISpecimenBuilder> primitiveBuilders;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultEngineParts"/> class with
        /// an instance of <see cref="DefaultPrimitiveBuilders"/>.
        /// </summary>
        public DefaultEngineParts()
            : this(new DefaultPrimitiveBuilders())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultEngineParts"/> class with the
        /// supplied sequence of <see cref="ISpecimenBuilder"/>.
        /// </summary>
        /// <param name="primitiveBuilders">
        /// The builders to use instead of <see cref="DefaultPrimitiveBuilders"/>.
        /// </param>
        public DefaultEngineParts(IEnumerable<ISpecimenBuilder> primitiveBuilders)
            : this(primitiveBuilders.ToArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultEngineParts"/> class with the
        /// supplied array of <see cref="ISpecimenBuilder"/>.
        /// </summary>
        /// <param name="primitiveBuilders">
        /// The builders to use instead of <see cref="DefaultPrimitiveBuilders"/>.
        /// </param>
        public DefaultEngineParts(params ISpecimenBuilder[] primitiveBuilders)
        {
            if (primitiveBuilders == null)
            {
                throw new ArgumentNullException(nameof(primitiveBuilders));
            }

            this.primitiveBuilders = primitiveBuilders;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}"/> that can be used to iterate
        /// through the collection.
        /// </returns>
        public override IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            var l = this.primitiveBuilders.ToList();
            var be = base.GetEnumerator();
            while (be.MoveNext())
            {
                l.Add(be.Current);
            }
            return l.GetEnumerator();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Supplies <see cref="ISpecimenBuilder"/> instances that can be used to implement the default
    /// engine of AutoFixture.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "This is not a 'collection' - it can't be modified.")]
    public class DefaultEngineBuilders : DefaultRelays
    {
        private readonly IEnumerable<ISpecimenBuilder> primitiveBuilders;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultEngineBuilders"/> class with
        /// an instance of <see cref="DefaultPrimitiveBuilders"/>.
        /// </summary>
        public DefaultEngineBuilders()
            : this(new DefaultPrimitiveBuilders())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultEngineBuilders"/> class with the
        /// supplied sequence of <see cref="ISpecimenBuilder"/>.
        /// </summary>
        /// <param name="primitiveBuilders">
        /// The builders to use instead of <see cref="DefaultPrimitiveBuilders"/>.
        /// </param>
        public DefaultEngineBuilders(IEnumerable<ISpecimenBuilder> primitiveBuilders)
            : this(primitiveBuilders.ToArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultEngineBuilders"/> class with the
        /// supplied array of <see cref="ISpecimenBuilder"/>.
        /// </summary>
        /// <param name="primitiveBuilders">
        /// The builders to use instead of <see cref="DefaultPrimitiveBuilders"/>.
        /// </param>
        public DefaultEngineBuilders(params ISpecimenBuilder[] primitiveBuilders)
        {
            if (primitiveBuilders == null)
            {
                throw new ArgumentNullException("primitiveBuilders");
            }

            this.primitiveBuilders = primitiveBuilders;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate
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

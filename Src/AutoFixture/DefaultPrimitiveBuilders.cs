using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using System.Collections;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Supplies <see cref="ISpecimenBuilder"/> instances that can resolve requests for many common
    /// primitives, such as <see cref="string"/> and <see cref="int"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "This is not a 'collection' - it can't be modified.")]
    public class DefaultPrimitiveBuilders : IEnumerable<ISpecimenBuilder>
    {
        #region IEnumerable<ISpecimenBuilder> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate
        /// through the collection.
        /// </returns>
        public virtual IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            yield return new StringGenerator(() => Guid.NewGuid());
            yield return new StringSeedRelay();
            yield return new Int32SequenceGenerator();
            yield return new DateTimeGenerator();
            yield return new DecimalSequenceGenerator();
            yield return new BooleanSwitch();
            yield return new GuidGenerator();
            yield return new Int64SequenceGenerator();
            yield return new UInt64SequenceGenerator();
            yield return new UInt32SequenceGenerator();
            yield return new Int16SequenceGenerator();
            yield return new UInt16SequenceGenerator();
            yield return new ByteSequenceGenerator();
            yield return new SByteSequenceGenerator();
            yield return new SingleSequenceGenerator();
            yield return new DoubleSequenceGenerator();
            yield return new DelegateGenerator();
            yield return new IntPtrGuard();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate
        /// through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}

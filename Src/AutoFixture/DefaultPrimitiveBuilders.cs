using System;
using System.Collections;
using System.Collections.Generic;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Supplies <see cref="ISpecimenBuilder"/> instances that can resolve requests for many common
    /// primitives, such as <see cref="string"/> and <see cref="int"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "This is not a 'collection' - it can't be modified.")]
    public class DefaultPrimitiveBuilders : IEnumerable<ISpecimenBuilder>
    {
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
            yield return new ConstrainedStringGenerator();
            yield return new StringSeedRelay();
            yield return new RandomNumericSequenceGenerator();
            yield return new RandomCharSequenceGenerator();
            yield return new UriGenerator();
            yield return new UriSchemeGenerator();
            yield return new RangedNumberGenerator();
            yield return new RegularExpressionGenerator();
            yield return new RandomDateTimeSequenceGenerator();
            yield return new BooleanSwitch();
            yield return new GuidGenerator();
            yield return new TypeGenerator();
            yield return new DelegateGenerator();
            yield return new TaskGenerator();
            yield return new IntPtrGuard();
#if SYSTEM_NET_MAIL
            yield return new MailAddressGenerator();
#endif
            yield return new EmailAddressLocalPartGenerator();
            yield return new DomainNameGenerator();
        }

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
    }
}

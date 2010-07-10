using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using System.Collections;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Supplies <see cref="ISpecimenBuilder"/> instances that works as AutoFixture's
    /// 'transmission'. Without those builders, very little is likely to work.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "This is not a 'collection' - it can't be modified.")]
    public class DefaultRelays : IEnumerable<ISpecimenBuilder>, IMultiple
    {
        private readonly MultipleRelay multiple;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRelays"/> class.
        /// </summary>
        public DefaultRelays()
        {
            this.multiple = new MultipleRelay();
        }

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
            yield return new ConstructorInvoker(new ModestConstructorQuery());
            yield return new ParameterRequestRelay();
            yield return new PropertyRequestRelay();
            yield return new FieldRequestRelay();
            yield return this.multiple;
            yield return new FiniteSequenceRelay();
            yield return new SeedIgnoringRelay();
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

        #region IMultiple Members

        /// <summary>
        /// Gets or sets the count that specifies how many <i>Many</i> is.
        /// </summary>
        public int Count
        {
            get { return this.multiple.Count; }
            set { this.multiple.Count = value; }
        }

        #endregion
    }
}

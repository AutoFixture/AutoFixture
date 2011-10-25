using System.Collections;
using System.Collections.Generic;
using Ploeh.AutoFixture.DataAnnotations;
using Ploeh.AutoFixture.Kernel;

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

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate
        /// through the collection.
        /// </returns>
        public virtual IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            yield return new ArrayRelay();
            yield return new MethodInvoker(
                new CompositeMethodQuery(
                    new ModestConstructorQuery(), 
                    new FactoryMethodQuery()));
            yield return new ParameterRequestRelay();
            yield return new RangeAttributeRelay();
            yield return new StringLengthAttributeRelay();
            yield return new PropertyRequestRelay();            
            yield return new FieldRequestRelay();
            yield return new FiniteSequenceRelay();
            yield return new SeedIgnoringRelay();
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

        /// <summary>
        /// Gets or sets the count that specifies how many <i>Many</i> is.
        /// </summary>
        public int Count
        {
            get { return this.multiple.Count; }
            set { this.multiple.Count = value; }
        }
    }
}

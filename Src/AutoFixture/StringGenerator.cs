using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates string values based on a supplied factory.
    /// </summary>
    public class StringGenerator : ISpecimenBuilder
    {
        private readonly Func<object> createSpecimen;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringGenerator"/> class with the supplied
        /// specimen factory.
        /// </summary>
        /// <param name="specimenFactory">
        /// A factory that creates a anonymous variables (specimens). The
        /// <see cref="StringGenerator"/> instance will call <see cref="object.ToString()"/> on the
        /// created specimen.
        /// </param>
        public StringGenerator(Func<object> specimenFactory)
        {
            if (specimenFactory == null)
            {
                throw new ArgumentNullException("specimenFactory");
            }

            this.createSpecimen = specimenFactory;
        }

        /// <summary>
        /// Gets the factory used to specimens.
        /// </summary>
        /// <seealso cref="StringGenerator(Func{object})"/>
        public Func<object> Factory
        {
            get { return this.createSpecimen; }
        }

        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates string specimens by invoking the supplied specimen factory and calling
        /// <see cref="object.ToString()"/> on the result.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="container">Not used.</param>
        /// <returns>
        /// The string representation of a specimen created by the contained specimen factory.
        /// </returns>
        public object Create(object request, ISpecimenContext container)
        {
            if (request != typeof(string))
            {
                return new NoSpecimen(request);
            }

            var specimen = this.createSpecimen();
            if (specimen == null)
            {
                return new NoSpecimen(request);
            }
            if (specimen is NoSpecimen)
            {
                return specimen;
            }
            return specimen.ToString();
        }

        #endregion
    }
}

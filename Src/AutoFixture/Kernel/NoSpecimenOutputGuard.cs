using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Prevents a decorated <see cref="ISpecimenBuilder"/> from returning a
    /// <see cref="NoSpecimen"/> instance.
    /// </summary>
    public class NoSpecimenOutputGuard : ISpecimenBuilder
    {
        private readonly ISpecimenBuilder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="NoSpecimenOutputGuard"/> class with an 
        /// <see cref="ISpecimenBuilder"/> to decorate.
        /// </summary>
        /// <param name="builder">The builder to decorate.</param>
        public NoSpecimenOutputGuard(ISpecimenBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            this.builder = builder;
        }

        /// <summary>
        /// Gets the decorated builder.
        /// </summary>
        /// <value>The <see cref="ISpecimenBuilder"/> supplied via the constructor.</value>
        /// <seealso cref="NoSpecimenOutputGuard"/>
        public ISpecimenBuilder Builder
        {
            get { return this.builder; }
        }

        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates a new specimen by delegating to the decorated <see cref="Builder"/>.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise an exception is thrown.
        /// </returns>
        /// <exception cref="ObjectCreationException">
        /// The decorated <see cref="Builder"/> returned a <see cref="NoSpecimen"/> result.
        /// </exception>
        public object Create(object request, ISpecimenContext context)
        {
            var result = this.Builder.Create(request, context);
            if (result is NoSpecimen)
            {
                throw new ObjectCreationException(string.Format(CultureInfo.CurrentCulture, "The decorated ISpecimenBuilder could not create a specimen based on the request: {0}. This can happen if the request represents an interface or abstract class; if this is the case, register an ISpecimenBuilder that can create specimens based on the request. If this happens in a strongly typed Build<T> expression, try supplying a factory using one of the IFactoryComposer<T> methods.", request));
            }
            return result;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Linq.Expressions;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Performs post-processing on a created specimen.
    /// </summary>
    public class Postprocessor : ISpecimenBuilder
    {
        private readonly ISpecimenBuilder builder;
        private readonly Action<object> action;

        /// <summary>
        /// Initializes a new instance of the <see cref="Postprocessor"/> class with the supplied
        /// parameters.
        /// </summary>
        /// <param name="builder">The <see cref="ISpecimenBuilder"/> to decorate.</param>
        /// <param name="action">The action to perform on the created specimen.</param>
        public Postprocessor(ISpecimenBuilder builder, Action<object> action)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            this.builder = builder;
            this.action = action;
        }

        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates a new specimen based on a request and performs an action on the created
        /// specimen.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="container">A container that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <paramref name="request"/> can be any object, but will often be a
        /// <see cref="Type"/> or other <see cref="System.Reflection.MemberInfo"/> instances.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContainer container)
        {
            var specimen = this.builder.Create(request, container);
            this.action(specimen);
            return specimen;
        }

        #endregion
    }

    /// <summary>
    /// Performs post-processing on a created specimen.
    /// </summary>
    /// <typeparam name="T">The type of specimen.</typeparam>
    public class Postprocessor<T> : ISpecimenBuilder
    {
        private readonly ISpecimenBuilder builder;
        private readonly Action<T> action;

        /// <summary>
        /// Initializes a new instance of the <see cref="Postprocessor"/> class with the supplied
        /// parameters.
        /// </summary>
        /// <param name="builder">The <see cref="ISpecimenBuilder"/> to decorate.</param>
        /// <param name="action">The action to perform on the created specimen.</param>
        public Postprocessor(ISpecimenBuilder builder, Action<T> action)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            this.builder = builder;
            this.action = action;
        }

        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates a new specimen based on a request and performs an action on the created
        /// specimen.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="container">A container that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <paramref name="request"/> can be any object, but will often be a
        /// <see cref="Type"/> or other <see cref="System.Reflection.MemberInfo"/> instances.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContainer container)
        {
            var specimen = this.builder.Create(request, container);
            if (!(specimen is T))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    "The specimen returned by the decorated ISpecimenBuilder is not compatible with {0}.", typeof(T)));
            }
            var item = (T)specimen;
            this.action(item);
            return specimen;
        }

        #endregion
    }
}

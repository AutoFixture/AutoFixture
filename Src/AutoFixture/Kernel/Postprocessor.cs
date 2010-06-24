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
    public class Postprocessor : Postprocessor<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Postprocessor"/> class with the supplied
        /// parameters.
        /// </summary>
        /// <param name="builder">The <see cref="ISpecimenBuilder"/> to decorate.</param>
        /// <param name="action">The action to perform on the created specimen.</param>
        public Postprocessor(ISpecimenBuilder builder, Action<object> action)
            : base(builder, action)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Postprocessor"/> class with the supplied
        /// parameters.
        /// </summary>
        /// <param name="builder">The <see cref="ISpecimenBuilder"/> to decorate.</param>
        /// <param name="action">The action to perform on the created specimen.</param>
        public Postprocessor(ISpecimenBuilder builder, Action<object, ISpecimenContext> action)
            : base(builder, action)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Postprocessor"/> class with the supplied
        /// parameters.
        /// </summary>
        /// <param name="builder">The <see cref="ISpecimenBuilder"/> to decorate.</param>
        /// <param name="action">The action to perform on the created specimen.</param>
        /// <param name="specification">
        /// A specification which is used to determine whether postprocessing should be performed
        /// for a request.
        /// </param>
        public Postprocessor(ISpecimenBuilder builder, Action<object, ISpecimenContext> action, IRequestSpecification specification)
            : base(builder, action, specification)
        {
        }
    }

    /// <summary>
    /// Performs post-processing on a created specimen.
    /// </summary>
    /// <typeparam name="T">The type of specimen.</typeparam>
    public class Postprocessor<T> : ISpecimenBuilder
    {
        private readonly ISpecimenBuilder builder;
        private readonly Action<T, ISpecimenContext> action;
        private readonly IRequestSpecification specification;

        /// <summary>
        /// Initializes a new instance of the <see cref="Postprocessor{T}"/> class with the
        /// supplied parameters.
        /// </summary>
        /// <param name="builder">The <see cref="ISpecimenBuilder"/> to decorate.</param>
        /// <param name="action">The action to perform on the created specimen.</param>
        public Postprocessor(ISpecimenBuilder builder, Action<T> action)
            : this(builder, action == null ? (Action<T, ISpecimenContext>)null : (s, c) => action(s))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Postprocessor{T}"/> class with the
        /// supplied parameters.
        /// </summary>
        /// <param name="builder">The <see cref="ISpecimenBuilder"/> to decorate.</param>
        /// <param name="action">The action to perform on the created specimen.</param>
        public Postprocessor(ISpecimenBuilder builder, Action<T, ISpecimenContext> action)
            : this(builder, action, new TrueRequestSpecification())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Postprocessor{T}"/> class with the
        /// supplied parameters.
        /// </summary>
        /// <param name="builder">The <see cref="ISpecimenBuilder"/> to decorate.</param>
        /// <param name="action">The action to perform on the created specimen.</param>
        /// <param name="specification">
        /// A specification which is used to determine whether postprocessing should be performed
        /// for a request.
        /// </param>
        public Postprocessor(ISpecimenBuilder builder, Action<T, ISpecimenContext> action, IRequestSpecification specification)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            if (specification == null)
            {
                throw new ArgumentNullException("specification");
            }

            this.builder = builder;
            this.action = action;
            this.specification = specification;
        }

        /// <summary>
        /// Gets the action to perform on created specimens.
        /// </summary>
        public Action<T, ISpecimenContext> Action
        {
            get { return this.action; }
        }

        /// <summary>
        /// Gets the decorated builder.
        /// </summary>
        public ISpecimenBuilder Builder
        {
            get { return this.builder; }
        }

        /// <summary>
        /// Gets the filter that determines whether <see cref="Action"/> should be executed.
        /// </summary>
        public IRequestSpecification Specification
        {
            get { return this.specification; }
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
        public object Create(object request, ISpecimenContext container)
        {
            var specimen = this.builder.Create(request, container);

            var ns = specimen as NoSpecimen;
            if (ns != null)
            {
                return ns;
            }

            if (!this.specification.IsSatisfiedBy(request))
            {
                return specimen;
            }

            if (!(specimen is T))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    "The specimen returned by the decorated ISpecimenBuilder is not compatible with {0}.", typeof(T)));
            }
            var s = (T)specimen;
            this.action(s, container);
            return specimen;
        }

        #endregion
    }
}

using System;
using System.Globalization;
using System.Collections.Generic;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Performs post-processing on a created specimen.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "The main responsibility of this class isn't to be a 'collection' (which, by the way, it isn't - it's just an Iterator).")]
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

        /// <summary>Composes the supplied builders.</summary>
        /// <param name="builders">The builders to compose.</param>
        /// <returns>
        /// A new <see cref="ISpecimenBuilderNode" /> instance containing
        /// <paramref name="builders" /> as child nodes.
        /// </returns>
        public override ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            var composedBuilder = CompositeSpecimenBuilder.ComposeIfMultiple(builders);
            return new Postprocessor(composedBuilder, this.Action, this.Specification);
        }
    }

    /// <summary>
    /// Performs post-processing on a created specimen.
    /// </summary>
    /// <typeparam name="T">The type of specimen.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "The main responsibility of this class isn't to be a 'collection' (which, by the way, it isn't - it's just an Iterator).")]
    public class Postprocessor<T> : ISpecimenBuilderNode
    {
        private readonly ISpecimenBuilder builder;
        private readonly Action<T, ISpecimenContext> action;
        private readonly ISpecimenCommand command;
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

        public Postprocessor(ISpecimenBuilder builder, ISpecimenCommand command)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");
            if (command == null)
                throw new ArgumentNullException("command");
            
            this.builder = builder;
            this.command = command;
            this.action = (s, c) => this.command.Execute(s, c);
            this.specification = new TrueRequestSpecification();
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
            this.command = new ActionSpecimenCommandAdapter(this.action);
            this.specification = specification;
        }

        public Postprocessor(
            ISpecimenBuilder builder,
            ISpecimenCommand command,
            IRequestSpecification specification)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");

            this.builder = builder;
            this.command = command;
            this.specification = specification;
            this.action = (s, c) => this.command.Execute(s, c);
        }

        /// <summary>
        /// Gets the action to perform on created specimens.
        /// </summary>
        public Action<T, ISpecimenContext> Action
        {
            get { return this.action; }
        }

        public ISpecimenCommand Command
        {
            get { return this.command; }
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

        /// <summary>
        /// Creates a new specimen based on a request and performs an action on the created
        /// specimen.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <paramref name="request"/> can be any object, but will often be a
        /// <see cref="Type"/> or other <see cref="System.Reflection.MemberInfo"/> instances.
        /// </para>
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ISpecimenBuilder", Justification = "Workaround for a bug in CA: https://connect.microsoft.com/VisualStudio/feedback/details/521030/")]
        public object Create(object request, ISpecimenContext context)
        {
            var specimen = this.builder.Create(request, context);

            var ns = specimen as NoSpecimen;
            if (ns != null)
                return ns;

            if (!this.specification.IsSatisfiedBy(request))
                return specimen;

            if (!(specimen is T))
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    "The specimen returned by the decorated ISpecimenBuilder is not compatible with {0}.", typeof(T)));
            this.command.Execute(specimen, context);
            return specimen;
        }

        /// <summary>Composes the supplied builders.</summary>
        /// <param name="builders">The builders to compose.</param>
        /// <returns>
        /// A new <see cref="ISpecimenBuilderNode" /> instance containing
        /// <paramref name="builders" /> as child nodes.
        /// </returns>
        public virtual ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            var composedBuilder = CompositeSpecimenBuilder.ComposeIfMultiple(builders);
            return new Postprocessor<T>(composedBuilder, this.action, this.specification);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{ISpecimenBuilder}" /> that can be used to
        /// iterate through the collection.
        /// </returns>
        public IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            yield return this.builder;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private class ActionSpecimenCommandAdapter : ISpecimenCommand
        {
            private readonly Action<T, ISpecimenContext> action;

            public ActionSpecimenCommandAdapter(Action<T, ISpecimenContext> action)
            {
                this.action = action;
            }

            public void Execute(object specimen, ISpecimenContext context)
            {
                var s = (T)specimen;
                this.action(s, context);
            }
        }
    }
}

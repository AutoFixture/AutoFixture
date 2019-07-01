using System;
using System.Collections.Generic;
using System.Globalization;

namespace AutoFixture.Kernel
{
#pragma warning disable SA1402 // File may only contain a single type
    /// <summary>
    /// Performs post-processing on a created specimen.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
        Justification = "The main responsibility of this class isn't to be a 'collection' (which, by the way, it isn't - it's just an Iterator).")]
#pragma warning disable 618
    public class Postprocessor : Postprocessor<object>
#pragma warning restore 618
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Postprocessor"/> class with the supplied
        /// parameters.
        /// </summary>
        /// <param name="builder">The <see cref="ISpecimenBuilder"/> to decorate.</param>
        /// <param name="action">The action to perform on the created specimen.</param>
        [Obsolete("Use Postprocessor(ISpecimenBuilder, ISpecimenCommand) instead", true)]
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
        [Obsolete("Use Postprocessor(ISpecimenBuilder, ISpecimenCommand) instead", true)]
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
        [Obsolete("Use Postprocessor(ISpecimenBuilder, ISpecimenCommand, IRequestSpecification) instead", true)]
        public Postprocessor(ISpecimenBuilder builder, Action<object, ISpecimenContext> action, IRequestSpecification specification)
            : base(builder, action, specification)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Postprocessor" />
        /// class.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="ISpecimenBuilder"/> to decorate.
        /// </param>
        /// <param name="command">
        /// The command to apply to the created specimen.
        /// </param>
        public Postprocessor(ISpecimenBuilder builder, ISpecimenCommand command)
            : base(builder, command)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Postprocessor" />
        /// class.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="ISpecimenBuilder"/> to decorate.
        /// </param>
        /// <param name="command">
        /// The command to apply to the created specimen.
        /// </param>
        /// A specification which is used to determine whether postprocessing
        /// should be performed
        /// <param name="specification">
        /// </param>
        public Postprocessor(
            ISpecimenBuilder builder,
            ISpecimenCommand command,
            IRequestSpecification specification)
            : base(builder, command, specification)
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
            if (builders == null) throw new ArgumentNullException(nameof(builders));

            var composedBuilder = CompositeSpecimenBuilder.ComposeIfMultiple(builders);
            var pp = new Postprocessor(composedBuilder, this.Command, this.Specification);
#pragma warning disable 618
            ObsoletedMemberShims.Postprocessor_SetAction(pp, ObsoletedMemberShims.Postprocessor_GetAction(this));
#pragma warning restore 618
            return pp;
        }
    }

    /// <summary>
    /// Performs post-processing on a created specimen.
    /// </summary>
    /// <typeparam name="T">The type of specimen.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "The main responsibility of this class isn't to be a 'collection' (which, by the way, it isn't - it's just an Iterator).")]
    [Obsolete("The generic version of the Postprocessor is no longer used and will be removed in future versions. Please use the non-generic version of the Postprocessor type.")]
    public class Postprocessor<T> : ISpecimenBuilderNode
    {
        private Action<T, ISpecimenContext> action;

        /// <summary>
        /// Initializes a new instance of the <see cref="Postprocessor{T}"/> class with the
        /// supplied parameters.
        /// </summary>
        /// <param name="builder">The <see cref="ISpecimenBuilder"/> to decorate.</param>
        /// <param name="action">The action to perform on the created specimen.</param>
        [Obsolete("Use Postprocessor(ISpecimenBuilder, ISpecimenCommand) instead", true)]
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
        [Obsolete("Use Postprocessor(ISpecimenBuilder, ISpecimenCommand) instead", true)]
        public Postprocessor(ISpecimenBuilder builder, Action<T, ISpecimenContext> action)
            : this(builder, action, new TrueRequestSpecification())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Postprocessor{T}" />
        /// class.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="ISpecimenBuilder"/> to decorate.
        /// </param>
        /// <param name="command">
        /// The command to apply to the created specimen.
        /// </param>
        public Postprocessor(ISpecimenBuilder builder, ISpecimenCommand command)
            : this(builder, command, new TrueRequestSpecification())
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
        [Obsolete("Use Postprocessor(ISpecimenBuilder, ISpecimenCommand, IRequestSpecification) instead", true)]
        public Postprocessor(ISpecimenBuilder builder, Action<T, ISpecimenContext> action, IRequestSpecification specification)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            if (specification == null)
            {
                throw new ArgumentNullException(nameof(specification));
            }

            this.Builder = builder;
            this.action = action;
            this.Command = new ActionSpecimenCommand<T>(this.action);
            this.Specification = specification;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Postprocessor{T}" />
        /// class.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="ISpecimenBuilder"/> to decorate.
        /// </param>
        /// <param name="command">
        /// The command to apply to the created specimen.
        /// </param>
        /// <param name="specification">
        /// A specification which is used to determine whether postprocessing
        /// should be performed for a request.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// builder, command, or specification is null.
        /// </exception>
        public Postprocessor(
            ISpecimenBuilder builder,
            ISpecimenCommand command,
            IRequestSpecification specification)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            if (specification == null)
                throw new ArgumentNullException(nameof(specification));

            this.Builder = builder;
            this.Command = command;
            this.Specification = specification;
            this.action = (s, c) => this.Command.Execute(s, c);
        }

        /// <summary>
        /// Gets the action to perform on created specimens.
        /// </summary>
        [Obsolete("Use the Command property instead.", true)]
        public Action<T, ISpecimenContext> Action
        {
            get => this.action;
            internal set => this.action = value;
        }

        /// <summary>
        /// Gets the command, which is applied during postprocessing.
        /// </summary>
        /// <value>The command supplied via one of the constructors.</value>
        public ISpecimenCommand Command { get; }

        /// <summary>
        /// Gets the decorated builder.
        /// </summary>
        public ISpecimenBuilder Builder { get; }

        /// <summary>
        /// Gets the filter that determines whether <see cref="Command"/> should be executed.
        /// </summary>
        public IRequestSpecification Specification { get; }

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
        public object Create(object request, ISpecimenContext context)
        {
            var specimen = this.Builder.Create(request, context);
            if (specimen == null)
                return specimen;

            var ns = specimen as NoSpecimen;
            if (ns != null)
                return ns;

            if (!this.Specification.IsSatisfiedBy(request))
                return specimen;

            if (!(specimen is T))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    "The specimen returned by the decorated ISpecimenBuilder is not compatible with {0}.", typeof(T)));
            }

            this.Command.Execute(specimen, context);
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
            if (builders == null) throw new ArgumentNullException(nameof(builders));

            var composedBuilder = CompositeSpecimenBuilder.ComposeIfMultiple(builders);
            var pp = new Postprocessor<T>(composedBuilder, this.Command, this.Specification);
            pp.action = this.action;
            return pp;
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
            yield return this.Builder;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
#pragma warning restore SA1402 // File may only contain a single type
}

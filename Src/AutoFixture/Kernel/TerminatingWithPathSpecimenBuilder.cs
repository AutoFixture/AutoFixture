using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Decorates an <see cref="ISpecimenBuilder"/> with a node which tracks specimen requests, and
    /// when <see cref="NoSpecimen"/> is detected, throws an <see cref="ObjectCreationException"/>, which
    /// includes a description of the request path.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
        Justification = "The main responsibility of this class isn't to be a 'collection' (which, by the way, it isn't - it's just an Iterator).")]
    public class TerminatingWithPathSpecimenBuilder : ISpecimenBuilderNode
    {
        private readonly TracingBuilder tracer;

        private readonly ConcurrentDictionary<Thread, RequestPath>
            _requestPathsByThread = new ConcurrentDictionary<Thread, RequestPath>();

        private RequestPath GetPathForCurrentThread()
        {
            return _requestPathsByThread.GetOrAdd(
                Thread.CurrentThread, _ => new RequestPath(EqualityComparer<object>.Default));
        }

        /// <summary>
        /// Creates a new <see cref="TerminatingWithPathSpecimenBuilder"/> using the
        /// supplied <see cref="TracingBuilder"/> to track specimen requests.
        /// </summary>
        /// <param name="tracer"></param>
        public TerminatingWithPathSpecimenBuilder(TracingBuilder tracer)
        {
            if (tracer == null)
                throw new ArgumentNullException("tracer");

            this.tracer = tracer;
            this.tracer.SpecimenRequested += OnSpecimenRequested;
            this.tracer.SpecimenCreated += OnSpecimenCreated;
        }

        private void OnSpecimenCreated(object sender, SpecimenCreatedEventArgs e)
        {
            GetPathForCurrentThread().Pop();
        }

        private void OnSpecimenRequested(object sender, RequestTraceEventArgs e)
        {
            GetPathForCurrentThread().Push(e.Request);
        }

        /// <summary>
        /// Gets the <see cref="TracingBuilder"/> decorated by this instance.
        /// </summary>
        public TracingBuilder Tracer
        {
            get { return this.tracer; }
        }

        /// <summary>
        /// Gets the observed specimen requests, in the order they were requested.
        /// </summary>
        public IEnumerable<object> SpecimenRequests
        {
            get { return this.GetPathForCurrentThread().Reverse(); }
        }

        /// <summary>
        /// Creates a new specimen based on a request by delegating to its decorated builder.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "AutoFixture", Justification = "Workaround for a bug in CA: https://connect.microsoft.com/VisualStudio/feedback/details/521030/")]
        public object Create(object request, ISpecimenContext context)
        {
            var result = this.tracer.Create(request, context);
            if (result is NoSpecimen)
            {
                throw new ObjectCreationException(string.Format(
                    CultureInfo.CurrentCulture,
                    "AutoFixture was unable to create an instance from {0}, most likely because it has no " +
                        "public constructor, is an abstract or non-public type.{1}{1}Request path:{1}{2}",
                    request,
                    Environment.NewLine,
                    GetPathForCurrentThread().ToRequestPathText(request)));
            }
            return result;
        }

        internal static ISpecimenBuilder ComposeIfMultiple(IEnumerable<ISpecimenBuilder> builders)
        {
            var isSingle = builders.Take(2).Count() == 1;
            if (isSingle)
                return builders.Single();

            return new CompositeSpecimenBuilder(builders);
        }

        /// <summary>Composes the supplied builders.</summary>
        /// <param name="builders">The builders to compose.</param>
        /// <returns>
        /// A new <see cref="ISpecimenBuilderNode" /> instance containing
        /// <paramref name="builders" /> as child nodes.
        /// </returns>
        public virtual ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            var builder = ComposeIfMultiple(builders);
            return new TerminatingWithPathSpecimenBuilder(new TracingBuilder(builder));
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
            yield return this.tracer.Builder;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
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
        private readonly ConcurrentDictionary<Thread, Stack<object>>
            _requestPathsByThread = new ConcurrentDictionary<Thread, Stack<object>>();

        private Stack<object> GetPathForCurrentThread()
        {
            return _requestPathsByThread.GetOrAdd(Thread.CurrentThread, _ => new Stack<object>());
        }

        /// <summary>
        /// Creates a new <see cref="TerminatingWithPathSpecimenBuilder"/> using the
        /// supplied <see cref="TracingBuilder"/> to track specimen requests.
        /// </summary>
        /// <param name="tracer"></param>
        public TerminatingWithPathSpecimenBuilder(TracingBuilder tracer)
        {
            if (tracer == null)
                throw new ArgumentNullException(nameof(tracer));

            this.Tracer = tracer;
            this.Tracer.SpecimenRequested += OnSpecimenRequested;
            this.Tracer.SpecimenCreated += OnSpecimenCreated;
        }

        private void OnSpecimenCreated(object sender, SpecimenCreatedEventArgs e)
        {
            // Keep the final NoSpecimen in the list, even though we're about to throw
            if (!(e.Specimen is NoSpecimen))
                GetPathForCurrentThread().Pop();
        }

        private void OnSpecimenRequested(object sender, RequestTraceEventArgs e)
        {
            GetPathForCurrentThread().Push(e.Request);
        }

        /// <summary>
        /// Gets the <see cref="TracingBuilder"/> decorated by this instance.
        /// </summary>
        public TracingBuilder Tracer { get; }

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
            var result = this.Tracer.Create(request, context);
            if (result is NoSpecimen)
            {
                try
                {
                    throw new ObjectCreationException(string.Format(
                        CultureInfo.CurrentCulture,
                        BuildCoreMessageTemplate(request),
                        request,
                        Environment.NewLine,
                        BuildRequestPathText(this.SpecimenRequests)));
                }
                finally
                {
                    this.GetPathForCurrentThread().Clear();
                }
            }
            return result;
        }

        private static string BuildCoreMessageTemplate(object request)
        {
            var t = request as Type;

            if (t != null && t.IsInterface())
                return
                    "AutoFixture was unable to create an instance from {0} " +
                    "because it's an interface. There's no single, most " +
                    "appropriate way to create an object implementing the " +
                    "interface, but you can help AutoFixture figure it out." +
                    "{1}" +
                    "{1}" +
                    "If you have a concrete class implementing the " +
                    "interface, you can map the interface to that class:" +
                    typeMappingOptionsHelp +
                    "{1}" +
                    "{1}" +
                    "Request path:{1}{2}{1}";

            if (t != null && t.IsAbstract())
                return
                    "AutoFixture was unable to create an instance from {0} " +
                    "because it's an abstract class. There's no single, " +
                    "most appropriate way to create an object deriving from " +
                    "that class, but you can help AutoFixture figure it out." +
                    "{1}" +
                    "{1}" +
                    "If you have a concrete class deriving from the abstract " +
                    "class, you can map the abstract class to that derived " + 
                    "class:" +
                    typeMappingOptionsHelp +
                    "{1}" +
                    "{1}" +
                    "Request path:{1}{2}{1}";

            return
                "AutoFixture was unable to create an instance from {0}, " +
                "most likely because it has no public constructor, is an " +
                "abstract or non-public type.{1}{1}Request path:{1}{2}";
        }

        private const string typeMappingOptionsHelp =
            "{1}" +
            "{1}" +
            "fixture.Customizations.Add({1}" +
            "    new TypeRelay({1}" +
            "        typeof({0}),{1}" +
            "        typeof(YourConcreteImplementation)));" +
            "{1}" +
            "{1}" +
            "Alternatively, you can turn AutoFixture into an Auto-Mocking " +
            "Container using your favourite dynamic mocking library, such " +
            "as Moq, FakeItEasy, NSubstitute, and others. As an example, to " +
            "use Moq, you can customize AutoFixture like this:" +
            "{1}" +
            "{1}" +
            "fixture.Customize(new AutoMoqCustomization());" +
            "{1}" +
            "{1}" +
            "See http://blog.ploeh.dk/2010/08/19/AutoFixtureasanauto-mockingcontainer " +
            "for more details.";

        private static string BuildRequestPathText(IEnumerable<object> recordedRequests)
        {
            var thisAssembly = typeof(TerminatingWithPathSpecimenBuilder).Assembly();
            return recordedRequests
                .Where(r => r.GetType().Assembly() != thisAssembly)
                .Select((r, i) => string.Format(CultureInfo.CurrentCulture, "\t{0} {1}", " ".PadLeft(i+1), r))
                .Aggregate((s1, s2) => s1 + " --> " + Environment.NewLine + s2);
        }

        /// <summary>Composes the supplied builders.</summary>
        /// <param name="builders">The builders to compose.</param>
        /// <returns>
        /// A new <see cref="ISpecimenBuilderNode" /> instance containing
        /// <paramref name="builders" /> as child nodes.
        /// </returns>
        public virtual ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            var builder = CompositeSpecimenBuilder.ComposeIfMultiple(builders);
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
            yield return this.Tracer.Builder;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

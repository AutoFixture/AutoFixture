using System;
using System.Collections.Generic;
using System.IO;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Trace writer that will write out a trace of object requests and created objects
    /// in the <see cref="ISpecimenBuilder" /> pipeline.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "The main responsibility of this class isn't to be a 'collection' (which, by the way, it isn't - it's just an Iterator).")]
    public class TraceWriter : ISpecimenBuilderNode
    {
        private readonly TextWriter writer;
        private Action<TextWriter, object, int> writeRequest;
        private Action<TextWriter, object, int> writeSpecimen;

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceWriter"/> class.
        /// </summary>
        /// <param name="writer">The output stream for the trace.</param>
        /// <param name="tracer">The <see cref="ISpecimenBuilder"/> to decorate.</param>
        public TraceWriter(TextWriter writer, TracingBuilder tracer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            if (tracer == null) throw new ArgumentNullException(nameof(tracer));

            this.Tracer = tracer;
            this.Tracer.SpecimenRequested += (sender, e) => this.writeRequest(writer, e.Request, e.Depth);
            this.Tracer.SpecimenCreated += (sender, e) => this.writeSpecimen(writer, e.Specimen, e.Depth);

            this.writer = writer;
            this.TraceRequestFormatter = (tw, r, i) => tw.WriteLine(new string(' ', i * 2) + "Requested: " + r);
            this.TraceSpecimenFormatter = (tw, r, i) => tw.WriteLine(new string(' ', i * 2) + "Created: " + r);
        }

        /// <summary>
        /// Gets the <see cref="TracingBuilder"/> decorated by this instance.
        /// </summary>
        public TracingBuilder Tracer { get; }

        /// <summary>
        /// Gets or sets the formatter for tracing a request.
        /// </summary>
        /// <value>The request trace formatter.</value>
        public Action<TextWriter, object, int> TraceRequestFormatter
        {
            get => this.writeRequest;
            set => this.writeRequest = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets the formatter for tracing a created specimen.
        /// </summary>
        /// <value>The created specimen trace formatter.</value>
        public Action<TextWriter, object, int> TraceSpecimenFormatter
        {
            get => this.writeSpecimen;
            set => this.writeSpecimen = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Creates a new specimen based on a request by delegating to its decorated builder.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            return this.Tracer.Create(request, context);
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

            var builder = CompositeSpecimenBuilder.ComposeIfMultiple(builders);

            return new TraceWriter(
                this.writer,
                new TracingBuilder(
                    builder));
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
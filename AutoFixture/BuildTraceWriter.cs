namespace Ploeh.AutoFixture
{
    using System;
    using System.IO;
    using Kernel;

    /// <summary>
    /// Trace writer that will write out a trace of object requests and created objects
    /// in the <see cref="ISpecimenBuilder" /> pipeline.
    /// </summary>
    public class BuildTraceWriter : RequestTracker
    {
        private TextWriter writer;
        private int indentLevel = 0;

        /// <summary>
        /// Gets or sets the formatter for tracing a request.
        /// </summary>
        /// <value>The request trace formatter.</value>
#warning Must handle null input
        public Action<TextWriter, object, int> TraceRequestFormatter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the formatter for tracing a created specimen.
        /// </summary>
        /// <value>The created specimen trace formatter.</value>
#warning Must handle null input
        public Action<TextWriter, object, int> TraceCreatedSpecimenFormatter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the current indent level of the trace.
        /// </summary>
        /// <value>The indent level.</value>
        public int IndentLevel
        {
            get { return this.indentLevel; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildTraceWriter"/> class.
        /// </summary>
        /// <param name="outStream">The output stream for the trace.</param>
        /// <param name="builder">The <see cref="ISpecimenBuilder"/> to decorate.</param>
        public BuildTraceWriter(TextWriter writer, ISpecimenBuilder builder)
            : base(builder)
        {
            this.writer = writer;
            this.TraceRequestFormatter = (tw, r, i) => tw.WriteLine(new string(' ', i * 2) + r.ToString());
            this.TraceCreatedSpecimenFormatter = (tw, r, i) => tw.WriteLine(new string(' ', i * 2) + r.ToString());
        }

        /// <summary>
        /// Invoked when a request is tracked. Writes a trace of the request.
        /// </summary>
        /// <param name="request">The request.</param>
        protected override void TrackRequest(object request)
        {
            this.TraceRequestFormatter(this.writer, request, this.indentLevel);
            this.indentLevel++;
        }

        /// <summary>
        /// Invoked when a created specimen is tracked. Writes a trace of the response.
        /// </summary>
        /// <param name="specimen">The specimen.</param>
        protected override void TrackCreatedSpecimen(object specimen)
        {
            this.indentLevel--;
            this.TraceCreatedSpecimenFormatter(this.writer, specimen, this.indentLevel);
        }
    }
}
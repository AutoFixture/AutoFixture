using System;
using System.IO;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Trace writer that will write out a trace of object requests and created objects
    /// in the <see cref="ISpecimenBuilder" /> pipeline.
    /// </summary>
    public class TraceWriter : ISpecimenBuilder
    {
        private readonly TracingBuilder tracer;
        private Action<TextWriter, object, int> writeRequest;
        private Action<TextWriter, object, int> writeSpecimen;

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceWriter"/> class.
        /// </summary>
        /// <param name="writer">The output stream for the trace.</param>
        /// <param name="tracer">The <see cref="ISpecimenBuilder"/> to decorate.</param>
        public TraceWriter(TextWriter writer, TracingBuilder tracer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }
            if (tracer == null)
            {
                throw new ArgumentNullException("tracer");
            }        

            this.tracer = tracer;
            this.tracer.SpecimenRequested += (sender, e) => this.writeRequest(writer, e.Request, e.Depth);
            this.tracer.SpecimenCreated += (sender, e) => this.writeSpecimen(writer, e.Specimen, e.Depth);

            this.TraceRequestFormatter = (tw, r, i) => tw.WriteLine(new string(' ', i * 2) + "Requested: " + r);
            this.TraceSpecimenFormatter = (tw, r, i) => tw.WriteLine(new string(' ', i * 2) + "Created: " + r);
        }

        /// <summary>
        /// Gets the <see cref="TracingBuilder"/> decorated by this instance.
        /// </summary>
        public TracingBuilder Tracer
        {
            get { return this.tracer; }
        }

        /// <summary>
        /// Gets or sets the formatter for tracing a request.
        /// </summary>
        /// <value>The request trace formatter.</value>
        public Action<TextWriter, object, int> TraceRequestFormatter
        {
            get { return this.writeRequest; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.writeRequest = value;
            }
        }

        /// <summary>
        /// Gets or sets the formatter for tracing a created specimen.
        /// </summary>
        /// <value>The created specimen trace formatter.</value>
        public Action<TextWriter, object, int> TraceSpecimenFormatter
        {
            get { return this.writeSpecimen; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.writeSpecimen = value;
            }
        }

        #region ISpecimenBuilder Members

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
            return this.tracer.Create(request, context);
        }

        #endregion
    }
}
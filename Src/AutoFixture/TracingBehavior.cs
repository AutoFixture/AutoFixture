using System;
using System.IO;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Decorates an <see cref="ISpecimenBuilder"/> with a <see cref="TraceWriter"/> to enable
    /// tracing for diagnosing how the decorated builder builds up specimens.
    /// </summary>
    public class TracingBehavior : ISpecimenBuilderTransformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TracingBehavior"/> class with the default
        /// <see cref="Writer"/>, which is <see cref="Console.Out"/>.
        /// </summary>
        public TracingBehavior()
            : this(Console.Out)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TracingBehavior"/> class with the supplied
        /// <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="writer">The writer to which diagnostics is written.</param>
        public TracingBehavior(TextWriter writer)
        {
            this.Writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        /// <summary>
        /// Gets the writer to which diagnostics information is written.
        /// </summary>
        public TextWriter Writer { get; }

        /// <summary>
        /// Decorates the supplied builder with a <see cref="TraceWriter"/>.
        /// </summary>
        /// <param name="builder">The builder to transform.</param>
        /// <returns>
        /// A new <see cref="TraceWriter"/> that decorates <paramref name="builder"/> using
        /// <see cref="Writer"/>.
        /// </returns>
        public ISpecimenBuilderNode Transform(ISpecimenBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return new TraceWriter(this.Writer, new TracingBuilder(builder));
        }
    }
}

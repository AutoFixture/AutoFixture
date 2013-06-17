using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Base class for recursion handling. Tracks requests and reacts when a recursion point in the
    /// specimen creation process is detected.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "The main responsibility of this class isn't to be a 'collection' (which, by the way, it isn't - it's just an Iterator).")]
    public class RecursionGuard : ISpecimenBuilderNode
    {
        private readonly ISpecimenBuilder builder;
        private readonly IRecursionHandler recursionHandler;
        private readonly IEqualityComparer comparer;
        private readonly Stack<object> monitoredRequests;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecursionGuard"/> class.
        /// </summary>
        /// <param name="builder">The intercepting builder to decorate.</param>
        public RecursionGuard(ISpecimenBuilder builder)
            : this(builder, EqualityComparer<object>.Default)
        {
        }

        public RecursionGuard(
            ISpecimenBuilder builder,
            IRecursionHandler recursionHandler)
        {
            this.recursionHandler = recursionHandler;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecursionGuard"/> class.
        /// </summary>
        /// <param name="builder">The intercepting builder to decorate.</param>
        /// <param name="comparer">
        /// An IEqualitycomparer implementation to use when comparing requests to determine recursion.
        /// </param>
        public RecursionGuard(ISpecimenBuilder builder, IEqualityComparer comparer)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            this.monitoredRequests = new Stack<object>();
            this.builder = builder;
            this.comparer = comparer;
        }

        /// <summary>
        /// Gets the decorated builder supplied via the constructor.
        /// </summary>
        /// <seealso cref="RecursionGuard(ISpecimenBuilder)"/>
        /// <seealso cref="RecursionGuard(ISpecimenBuilder, IEqualityComparer)" />
        public ISpecimenBuilder Builder
        {
            get { return this.builder; }
        }

        public IRecursionHandler RecursionHandler
        {
            get { return this.recursionHandler; }
        }

        /// <summary>Gets the comparer supplied via the constructor.</summary>
        /// <seealso cref="RecursionGuard(ISpecimenBuilder, IEqualityComparer)" />
        public IEqualityComparer Comparer
        {
            get { return this.comparer; }
        }

        /// <summary>
        /// Gets the recorded requests so far.
        /// </summary>
        protected IEnumerable RecordedRequests
        {
            get { return this.monitoredRequests; }
        }

        /// <summary>
        /// Handles a request that would cause recursion.
        /// </summary>
        /// <param name="request">The recursion causing request.</param>
        /// <returns>The specimen to return.</returns>
        public virtual object HandleRecursiveRequest(object request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A container that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// 	<para>
        /// The <paramref name="request"/> can be any object, but will often be a
        /// <see cref="Type"/> or other <see cref="System.Reflection.MemberInfo"/> instances.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (this.monitoredRequests.Any(x => this.comparer.Equals(x, request)))
            {
                return this.HandleRecursiveRequest(request);
            }

            this.monitoredRequests.Push(request);
            var specimen = this.builder.Create(request, context);
            this.monitoredRequests.Pop();
            return specimen;
        }

        /// <summary>Composes the supplied builders.</summary>
        /// <param name="builders">The builders to compose.</param>
        /// <returns>A <see cref="ISpecimenBuilderNode" /> instance.</returns>
        /// <remarks>
        /// <para>
        /// Note to implementers:
        /// </para>
        /// <para>
        /// The intent of this method is to compose the supplied
        /// <paramref name="builders" /> into a new instance of the type
        /// implementing <see cref="ISpecimenBuilderNode" />. Thus, the
        /// concrete return type is expected to the same type as the type
        /// implementing the method. However, it is not considered a failure to
        /// deviate from this idiom - it would just not be a mainstream
        /// implementation.
        /// </para>
        /// <para>
        /// The returned instance is normally expected to contain the builders
        /// supplied as an argument, but again this is not strictly required.
        /// The implementation may decide to filter the sequence or add to it
        /// during composition.
        /// </para>
        /// </remarks>
        public virtual ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{ISpecimenBuilder}" /> that can be used to
        /// iterate through the collection.
        /// </returns>
        public virtual IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            yield return this.builder;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
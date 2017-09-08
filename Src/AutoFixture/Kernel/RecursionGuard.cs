using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Base class for recursion handling. Tracks requests and reacts when a recursion point in the
    /// specimen creation process is detected.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "The main responsibility of this class isn't to be a 'collection' (which, by the way, it isn't - it's just an Iterator).")]
    public class RecursionGuard : ISpecimenBuilderNode
    {
        private readonly ConcurrentDictionary<Thread, Stack<object>>
            _requestsByThread = new ConcurrentDictionary<Thread, Stack<object>>();

        private Stack<object> GetMonitoredRequestsForCurrentThread()
        {
            return _requestsByThread.GetOrAdd(Thread.CurrentThread, _ => new Stack<object>());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecursionGuard"/> class.
        /// </summary>
        /// <param name="builder">The intercepted builder to decorate.</param>
        [Obsolete("This constructor overload is obsolete and will be removed in a future version of AutoFixture. Please use RecursionGuard(ISpecimenBuilder, IRecursionHandler) instead.")]
        public RecursionGuard(ISpecimenBuilder builder)
            : this(builder, EqualityComparer<object>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecursionGuard" />
        /// class.
        /// </summary>
        /// <param name="builder">The intercepted builder to decorate.</param>
        /// <param name="recursionHandler">
        /// An <see cref="IRecursionHandler" /> that will handle a recursion
        /// situation, if one is detected.
        /// </param>
        public RecursionGuard(
            ISpecimenBuilder builder,
            IRecursionHandler recursionHandler)
            : this(
                builder, 
                recursionHandler,
                EqualityComparer<object>.Default,
                1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecursionGuard" />
        /// class.
        /// </summary>
        /// <param name="builder">The intercepted builder to decorate.</param>
        /// <param name="recursionHandler">
        /// An <see cref="IRecursionHandler" /> that will handle a recursion
        /// situation, if one is detected.
        /// </param>
        /// <param name="recursionDepth">The recursion depth at which the request will be treated as a recursive
        /// request</param>
        public RecursionGuard(
            ISpecimenBuilder builder,
            IRecursionHandler recursionHandler,
            int recursionDepth)
            : this(
                builder, 
                recursionHandler,
                EqualityComparer<object>.Default,
                recursionDepth)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecursionGuard"/> class.
        /// </summary>
        /// <param name="builder">The intercepted builder to decorate.</param>
        /// <param name="comparer">
        /// An IEqualityComparer implementation to use when comparing requests to determine recursion.
        /// </param>
        [Obsolete("This constructor overload is obsolete and will be removed in a future version of AutoFixture. Please use RecursionGuard(ISpecimenBuilder, IRecursionHandler, IEqualityComparer, int) instead.")]
        public RecursionGuard(ISpecimenBuilder builder, IEqualityComparer comparer)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            this.Builder = builder;
            this.Comparer = comparer;
            this.RecursionDepth = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecursionGuard" />
        /// class.
        /// </summary>
        /// <param name="builder">The intercepted builder to decorate.</param>
        /// <param name="recursionHandler">
        /// An <see cref="IRecursionHandler" /> that will handle a recursion
        /// situation, if one is detected.
        /// </param>
        /// <param name="comparer">
        /// An <see cref="IEqualityComparer" /> implementation to use when
        /// comparing requests to determine recursion.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// builder
        /// or
        /// recursionHandler
        /// or
        /// comparer
        /// </exception>
        [Obsolete("This constructor overload is obsolete and will be removed in a future version of AutoFixture. Please use RecursionGuard(ISpecimenBuilder, IRecursionHandler, IEqualityComparer, int) instead.")]
        public RecursionGuard(
            ISpecimenBuilder builder,
            IRecursionHandler recursionHandler,
            IEqualityComparer comparer)
            : this(
            builder, 
            recursionHandler, 
            comparer, 
            1)
        {
        }        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RecursionGuard" />
        /// class.
        /// </summary>
        /// <param name="builder">The intercepted builder to decorate.</param>
        /// <param name="recursionHandler">
        /// An <see cref="IRecursionHandler" /> that will handle a recursion
        /// situation, if one is detected.
        /// </param>
        /// <param name="comparer">
        /// An <see cref="IEqualityComparer" /> implementation to use when
        /// comparing requests to determine recursion.
        /// </param>
        /// <param name="recursionDepth">The recursion depth at which the request will be treated as a recursive
        /// request.</param>
        /// <exception cref="System.ArgumentNullException">
        /// builder
        /// or
        /// recursionHandler
        /// or
        /// comparer
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">recursionDepth is less than one.</exception>
        public RecursionGuard(
            ISpecimenBuilder builder,
            IRecursionHandler recursionHandler,
            IEqualityComparer comparer,
            int recursionDepth)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (recursionHandler == null)
                throw new ArgumentNullException(nameof(recursionHandler));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            if (recursionDepth < 1)
                throw new ArgumentOutOfRangeException(nameof(recursionDepth), "Recursion depth must be greater than 0.");

            this.Builder = builder;
            this.RecursionHandler = recursionHandler;
            this.Comparer = comparer;
            this.RecursionDepth = recursionDepth;
        }

        /// <summary>
        /// Gets the decorated builder supplied via the constructor.
        /// </summary>
        /// <seealso cref="RecursionGuard(ISpecimenBuilder)"/>
        /// <seealso cref="RecursionGuard(ISpecimenBuilder, IEqualityComparer)" />
        public ISpecimenBuilder Builder { get; }

        /// <summary>
        /// Gets the recursion handler originally supplied as a constructor
        /// argument.
        /// </summary>
        /// <value>
        /// The recursion handler used to handle recursion situations.
        /// </value>
        /// <seealso cref="RecursionGuard(ISpecimenBuilder, IRecursionHandler)" />
        /// <seealso cref="RecursionGuard(ISpecimenBuilder, IRecursionHandler, IEqualityComparer)" />
        public IRecursionHandler RecursionHandler { get; }

        /// <summary>
        /// The recursion depth at which the request will be treated as a 
        /// recursive request
        /// </summary>
        public int RecursionDepth { get; }

        /// <summary>Gets the comparer supplied via the constructor.</summary>
        /// <seealso cref="RecursionGuard(ISpecimenBuilder, IEqualityComparer)" />
        public IEqualityComparer Comparer { get; }

        /// <summary>
        /// Gets the recorded requests so far.
        /// </summary>
        protected IEnumerable RecordedRequests
        {
            get { return GetMonitoredRequestsForCurrentThread(); }
        }

        /// <summary>
        /// Handles a request that would cause recursion.
        /// </summary>
        /// <param name="request">The recursion causing request.</param>
        /// <returns>The specimen to return.</returns>
        [Obsolete("This method will be removed in a future version of AutoFixture. Use IRecursionHandler.HandleRecursiveRequest instead.")]
        public virtual object HandleRecursiveRequest(object request)
        {
            return this.RecursionHandler.HandleRecursiveRequest(
                request,
                GetMonitoredRequestsForCurrentThread());
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
            var requestsForCurrentThread = GetMonitoredRequestsForCurrentThread();
            if (requestsForCurrentThread.Count > 0)
            {
                // This is performance-sensitive code when used repeatedly over many requests.
                // See discussion at https://github.com/AutoFixture/AutoFixture/pull/218
                var requestsArray = requestsForCurrentThread.ToArray();
                int numRequestsSameAsThisOne = 0;
                for (int i = 0; i < requestsArray.Length; i++)
                {
                    var existingRequest = requestsArray[i];
                    if (this.Comparer.Equals(existingRequest, request))
                    {
                        numRequestsSameAsThisOne++;
                    }

                    if (numRequestsSameAsThisOne >= this.RecursionDepth)
                    {
#pragma warning disable 618
                        return this.HandleRecursiveRequest(request);
#pragma warning restore 618
                    }
                }
            }

            requestsForCurrentThread.Push(request);
            try
            {
                return this.Builder.Create(request, context);
            }
            finally
            {
                requestsForCurrentThread.Pop();
            }
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
        public virtual ISpecimenBuilderNode Compose(
            IEnumerable<ISpecimenBuilder> builders)
        {
            var composedBuilder = CompositeSpecimenBuilder.ComposeIfMultiple(
                builders);
            return new RecursionGuard(
                composedBuilder,
                this.RecursionHandler,
                this.Comparer,
                this.RecursionDepth);
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
            yield return this.Builder;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
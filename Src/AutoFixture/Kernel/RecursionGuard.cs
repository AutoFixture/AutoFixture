using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Base class for recursion handling. Tracks requests and reacts when a recursion point in the
    /// specimen creation process is detected.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
        Justification = "The main responsibility of this class isn't to be a 'collection' (which, by the way, it isn't - it's just an Iterator).")]
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",
        Justification = "Fixture doesn't support disposal, so we cannot dispose current builder somehow.")]
    public class RecursionGuard : ISpecimenBuilderNode
    {
        private readonly ThreadLocal<Stack<object>> requestsByThread
            = new ThreadLocal<Stack<object>>(() => new Stack<object>());

        private Stack<object> GetMonitoredRequestsForCurrentThread() => this.requestsByThread.Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecursionGuard"/> class.
        /// </summary>
        [Obsolete("This constructor overload is obsolete and will be removed in a future version of AutoFixture. Please use RecursionGuard(ISpecimenBuilder, IRecursionHandler) instead.", true)]
        public RecursionGuard(ISpecimenBuilder builder)
            : this(builder, EqualityComparer<object>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecursionGuard" /> class.
        /// </summary>
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
        /// Initializes a new instance of the <see cref="RecursionGuard" /> class.
        /// </summary>
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
        [Obsolete("This constructor overload is obsolete and will be removed in a future version of AutoFixture. Please use RecursionGuard(ISpecimenBuilder, IRecursionHandler, IEqualityComparer, int) instead.", true)]
        public RecursionGuard(ISpecimenBuilder builder, IEqualityComparer comparer)
        {
            this.Builder = builder ?? throw new ArgumentNullException(nameof(builder));
            this.Comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            this.RecursionDepth = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecursionGuard" /> class.
        /// </summary>
        [Obsolete("This constructor overload is obsolete and will be removed in a future version of AutoFixture. Please use RecursionGuard(ISpecimenBuilder, IRecursionHandler, IEqualityComparer, int) instead.", true)]
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
        public RecursionGuard(
            ISpecimenBuilder builder,
            IRecursionHandler recursionHandler,
            IEqualityComparer comparer,
            int recursionDepth)
        {
            if (recursionDepth < 1)
                throw new ArgumentOutOfRangeException(nameof(recursionDepth), "Recursion depth must be greater than 0.");

            this.Builder = builder ?? throw new ArgumentNullException(nameof(builder));
            this.RecursionHandler = recursionHandler ?? throw new ArgumentNullException(nameof(recursionHandler));
            this.Comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            this.RecursionDepth = recursionDepth;
        }

        /// <summary>
        /// Gets the decorated builder supplied via the constructor.
        /// </summary>
        public ISpecimenBuilder Builder { get; }

        /// <summary>
        /// Gets the recursion handler originally supplied as a constructor argument.
        /// </summary>
        public IRecursionHandler RecursionHandler { get; }

        /// <summary>
        /// The recursion depth at which the request will be treated as a recursive request.
        /// </summary>
        public int RecursionDepth { get; }

        /// <summary> Gets the comparer supplied via the constructor. </summary>
        public IEqualityComparer Comparer { get; }

        /// <summary> Gets the recorded requests so far. </summary>
        protected IEnumerable RecordedRequests => this.GetMonitoredRequestsForCurrentThread();

        /// <summary>
        /// Handles a request that would cause recursion.
        /// </summary>
        [Obsolete("This method will be removed in a future version of AutoFixture. Use IRecursionHandler.HandleRecursiveRequest instead.", true)]
        public virtual object HandleRecursiveRequest(object request)
        {
            return this.RecursionHandler.HandleRecursiveRequest(
                request,
                this.GetMonitoredRequestsForCurrentThread());
        }

        /// <inheritdoc />
        public object Create(object request, ISpecimenContext context)
        {
            var requestsForCurrentThread = this.GetMonitoredRequestsForCurrentThread();
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
                        return ObsoletedMemberShims.RecursionGuard_HandleRecursiveRequest(this, request);
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

        /// <inheritdoc />
        public virtual ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            if (builders == null) throw new ArgumentNullException(nameof(builders));

            var composedBuilder = CompositeSpecimenBuilder.ComposeIfMultiple(builders);
            return new RecursionGuard(
                composedBuilder,
                this.RecursionHandler,
                this.Comparer,
                this.RecursionDepth);
        }

        /// <inheritdoc />
        public virtual IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            yield return this.Builder;
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
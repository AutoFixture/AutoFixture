using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace AutoFixture.Kernel;

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
    private readonly ThreadLocal<Stack<object>> _requestsByThread = new(() => new Stack<object>());

    private Stack<object> GetMonitoredRequestsForCurrentThread() => _requestsByThread.Value;

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

        Builder = builder ?? throw new ArgumentNullException(nameof(builder));
        RecursionHandler = recursionHandler ?? throw new ArgumentNullException(nameof(recursionHandler));
        Comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        RecursionDepth = recursionDepth;
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
    protected IEnumerable RecordedRequests => GetMonitoredRequestsForCurrentThread();

    /// <inheritdoc />
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
                if (Comparer.Equals(existingRequest, request))
                {
                    numRequestsSameAsThisOne++;
                }

                if (numRequestsSameAsThisOne >= RecursionDepth)
                {
                    return RecursionHandler.HandleRecursiveRequest(
                        request,
                        GetMonitoredRequestsForCurrentThread());
                }
            }
        }

        requestsForCurrentThread.Push(request);
        try
        {
            return Builder.Create(request, context);
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
            RecursionHandler,
            Comparer,
            RecursionDepth);
    }

    /// <inheritdoc />
    public virtual IEnumerator<ISpecimenBuilder> GetEnumerator()
    {
        yield return Builder;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
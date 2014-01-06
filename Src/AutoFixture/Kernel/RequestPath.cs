using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Stores the path to a specimen request.
    /// </summary>
    internal class RequestPath : IEnumerable<object>
    {
        class PathPopper : IDisposable
        {
            private readonly RequestPath _parent;

            public PathPopper(RequestPath parent)
            {
                _parent = parent;
            }

            public void Dispose()
            {
                _parent.Pop();
            }
        }

        private readonly Stack<object> stack;
        private readonly Dictionary<object, int> requestCountByRequest;

        /// <summary>
        /// Constructs a new empty <see cref="RequestPath"/> instance.
        /// </summary>
        /// <param name="comparer">A comparer of request objects</param>
        public RequestPath(IEqualityComparer comparer)
        {
            if (comparer == null) throw new ArgumentNullException("comparer");
            this.stack = new Stack<object>();
            this.requestCountByRequest = new Dictionary<object, int>(
                new GenericsEqualityComparerAdapter(comparer));
        }

        /// <summary>
        /// Appends the <paramref name="request"/> to the end of this request path.
        /// </summary>
        /// <param name="request">The request to append</param>
        public IDisposable BeginChildRequest(object request)
        {
            Push(request);
            return new PathPopper(this);
        }

        public void Push(object request)
        {
            this.stack.Push(request);
            int existingCount;
            this.requestCountByRequest.TryGetValue(request, out existingCount);
            this.requestCountByRequest[request] = ++existingCount;
        }

        public void Pop()
        {
            this.requestCountByRequest.Remove(this.stack.Pop());
        }

        IEnumerator<object> IEnumerable<object>.GetEnumerator()
        {
            return stack.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return stack.GetEnumerator();
        }

        /// <summary>
        /// Gets the number of existing requests already in the path, where those requests
        /// match (are equal to) the given <paramref name="request"/>.
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>The number of requests that are equal to the given 
        /// <paramref name="request"/></returns>
        public int GetCount(object request)
        {
            int count;
            return this.requestCountByRequest.TryGetValue(request, out count) ? count : 0;
        }
    }
}
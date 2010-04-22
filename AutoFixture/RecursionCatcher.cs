namespace Ploeh.AutoFixture
{
    using System.Collections.Generic;
	using Kernel;

	public abstract class RecursionCatcher : RequestTracker
	{
		private Stack<object> monitoredRequests;
        private InterceptingBuilder interceptor;

		protected RecursionCatcher(InterceptingBuilder interceptBuilder) : base(interceptBuilder)
		{
			monitoredRequests = new Stack<object>();
            interceptor = interceptBuilder;
		}

		protected override void TrackRequest(object request)
		{
			if (monitoredRequests.Contains(request))
            {
                interceptor.SetInterception(request, GetRecursionBreakSpecimen(request));
            }

            monitoredRequests.Push(request);
		}

		protected override void TrackCreatedSpecimen(object specimen)
		{
			monitoredRequests.Pop();
		}

		protected abstract object GetRecursionBreakSpecimen(object request);
	}
}
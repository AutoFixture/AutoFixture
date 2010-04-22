namespace Ploeh.AutoFixture
{
	using System;
	using System.Collections.Generic;
	using Kernel;

	public abstract class RecursionCatcher : RequestTracker
	{
		private Stack<object> monitoredRequests;
		private Stack<object> interception;

		protected RecursionCatcher(ISpecimenBuilder builder) : base(builder)
		{
			monitoredRequests = new Stack<object>();
			interception = new Stack<object>();
		}

		protected override void TrackRequest(object request)
		{
			if (monitoredRequests.Contains(request))
			{
				interception.Push(GetRecursionBreakInstance(request));
			}

			monitoredRequests.Push(request);
		}

		protected override void TrackCreatedSpecimen(object specimen)
		{
			monitoredRequests.Pop();
		}

		protected override object GetCreationInterception()
		{
			if (interception.Count > 0)
				return interception.Pop();

			return new NoSpecimen();
		}

		protected abstract object GetRecursionBreakInstance(object request);
	}
}
using System;
using System.Collections.ObjectModel;

namespace Ploeh.AutoFixture.Kernel
{
	public abstract class RequestTracker : ISpecimenBuilder
	{
		private bool skip;

		public Collection<Type> IgnoredTypes
		{
			get;
			private set;
		}

		protected RequestTracker()
		{
			IgnoredTypes = new Collection<Type>();
		}

		public object Create(object request, ISpecimenContainer container)
		{
			if (skip)
			{
				skip = false;
				return new NoSpecimen(request);
			}

			if (!IgnoredTypes.Contains(request.GetType()))
				TrackRequest(request);
			skip = true;
			object specimen = container.Create(request);
			if (!IgnoredTypes.Contains(request.GetType()))
				TrackCreatedSpecimen(specimen);
			return specimen;
		}

		protected abstract void TrackRequest(object request);

		protected abstract void TrackCreatedSpecimen(object specimen);
	}
}
namespace Ploeh.AutoFixture
{
	using System;
	using System.IO;
	using Kernel;

	public class BuildTraceWriter : RequestTracker
	{
		private TextWriter outputStream;
		private int indentLevel = 0;

		public Func<object, int, string> TrackedRequestFormatter
		{
			get;
			set;
		}

		public Func<object, int, string> TrackedCreatedSpecimenFormatter
		{
			get;
			set;
		}

		public int IndentLevel
		{
			get;
			private set;
		}

		public BuildTraceWriter(TextWriter outStream)
		{
			outputStream = outStream;
			TrackedRequestFormatter = (obj, i) => obj.ToString();
			TrackedCreatedSpecimenFormatter = (obj, i) => obj.ToString();
		}

		protected override void TrackRequest(object request)
		{
			outputStream.WriteLine(TrackedRequestFormatter(request, indentLevel));
			indentLevel++;
		}

		protected override void TrackCreatedSpecimen(object specimen)
		{
			indentLevel--;
			outputStream.WriteLine(TrackedCreatedSpecimenFormatter(specimen, indentLevel));
		}
	}
}
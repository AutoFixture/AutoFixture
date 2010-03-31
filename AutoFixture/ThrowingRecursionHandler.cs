namespace Ploeh.AutoFixture
{
	using System;
	using System.Globalization;
	using System.Linq;

	/// <summary>
	/// Recursion monitor that throws an exception if recursion is detected.
	/// </summary>
	public class ThrowingRecursionHandler : RecursionHandler
	{
		/// <summary>
		/// Gets the recursion break instance.
		/// </summary>
		/// <param name="theType">The type of instance at the recursion point.</param>
		/// <returns>An instance of the type.</returns>
		public override object GetRecursionBreakInstance(Type theType)
		{
			throw new ObjectCreationException(string.Format(
						CultureInfo.InvariantCulture,
						"AutoFixture was unable to create an instance of type {0} because the traversed object graph contains a circular referece. Path: {1}.",
						this.ProcessedTypes.First(),
						FlattenProcessedTypes() + theType));
		}
	}
}
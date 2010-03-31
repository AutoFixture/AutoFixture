namespace Ploeh.AutoFixture
{
	using System;

	/// <summary>
	/// Recursion monitor that returns null 
	/// </summary>
	public class NullRecursionHandler : RecursionHandler
	{
		/// <summary>
		/// Gets the recursion break instance.
		/// </summary>
		/// <param name="theType">The type of instance at the recursion point.</param>
		/// <returns>An instance of the type.</returns>
		public override object GetRecursionBreakInstance(Type theType)
		{
			return null;
		}
	}
}
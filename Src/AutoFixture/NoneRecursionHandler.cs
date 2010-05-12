namespace Ploeh.AutoFixture
{
	using System;

	/// <summary>
	/// Recursion monitor that does nothing.
	/// </summary>
	public class NoneRecursionHandler : RecursionHandler
	{
		/// <summary>
		/// Puts a recursion check on the specified created object type, and returns a boolean indicating if a recursion point has been detected.
		/// </summary>
		/// <param name="typeToCheck">The type of object.</param>
		/// <returns>
		/// true if recursion point is detected, otherwise, false.
		/// </returns>
		public override bool Check(Type typeToCheck)
		{
			this.ProcessedTypes.Add(typeToCheck);
			return false;
		}

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
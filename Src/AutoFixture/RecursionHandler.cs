namespace Ploeh.AutoFixture
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Text;

	/// <summary>
	/// Keeps track of created objects and provides options for breaking endless recursion loops during object creation.
	/// </summary>
	public abstract class RecursionHandler
	{
		/// <summary>
		/// Gets or sets the processed types.
		/// </summary>
		/// <value>The processed types.</value>
		protected Collection<Type> ProcessedTypes
		{
			get;
			private set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RecursionHandler"/> class.
		/// </summary>
		protected RecursionHandler()
		{
			ProcessedTypes = new Collection<Type>();
		}

		/// <summary>
		/// Puts a recursion check on the specified created object type, and returns a boolean indicating if a recursion point has been detected.
		/// </summary>
		/// <param name="typeToCheck">The type of object.</param>
		/// <returns>true if recursion point is detected, otherwise, false.</returns>
		public virtual bool Check(Type typeToCheck)
		{
			if (this.ProcessedTypes.Contains(typeToCheck))
				return true;

			this.ProcessedTypes.Add(typeToCheck);
			return false;
		}

		/// <summary>
		/// Gets the recursion break instance.
		/// </summary>
		/// <param name="theType">The type of instance at the recursion point.</param>
		/// <returns>An instance of the type.</returns>
		public abstract object GetRecursionBreakInstance(Type theType);

		/// <summary>
		/// Removes a recursion check on the specified created object type.
		/// </summary>
		/// <param name="theType">The object type to remove.</param>
		public virtual void Uncheck(Type theType)
		{
			this.ProcessedTypes.Remove(theType);
		}

		/// <summary>
		/// Flattens the processed types so far for use in a friendly error message.
		/// </summary>
		/// <returns></returns>
		protected string FlattenProcessedTypes()
		{
			var sb = new StringBuilder();
			foreach (Type theType in this.ProcessedTypes)
				sb.Append(theType.ToString() + " --> ");
			return sb.ToString();
		}
	}
}
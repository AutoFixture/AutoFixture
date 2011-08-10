using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
	public class ArrayFavoringConstructorQuery : IMethodQuery
	{
		#region IMethodQuery Members

		public IEnumerable<IMethod> SelectMethods(Type type)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			return from ci in type.GetConstructors()
				   let score = new ArrayParameterScore(ci.GetParameters())
				   orderby score descending
				   select new ConstructorMethod(ci) as IMethod;
		}

		#endregion

		private class ArrayParameterScore : IComparable<ArrayParameterScore>
		{
			private readonly int score;

			public ArrayParameterScore(IEnumerable<ParameterInfo> parameters)
			{
				if (parameters == null)
					throw new ArgumentNullException("parameters");

				this.score = parameters.Count(p => p.ParameterType.IsArray);
			}

			#region IComparable<ArrayParameterScore> Members

			public int CompareTo(ArrayParameterScore other)
			{
				if (other == null)
				{
					return 1;
				}

				return this.score.CompareTo(other.score);
			}

			#endregion
		}
	}
}

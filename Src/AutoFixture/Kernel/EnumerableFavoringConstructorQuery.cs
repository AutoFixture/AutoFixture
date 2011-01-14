using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    public class EnumerableFavoringConstructorQuery : IConstructorQuery
    {
        #region IConstructorQuery Members

        public IEnumerable<IMethod> SelectConstructors(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return from ci in type.GetConstructors()
                   let parameters = ci.GetParameters()
                   orderby EnumerableFavoringConstructorQuery.Score(type, parameters) descending
                   select new ConstructorMethod(ci) as IMethod;
        }

        #endregion

        private static int Score(Type type, ParameterInfo[] parameters)
        {
            var genericParameterTypes = type.GetGenericArguments();
            if (genericParameterTypes.Length != 1)
            {
                return 0;
            }
            var genericParameterType = genericParameterTypes.Single();

            var enumerableType = typeof(IEnumerable<>).MakeGenericType(genericParameterType);
            return parameters.Count(p => enumerableType.IsAssignableFrom(p.ParameterType));
        }
    }
}

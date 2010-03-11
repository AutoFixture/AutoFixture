using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Creates a new instance of the requested type by invoking its most modest constructor.
    /// </summary>
    public class ModestConstructorInvoker : ISpecimenBuilder
    {
        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates a specimen of the requested type by invoking its most modest constructor.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="container">A container that can be used to create other specimens.</param>
        /// <returns>
        /// A specimen generated from the requested type's most modest constructor, if possible;
        /// otherwise, <see langword="null"/>.
        /// </returns>
        public object Create(object request, ISpecimenContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            var ctor = ModestConstructorInvoker.GetModestConstructor(request);
            if (ctor == null)
            {
                return null;
            }

            var paramValues = (from pi in ctor.GetParameters()
                               select container.Create(pi)).ToList();

            if (paramValues.Any(pv => pv == null))
            {
                return null;
            }

            return ctor.Invoke(paramValues.ToArray());
        }

        #endregion

        private static ConstructorInfo GetModestConstructor(object request)
        {
            var requestedType = request as Type;
            if (requestedType == null)
            {
                return null;
            }

            return (from ci in requestedType.GetConstructors()
                    let ps = ci.GetParameters()
                    orderby ps.Length ascending
                    select ci).FirstOrDefault();
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.DataAnnotations
{
    /// <summary>
    /// Relays a request for a range number to a <see cref="RangedNumberRequest"/>.
    /// </summary>
    public class RangeAttributeRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new specimen based on a requested range.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A container that can be used to create other specimens.</param>
        /// <returns>
        /// A specimen created from a <see cref="RangedNumberRequest"/> encapsulating the operand
        /// type, the minimum and the maximum of the requested number, if possible; otherwise,
        /// a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (request == null)
            {
                return new NoSpecimen();
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            // On .NET Framework 3.5 RangeAttribute can be applied only on Properties and Fields.
            // On .NET Framrwork 4.0 RangeAttribute can be applied also on Parameters.
            var mi = request as MemberInfo;
            if (mi == null)
            {
                return new NoSpecimen(request);
            }

            // Only one instance of the indicated attribute can be specified for a single element.
            var rangeAttribute = mi.GetCustomAttributes(typeof(RangeAttribute), inherit: true).Cast<RangeAttribute>().FirstOrDefault();
            if (rangeAttribute == null)
            {
                return new NoSpecimen(request);
            }

            var specimen = context.Resolve(new RangedNumberRequest(rangeAttribute.OperandType, rangeAttribute.Minimum, rangeAttribute.Maximum));

            return specimen;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Kernel;

namespace AutoFixture.DataAnnotations
{
    public class StringMinLengthAttributeRelay : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request == null)
            {
                return new NoSpecimen();
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var stringLengthAttribute = TypeEnvy.GetAttribute<StringLengthAttribute>(request);
            if (stringLengthAttribute == null)
            {
                return new NoSpecimen();
            }

            return context.Resolve(new ConstrainedMinLengthStringRequest(stringLengthAttribute.MinimumLength));
        }
    }
}

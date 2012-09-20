using Ploeh.AutoFixture.Kernel;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Ploeh.AutoFixture
{
    public class RandomNumericSequenceGenerator : ISpecimenBuilder
    {
        private readonly IEnumerable<int> boundaries;

        public RandomNumericSequenceGenerator(IEnumerable<int> boundaries)
            : this(boundaries.ToArray())
        {
        }

        public RandomNumericSequenceGenerator(params int[] boundaries)
        {
            if (boundaries == null)
            {
                throw new ArgumentNullException("boundaries");
            }

            this.boundaries = boundaries;
        }

        public IEnumerable<int> Boundaries
        {
            get { return this.boundaries; }
        }

        public object Create(object request, ISpecimenContext context)
        {
            var type = request as Type;
            if (type == null)
            {
                return new NoSpecimen(request);
            }

            return null;
        }
    }
}

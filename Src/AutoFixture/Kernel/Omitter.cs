using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class Omitter : ISpecimenBuilder
    {
        private readonly IRequestSpecification specification;

        public Omitter()
            : this(new TrueRequestSpecification())
        {
        }

        public Omitter(IRequestSpecification specification)
        {
            if (specification == null)
                throw new ArgumentNullException("specification");

            this.specification = specification;
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (this.specification.IsSatisfiedBy(request))
                return new OmitSpecimen();

            return new NoSpecimen(request);
        }

        public IRequestSpecification Specification
        {
            get { return this.specification; }
        }
    }
}

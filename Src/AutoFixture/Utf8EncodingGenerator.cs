using Ploeh.AutoFixture.Kernel;
using System.Text;

namespace Ploeh.AutoFixture
{
    public class Utf8EncodingGenerator : ISpecimenBuilder
    {
        private readonly ExactTypeSpecification encodingTypeSpecification = new ExactTypeSpecification(typeof(Encoding));

        public object Create(object request, ISpecimenContext context)
        {
            if (request == null)
            {
                return new NoSpecimen();
            }

            if (!this.encodingTypeSpecification.IsSatisfiedBy(request))
            {
                return new NoSpecimen();
            }

            return Encoding.UTF8;
        }
    }
}
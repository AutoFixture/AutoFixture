using Ploeh.AutoFixture.Kernel;
using System.Text;

namespace Ploeh.AutoFixture
{
    public class Utf8EncodingGenerator : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (typeof(Encoding).Equals(request))
            {
                return Encoding.UTF8;
            }
            return new NoSpecimen();
        }
    }
}
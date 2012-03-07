using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.DataAnnotations;

namespace Ploeh.AutoFixture
{
#warning Consider removing this class, as it may be redundant
    public class DefaultRelaysPipe : ISpecimenBuilderPipe
    {
        public IEnumerable<ISpecimenBuilder> Pipe(IEnumerable<ISpecimenBuilder> builders)
        {
            yield return new ArrayRelay();
            yield return new MethodInvoker(
                new CompositeMethodQuery());
            yield return new ParameterRequestRelay();
            yield return new RangeAttributeRelay();
            yield return new StringLengthAttributeRelay();
            yield return new PropertyRequestRelay();
            yield return new FieldRequestRelay();
            yield return new FiniteSequenceRelay();
            yield return new SeedIgnoringRelay();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class FixedPipe : ISpecimenBuilderPipe
    {
        private readonly ISpecimenBuilder builder;

        public FixedPipe(ISpecimenBuilder builder)
        {
            this.builder = builder;
        }

        public IEnumerable<ISpecimenBuilder> Pipe(IEnumerable<ISpecimenBuilder> builders)
        {
            return new[] { this.builder }.Concat(builders);
        }

        public ISpecimenBuilder Builder
        {
            get { return this.builder; }
        }
    }
}

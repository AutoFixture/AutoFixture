using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    public class MarkerNode : ISpecimenBuilderNode
    {
        private readonly ISpecimenBuilder builder;

        public MarkerNode(ISpecimenBuilder builder)
        {
            this.builder = builder;
        }
        
        public ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            var l = builders.ToList();
            if (l.Count == 1)
                return new MarkerNode(l.Single());
            return new MarkerNode(new CompositeSpecimenBuilder(builders));
        }

        public object Create(object request, ISpecimenContext context)
        {
            return this.builder.Create(request, context);
        }

        public IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            yield return this.builder;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class TypedNode : ISpecimenBuilderNode
    {
        private readonly Type targetType;
        private readonly ISpecimenBuilder factory;
        private readonly ISpecimenBuilderNode graph;

        public TypedNode(Type targetType, ISpecimenBuilder factory)
        {
            if (targetType == null)
                throw new ArgumentNullException("targetType");
            if (factory == null)
                throw new ArgumentNullException("factory");

            this.targetType = targetType;
            this.factory = factory;
            this.graph = new FilteringSpecimenBuilder(
                new CompositeSpecimenBuilder(
                    new NoSpecimenOutputGuard(
                        factory,
                        new InverseRequestSpecification(
                            new SeedRequestSpecification(
                                targetType))),
                    new SeedIgnoringRelay()),
                new OrRequestSpecification(
                    new SeedRequestSpecification(targetType),
                    new ExactTypeSpecification(targetType)));
        }

        public virtual ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            throw new NotImplementedException();
        }

        public object Create(object request, ISpecimenContext context)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            yield return graph;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public Type TargetType
        {
            get { return this.targetType; }
        }

        public ISpecimenBuilder Factory
        {
            get { return this.factory; }
        }
    }
}

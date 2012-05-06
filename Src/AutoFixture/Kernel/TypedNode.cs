using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class TypedNode : FilteringSpecimenBuilder
    {
        private readonly Type targetType;
        private readonly ISpecimenBuilder factory;

        public TypedNode(Type targetType, ISpecimenBuilder factory)
            : base(DecorateFactory(targetType, factory), CreateSpecification(targetType))
        {
            this.targetType = targetType;
            this.factory = factory;
        }
        
        public Type TargetType
        {
            get { return this.targetType; }
        }

        public ISpecimenBuilder Factory
        {
            get { return this.factory; }
        }

        private static ISpecimenBuilder DecorateFactory(
            Type targetType, ISpecimenBuilder factory)
        {
            return new CompositeSpecimenBuilder(
                new NoSpecimenOutputGuard(
                    factory,
                    new InverseRequestSpecification(
                        new SeedRequestSpecification(
                            targetType))),
                new SeedIgnoringRelay());
        }

        private static IRequestSpecification CreateSpecification(
            Type targetType)
        {
            return new OrRequestSpecification(
                new SeedRequestSpecification(targetType),
                new ExactTypeSpecification(targetType));
        }
    }
}

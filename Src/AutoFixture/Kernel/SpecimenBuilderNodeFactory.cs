using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Dsl;

namespace Ploeh.AutoFixture.Kernel
{
    public class SpecimenBuilderNodeFactory
    {
        public static NodeComposer<T> CreateComposer<T>()
        {
            return new NodeComposer<T>(
                SpecimenBuilderNodeFactory.CreateTypedNode(
                    typeof(T),
                    new MethodInvoker(
                        new ModestConstructorQuery())));
        }

        public static FilteringSpecimenBuilder CreateTypedNode(
            Type targetType,
            ISpecimenBuilder factory)
        {
            return new FilteringSpecimenBuilder(
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
    }
}

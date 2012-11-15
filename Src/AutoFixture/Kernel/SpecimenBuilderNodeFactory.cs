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
                new TypedNode(
                    typeof(T),
                    new MethodInvoker(
                        new ModestConstructorQuery())));
        }
    }
}

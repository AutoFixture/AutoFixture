using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Ploeh.AutoFixture.Idioms
{
    public class DefaultTypeGuardSpecificationCollection : Collection<ITypeGuardSpecification>
    {
        public DefaultTypeGuardSpecificationCollection() : base (
            new ITypeGuardSpecification[] {
                new GuidGuardSpecification(), 
                new ValueTypeGuardSpecification(),
                new StringGuardSpecification(),
                new ReferenceTypeGuardSpecification()
            }.ToList())
        {
        }
    }
}
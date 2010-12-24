using System.Collections.Generic;

namespace Ploeh.AutoFixture.Idioms
{
    public interface IPickedProperty
    {
        void IsWellBehavedWritableProperty();
        
        void AssertInvariants();

        void AssertInvariants(IBoundaryConvention convention);
    }
}
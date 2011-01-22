using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public class MultipleCustomization : ICustomization
    {
        #region ICustomization Members

        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            fixture.ResidueCollectors.Add(new CollectionRelay());
            fixture.ResidueCollectors.Add(new ListRelay());
            fixture.ResidueCollectors.Add(new EnumerableRelay());

            fixture.Customizations.Add(
                new FilteringSpecimenBuilder(
                    new ConstructorInvoker(
                        new ListFavoringConstructorQuery()),
                        new CollectionSpecification()));
            fixture.Customizations.Add(
                new FilteringSpecimenBuilder(
                    new ConstructorInvoker(
                        new EnumerableFavoringConstructorQuery()),
                        new HashSetSpecification()));
            fixture.Customizations.Add(
                new FilteringSpecimenBuilder(
                    new ConstructorInvoker(
                        new EnumerableFavoringConstructorQuery()),
                        new ListSpecification()));
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    internal class ValueObjectPropertyModificatorUsingDifferentValues : ValueObjectMemberModificator
    {
        public ValueObjectPropertyModificatorUsingDifferentValues(IFixture fixture)
            : base(fixture)
        {
        }

        internal override void ChangeMembers(List<Tuple<object, object, StringBuilder>> listOfObjectsToModify, Type type, ISpecimenContext context)
        {
            var propertyInfos = GetProperties(type).ToArray();
            int count = propertyInfos.Count();
            for (int i = 0; i < listOfObjectsToModify.Count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    var propertyValue = this.Fixture.Create(propertyInfos[j].PropertyType, context);
                    propertyInfos[j].SetValue(listOfObjectsToModify[i].Item1, propertyValue, null);
                    if (i == j)
                    {
                        propertyValue = this.Fixture.Create(propertyInfos[j].PropertyType, context);
                        listOfObjectsToModify[i].Item3.AppendFormat(
                                      "All settable properties were set using the same values except of one: name: {0}, type: {1}. ",
                                      propertyInfos[j].Name, propertyInfos[j].PropertyType);
                    }
                    propertyInfos[j].SetValue(listOfObjectsToModify[i].Item2, propertyValue, null);
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    internal class ValueObjectPropertyModificatorUsingTheSameValues : ValueObjectMemberModificator
    {
        public ValueObjectPropertyModificatorUsingTheSameValues(IFixture fixture) : base(fixture)
        {
        }

        internal override void ChangeMembers(List<Tuple<object, object, StringBuilder>> listOfObjectsToModify, Type type, ISpecimenContext context)
        {
            foreach (var objectToModify in listOfObjectsToModify)
            {
                foreach (var propertyInfo in GetProperties(type))
                {
                    var propertyValue = this.Fixture.Create(propertyInfo.PropertyType, context);
                    propertyInfo.SetValue(objectToModify.Item1, propertyValue, null);
                    propertyInfo.SetValue(objectToModify.Item2, propertyValue, null);
                }
                objectToModify.Item3.Append("All settable properties were set with the same values. ");
            }
        }
    }
}
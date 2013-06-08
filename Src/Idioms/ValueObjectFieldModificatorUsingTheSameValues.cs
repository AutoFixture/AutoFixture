using System;
using System.Collections.Generic;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    internal class ValueObjectFieldModificatorUsingTheSameValues : ValueObjectMemberModificator
    {
        public ValueObjectFieldModificatorUsingTheSameValues(IFixture fixture) : base(fixture)
        {
        }

        internal override void ChangeMembers(List<Tuple<object, object>> listOfObjectsToModify, Type type, ISpecimenContext context)
        {
            foreach (var objectToModify in listOfObjectsToModify)
            {
                foreach (var fieldInfo in GetFields(type))
                {
                    var fieldValue = this.Fixture.Create(fieldInfo.FieldType, context);
                    fieldInfo.SetValue(objectToModify.Item1, fieldValue);
                    fieldInfo.SetValue(objectToModify.Item2, fieldValue);
                }
            }
        }
    }
}
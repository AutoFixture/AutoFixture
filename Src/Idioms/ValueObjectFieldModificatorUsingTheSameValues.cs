using System;
using System.Collections.Generic;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    internal class ValueObjectFieldModificatorUsingTheSameValues : ValueObjectMemberModificator
    {
        public ValueObjectFieldModificatorUsingTheSameValues(IFixture fixture) : base(fixture)
        {
        }

        internal override void ChangeMembers(List<Tuple<object, object, StringBuilder>> listOfObjectsToModify, Type type, ISpecimenContext context)
        {
            foreach (var objectToModify in listOfObjectsToModify)
            {
                foreach (var fieldInfo in GetFields(type))
                {
                    var fieldValue = this.Fixture.Create(fieldInfo.FieldType, context);
                    fieldInfo.SetValue(objectToModify.Item1, fieldValue);
                    fieldInfo.SetValue(objectToModify.Item2, fieldValue);
                }
                objectToModify.Item3.Append("All public fields were set with the same values. ");

            }
        }
    }
}
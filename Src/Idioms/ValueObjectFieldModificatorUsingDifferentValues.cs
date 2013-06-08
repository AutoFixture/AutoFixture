using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    internal class ValueObjectFieldModificatorUsingDifferentValues : ValueObjectMemberModificator
    {
        public ValueObjectFieldModificatorUsingDifferentValues(IFixture fixture) : base(fixture)
        {
        }

        internal override void ChangeMembers(List<Tuple<object, object>> listOfObjectsToModify, Type type, ISpecimenContext context)
        {
            var fieldInfos = GetFields(type).ToArray();
            int count = fieldInfos.Count();
            for (int i = 0; i < listOfObjectsToModify.Count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    var fieldValue = this.Fixture.Create(fieldInfos[j].FieldType, context);
                    fieldInfos[j].SetValue(listOfObjectsToModify[i].Item1, fieldValue);
                    if (i == j)
                        fieldValue = this.Fixture.Create(fieldInfos[j].FieldType, context);
                    fieldInfos[j].SetValue(listOfObjectsToModify[i].Item2, fieldValue);
                }
            }
        }
    }
}
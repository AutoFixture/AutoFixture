using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    internal class ValueObjectFieldModificatorUsingDifferentValues : ValueObjectMemberModificator
    {
        public ValueObjectFieldModificatorUsingDifferentValues(IFixture fixture) : base(fixture)
        {
        }

        internal override void ChangeMembers(List<Tuple<object, object, StringBuilder>> listOfObjectsToModify, Type type, ISpecimenContext context)
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
                    {
                        fieldValue = this.Fixture.Create(fieldInfos[j].FieldType, context);
                        listOfObjectsToModify[i].Item3.AppendFormat(
                                      "All public fields were set using the same values except of one: name: {0}, type: {1}. ",
                                      fieldInfos[j].Name, fieldInfos[j].FieldType);
                    }
                    fieldInfos[j].SetValue(listOfObjectsToModify[i].Item2, fieldValue);
                    

                }
            }
        }
    }
}
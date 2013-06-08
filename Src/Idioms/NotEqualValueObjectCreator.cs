using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    internal class NotEqualValueObjectCreator : ValueObjectCreator
    {
        public NotEqualValueObjectCreator(IFixture fixture, IMethodQuery query)
            : base(fixture, query)
        {
        }

        internal override List<List<Tuple<object, object>>> BuildType(Type type, ISpecimenContext context)
        {
            var listOfLists = new List<List<Tuple<object, object>>>();
            var constructors = this.Query.SelectMethods(type).ToArray();
            for (int i = 0; i < constructors.Count(); i++)
            {
                var listOfObjects = new List<Tuple<object, object>>();
                var paramValues = (from pi in constructors[i].Parameters
                                   select this.Fixture.Create(pi, context)).ToList();

                if (paramValues.All(x => !(x is NoSpecimen)))
                {
                    int count = paramValues.Count;
                    for (int j = 0; j < count; j++)
                    {
                        var firstObject = constructors[i].Invoke(paramValues.ToArray());
                        paramValues[j] = this.Fixture.Create(paramValues[j].GetType(), context);
                        var secondObject = constructors[i].Invoke(paramValues.ToArray());
                        listOfObjects.Add(new Tuple<object, object>(firstObject, secondObject));
                    }
                }
                listOfLists.Add(listOfObjects);
            }
            return listOfLists;
        }
    }
}
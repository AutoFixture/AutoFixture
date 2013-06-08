using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    internal class EqualValueObjectCreator : ValueObjectCreator
    {
        public EqualValueObjectCreator(IFixture fixture, IMethodQuery query) : base(fixture, query)
        {
        }

        internal override List<List<Tuple<object, object>>> BuildType(Type type, ISpecimenContext context)
        {
            var listOfLists = new List<List<Tuple<object, object>>>();
            foreach (var selectedConstructor in this.Query.SelectMethods(type))
            {
                
                var paramValues = (from pi in selectedConstructor.Parameters
                                   select this.Fixture.Create(pi, context)).ToList();

                if (paramValues.All(x => !(x is NoSpecimen)))
                {
                    var listOfObjects = BuildTwoObjects(type, selectedConstructor, paramValues);
                    listOfLists.Add(listOfObjects);
                }
            }
            
            return listOfLists;
        }

        private List<Tuple<object, object>> BuildTwoObjects(Type type, IMethod selectConstructor, List<object> paramValues)
        {
            var listOfObjects = new List<Tuple<object, object>>();
            var propertyInfos = this.GetProperties(type);
            var count = propertyInfos.Count();
            count = count == 0 ? 1 : count;
            for (int j = 0; j < count; j++)
            {
                var firstObject = selectConstructor.Invoke(paramValues.ToArray());
                var secondObject = selectConstructor.Invoke(paramValues.ToArray());
                listOfObjects.Add(new Tuple<object, object>(firstObject, secondObject));
            }
            return listOfObjects;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    internal class EqualValueObjectCreator : ValueObjectCreator
    {
        public EqualValueObjectCreator(IFixture fixture, IMethodQuery query) : base(fixture, query)
        {
        }

        internal override List<List<Tuple<object, object, StringBuilder>>> BuildType(Type type, ISpecimenContext context)
        {
            var listOfLists = new List<List<Tuple<object, object, StringBuilder>>>();
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

        private List<Tuple<object, object, StringBuilder>> BuildTwoObjects(Type type, IMethod selectedConstructor, List<object> paramValues)
        {
            var listOfObjects = new List<Tuple<object, object, StringBuilder>>();
            var propertyInfos = ValueObjectMemberModificator.GetProperties(type);
            var count = propertyInfos.Count();
            count = count == 0 ? 1 : count;
            for (int j = 0; j < count; j++)
            {
                var firstObject = selectedConstructor.Invoke(paramValues.ToArray());
                var secondObject = selectedConstructor.Invoke(paramValues.ToArray());
                listOfObjects.Add(new Tuple<object, object, StringBuilder>(firstObject, secondObject, new StringBuilder(
                    string.Format("Building both instances with the same constructor parameters. Constructor used: {0}. ", (selectedConstructor as ConstructorMethod).Constructor.ToString()))));
            }
            return listOfObjects;
        }
    }
}
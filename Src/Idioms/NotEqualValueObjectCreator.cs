using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    internal class NotEqualValueObjectCreator : ValueObjectCreator
    {
        public NotEqualValueObjectCreator(IFixture fixture, IMethodQuery query)
            : base(fixture, query)
        {
        }

        internal override List<List<Tuple<object, object, StringBuilder>>> BuildType(Type type, ISpecimenContext context)
        {
            var listOfLists = new List<List<Tuple<object, object, StringBuilder>>>();
            var constructors = this.Query.SelectMethods(type).ToArray();
            for (int i = 0; i < constructors.Count(); i++)
            {
                var listOfObjects = new List<Tuple<object, object, StringBuilder>>();
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
                        listOfObjects.Add(new Tuple<object, object, StringBuilder>(firstObject, secondObject, new StringBuilder(
                                                                            string.Format(CultureInfo.CurrentCulture, 
                                                                                "Building both instances with the one parameter supplied with different value. Constructor used: {0}, parameter name that was suplied with different value {1} of type {2}.",
                                                                                (constructors[i] as ConstructorMethod).Constructor.ToString(),
                                                                                constructors[i].Parameters.ElementAt(j)
                                                                                               .Name,
                                                                                constructors[i].Parameters.ElementAt(j)
                                                                                               .ParameterType))));
                    }
                }
                listOfLists.Add(listOfObjects);
            }
            return listOfLists;
        }
    }
}
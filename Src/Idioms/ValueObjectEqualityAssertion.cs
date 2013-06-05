using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    public class ValueObjectEqualityAssertion : IdiomaticAssertion
    {
        private readonly IFixture fixture;
        private readonly ISpecimenContext engine;
        private readonly IMethodQuery query;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueObjectEqualityAssertion"/> class.
        /// </summary>
        /// <param name="builder">
        /// A composer which can create instances required to implement the idiomatic unit test,
        /// such as instance of given type to verify if equality works correctly.
        /// </param>
        /// <remarks>
        /// <para>
        /// <paramref name="builder" /> will typically be a <see cref="Fixture" /> instance.
        /// </para>
        /// </remarks>
        public ValueObjectEqualityAssertion(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            this.fixture = fixture;
            engine = new SpecimenContext(this.Fixture);
            query = new GreedyConstructorQuery();
        }


        /// <summary>
        /// Gets the builder supplied via the constructor.
        /// </summary>
        public IFixture Fixture
        {
            get { return this.fixture; }
        }

        public ISpecimenContext Engine
        {
            get { return this.engine; }
        }

        public IMethodQuery Query
        {
            get { return this.query; }
        }

        public override void Verify(System.Type type)
        {
            if(type == null)
                throw new ArgumentNullException("type");

            this.VerifyThatAllPropertiesAreEqual(type);

            this.VerifyThatIfSomePropertiesAreDifferentEqualsIsFalse(type);

            this.VerifyThatIfSomePropertiesAreDifferentUsingSetterEqualsIsFalse(type);
        }

        private void VerifyThatIfSomePropertiesAreDifferentUsingSetterEqualsIsFalse(Type type)
        {
            foreach (var selectConstructor in this.query.SelectMethods(type))
            {
                var paramValues = (from pi in selectConstructor.Parameters
                                   select this.Fixture.Create(pi, this.Engine)).ToList();

                if (paramValues.All(x => !(x is NoSpecimen)))
                {
                    var propertyInfos = this.GetProperties(type).ToArray();
                    int count = propertyInfos.Count();
                    for (int i = 0; i < count; i++)
                    {
                        var firstObject = selectConstructor.Invoke(paramValues.ToArray());
                        var secondObject = selectConstructor.Invoke(paramValues.ToArray());

                        for (int j = 0; j < count; j++)
                        {
                            var propertyValue = this.Fixture.Create(propertyInfos[j].PropertyType, this.Engine);
                            propertyInfos[j].SetValue(firstObject, propertyValue, null);
                            propertyValue = this.Fixture.Create(propertyInfos[j].PropertyType, this.Engine);
                            propertyInfos[j].SetValue(secondObject, propertyValue, null);
                        }

                        CheckEquality(type, firstObject, secondObject, true);
                    }
                }
            }
        }

        private static void CheckEquality(Type type, object firstObject, object secondObject, bool expectedResult)
        {
            bool result;
            var iequatableInterface = GetIEquatableInterface(type);
            if (iequatableInterface != null)
            {
                var objectResult = iequatableInterface.InvokeMember("Equals", BindingFlags.InvokeMethod,
                                                                    null, firstObject,
                                                                    new object[] {secondObject});

                result = objectResult is bool && (bool) objectResult;
            }
            else
            {
                result = firstObject.Equals(secondObject);
            }

            if (result == expectedResult)
                throw new ValueObjectEqualityException();
        }

        private static Type GetIEquatableInterface(Type type)
        {
            var iequatableTypeName = typeof (IEquatable<>).Name;
            var iequatableInterface = type.GetInterface(iequatableTypeName);
            return iequatableInterface;
        }

        private void VerifyThatIfSomePropertiesAreDifferentEqualsIsFalse(Type type)
        {
            foreach (var selectConstructor in this.query.SelectMethods(type))
            {
                var paramValues = (from pi in selectConstructor.Parameters
                                   select this.Fixture.Create(pi, this.Engine)).ToList();

                if (paramValues.All(x => !(x is NoSpecimen)))
                {
                    int count = paramValues.Count;
                    for (int i = 0; i < count; i++)
                    {
                        var firstObject = selectConstructor.Invoke(paramValues.ToArray());
                        paramValues[i] = this.Fixture.Create(paramValues[i].GetType(), this.Engine);
                        var secondObject = selectConstructor.Invoke(paramValues.ToArray());

                        CheckEquality(type, firstObject, secondObject, true);
                    }
                }
            }
        }

        private void VerifyThatAllPropertiesAreEqual(Type type)
        {
            foreach (var selectConstructor in this.query.SelectMethods(type))
            {
                var paramValues = (from pi in selectConstructor.Parameters
                                   select this.Fixture.Create(pi, this.Engine)).ToList();

                if (paramValues.All(x => !(x is NoSpecimen)))
                {
                    var firstObject = selectConstructor.Invoke(paramValues.ToArray());
                    var secondObject = selectConstructor.Invoke(paramValues.ToArray());

                    foreach (var propertyInfo in this.GetProperties(type))
                    {
                        var propertyValue = this.Fixture.Create(propertyInfo.PropertyType, this.Engine);
                        propertyInfo.SetValue(firstObject, propertyValue, null);
                        propertyInfo.SetValue(secondObject, propertyValue, null);
                    }

                    CheckEquality(type, firstObject, secondObject, false);
                }
            }
        }

        private IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return from pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                   where pi.GetSetMethod() != null
                   && pi.GetIndexParameters().Length == 0
                   select pi;
        }
    }
}
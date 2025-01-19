using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.Xunit3.UnitTest.TestTypes
{
    internal abstract class InlineAttributeTestData : IEnumerable<object[]>
    {
        protected static DerivedInlineAutoDataAttribute CreateAttributeWithFakeFixture(
            object[] inlineValues,
            params (string ParameterName, object Value)[] parameters)
        {
            return new DerivedInlineAutoDataAttribute(
                fixtureFactory: () => new DelegatingFixture { OnCreate = OnCreateParameter },
                values: inlineValues);

            object OnCreateParameter(object request, ISpecimenContext context)
            {
                if (request is not ParameterInfo parameterInfo)
                    throw new InvalidOperationException();

                return parameters
                    .Where(x => x.ParameterName == parameterInfo.Name)
                    .Select(x => x.Value).FirstOrDefault();
            }
        }

        protected static DerivedInlineAutoDataAttribute CreateAttribute(
            object[] inlineValues,
            params (string ParameterName, object Value)[] parameters)
        {
            return new DerivedInlineAutoDataAttribute(
                () => new Fixture().Customize(CreateCustomization()),
                inlineValues);

            ICustomization CreateCustomization()
            {
                var builders = parameters
                    .Select(x => new FilteringSpecimenBuilder(
                        builder: new FixedBuilder(x.Value),
                        specification: new ParameterSpecification(
                            new ParameterNameCriterion(x.ParameterName))))
                    .ToList();

                return new DelegatingCustomization
                {
                    OnCustomize = f => f.Customizations
                        .Insert(0, new CompositeSpecimenBuilder(builders))
                };
            }
        }

        public abstract IEnumerator<object[]> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
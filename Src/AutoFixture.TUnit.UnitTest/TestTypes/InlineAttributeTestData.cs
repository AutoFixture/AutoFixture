using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.TUnit.UnitTest.TestTypes;

internal abstract class InlineAttributeTestData<T>
{
    protected static DerivedArgumentsAutoDataAttribute CreateAttributeWithFakeFixture(
        object[] inlineValues,
        params (string ParameterName, object Value)[] parameters)
    {
        return new DerivedArgumentsAutoDataAttribute(
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

    protected static DerivedArgumentsAutoDataAttribute CreateAttribute(
        object[] inlineValues,
        params (string ParameterName, object Value)[] parameters)
    {
        return new DerivedArgumentsAutoDataAttribute(
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

    public abstract IEnumerable<T> GetData();
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Kernel;

/// <summary>
/// A command that assigns anonymous values to all writable properties and fields of a type.
/// </summary>
public class AutoPropertiesCommand : ISpecimenCommand
{
    /// <summary>
    /// Specification that filters properties and files that should be populated.
    /// </summary>
    public IRequestSpecification Specification { get; } = new TrueRequestSpecification();

    /// <summary>
    /// The explicitly specified <see cref="Type"/> that should be used to resolve fields and properties
    /// to populate for the specimen.
    /// <remarks>
    /// <para>
    /// Property will return null if no explicit specimen type was specified during the command construction.
    /// In this case command uses the runtime type of the generated specimen to resolve its fields and properties.
    /// </para>
    /// </remarks>
    /// </summary>
    public Type ExplicitSpecimenType { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AutoPropertiesCommand"/> class.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When created without an explicit specimen type, the <see cref="AutoPropertiesCommand"/>
    /// will infer the specimen type from the actual specimen instance.
    /// </para>
    /// </remarks>
    public AutoPropertiesCommand()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AutoPropertiesCommand"/> class with the
    /// supplied specimen type.
    /// </summary>
    /// <param name="specimenType">The specimen type on which properties are assigned.</param>
    public AutoPropertiesCommand(Type specimenType)
    {
        ExplicitSpecimenType = specimenType ?? throw new ArgumentNullException(nameof(specimenType));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AutoPropertiesCommand"/> class with the
    /// supplied specification.
    /// </summary>
    /// <param name="specification">
    /// A specification that is used as a filter to include properties or fields.
    /// </param>
    /// <remarks>
    /// <para>
    /// Only properties or fields satisfied by <paramref name="specification"/> will get
    /// assigned values.
    /// </para>
    /// </remarks>
    public AutoPropertiesCommand(IRequestSpecification specification)
    {
        Specification = specification ?? throw new ArgumentNullException(nameof(specification));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AutoPropertiesCommand"/> class with the
    /// supplied specimen type and specification.
    /// </summary>
    /// <param name="specimenType">The specimen type on which properties are assigned.</param>
    /// <param name="specification">
    /// A specification that is used as a filter to include properties or fields.
    /// </param>
    /// <remarks>
    /// <para>
    /// Only properties or fields satisfied by <paramref name="specification"/> will get
    /// assigned values.
    /// </para>
    /// </remarks>
    public AutoPropertiesCommand(Type specimenType, IRequestSpecification specification)
    {
        ExplicitSpecimenType = specimenType ?? throw new ArgumentNullException(nameof(specimenType));
        Specification = specification ?? throw new ArgumentNullException(nameof(specification));
    }

    /// <summary>
    /// Assigns anonymous values to properties and fields on a specimen.
    /// </summary>
    public void Execute(object specimen, ISpecimenContext context)
    {
        if (specimen == null) throw new ArgumentNullException(nameof(specimen));
        if (context == null) throw new ArgumentNullException(nameof(context));

        foreach (var pi in GetProperties(specimen))
        {
            var propertyValue = context.Resolve(pi);
            if (propertyValue is not OmitSpecimen)
                pi.SetValue(specimen, propertyValue, null);
        }

        foreach (var fi in GetFields(specimen))
        {
            var fieldValue = context.Resolve(fi);
            if (fieldValue is not OmitSpecimen)
                fi.SetValue(specimen, fieldValue);
        }
    }

    private Type GetSpecimenType(object specimen)
    {
        return ExplicitSpecimenType ?? specimen.GetType();
    }

    private IEnumerable<FieldInfo> GetFields(object specimen)
    {
        return from fi in GetSpecimenType(specimen).GetTypeInfo().GetFields(BindingFlags.Public | BindingFlags.Instance)
            where !fi.IsInitOnly
                  && Specification.IsSatisfiedBy(fi)
            select fi;
    }

    private IEnumerable<PropertyInfo> GetProperties(object specimen)
    {
        return from pi in GetSpecimenType(specimen).GetTypeInfo().GetProperties(BindingFlags.Public | BindingFlags.Instance)
            where pi.GetSetMethod() != null
                  && pi.GetIndexParameters().Length == 0
                  && Specification.IsSatisfiedBy(pi)
            select pi;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Albedo;

namespace AutoFixture.Idioms;

/// <summary>
/// Collections <see cref="NameAndType"/> instances from <see cref="FieldInfoElement"/>,
/// <see cref="ParameterInfoElement"/>, and <see cref="PropertyInfoElement"/>.
/// </summary>
internal class NameAndTypeCollectingVisitor : ReflectionVisitor<IEnumerable<NameAndType>>
{
    private readonly NameAndType[] _values;

    public NameAndTypeCollectingVisitor(
        params NameAndType[] values)
    {
        _values = values;
    }

    public override IEnumerable<NameAndType> Value => _values;

    public override IReflectionVisitor<IEnumerable<NameAndType>> Visit(
        FieldInfoElement fieldInfoElement)
    {
        if (fieldInfoElement == null) throw new ArgumentNullException(nameof(fieldInfoElement));
        var v = new NameAndType(
            fieldInfoElement.FieldInfo.Name,
            fieldInfoElement.FieldInfo.FieldType);
        return new NameAndTypeCollectingVisitor(
            _values.Concat(new[] { v }).ToArray());
    }

    public override IReflectionVisitor<IEnumerable<NameAndType>> Visit(
        ParameterInfoElement parameterInfoElement)
    {
        if (parameterInfoElement == null) throw new ArgumentNullException(nameof(parameterInfoElement));
        var v = new NameAndType(
            parameterInfoElement.ParameterInfo.Name,
            parameterInfoElement.ParameterInfo.ParameterType);
        return new NameAndTypeCollectingVisitor(
            _values.Concat(new[] { v }).ToArray());
    }

    public override IReflectionVisitor<IEnumerable<NameAndType>> Visit(
        PropertyInfoElement propertyInfoElement)
    {
        if (propertyInfoElement == null) throw new ArgumentNullException(nameof(propertyInfoElement));
        var v = new NameAndType(
            propertyInfoElement.PropertyInfo.Name,
            propertyInfoElement.PropertyInfo.PropertyType);
        return new NameAndTypeCollectingVisitor(
            _values.Concat(new[] { v }).ToArray());
    }
}
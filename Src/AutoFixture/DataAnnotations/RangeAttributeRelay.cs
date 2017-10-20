using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.DataAnnotations
{
    /// <summary>
    /// Relays a request for a range number to a <see cref="RangedNumberRequest"/>.
    /// </summary>
    public class RangeAttributeRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new specimen based on a requested range.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A container that can be used to create other specimens.</param>
        /// <returns>
        /// A specimen created from a <see cref="RangedNumberRequest"/> encapsulating the operand
        /// type, the minimum and the maximum of the requested number, if possible; otherwise,
        /// a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (request == null)
            {
                return new NoSpecimen();
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var rangeAttribute = TypeEnvy.GetAttribute<RangeAttribute>(request);
            if (rangeAttribute == null)
            {
                return new NoSpecimen();
            }

            return context.Resolve(Create(rangeAttribute, request));
        }

        [SuppressMessage("Performance", "CA1801:Review unused parameters",
            Justification = "False positive - request property is used. Bug: https://github.com/dotnet/roslyn-analyzers/issues/1294")]
        private static RangedNumberRequest Create(RangeAttribute rangeAttribute, object request)
        {
            Type conversionType;
            switch (request)
            {
                case PropertyInfo pi:
                    conversionType = pi.PropertyType;
                    break;

                case FieldInfo fi:
                    conversionType = fi.FieldType;
                    break;

                default:
                    conversionType = rangeAttribute.OperandType;
                    break;
            }

            Type underlyingType = Nullable.GetUnderlyingType(conversionType);
            if (underlyingType != null)
            {
                conversionType = underlyingType;
            }

            return new RangedNumberRequest(
                conversionType,
                Convert.ChangeType(rangeAttribute.Minimum, conversionType, CultureInfo.CurrentCulture),
                Convert.ChangeType(rangeAttribute.Maximum, conversionType, CultureInfo.CurrentCulture)
                );
        }
    }
}

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
                GetConvertedRangeBoundary(rangeAttribute.Minimum, conversionType),
                GetConvertedRangeBoundary(rangeAttribute.Maximum, conversionType)
            );
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "RangeAttribute",
            Justification = "Workaround for a bug in CA: https://connect.microsoft.com/VisualStudio/feedback/details/521030/")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "OverflowException",
            Justification = "Workaround for a bug in CA: https://connect.microsoft.com/VisualStudio/feedback/details/521030/")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "typeof",
            Justification = "Workaround for a bug in CA: https://connect.microsoft.com/VisualStudio/feedback/details/521030/")]
        private static object GetConvertedRangeBoundary(object attributeValue, Type conversionType)
        {
            try
            {
                return Convert.ChangeType(attributeValue, conversionType, CultureInfo.CurrentCulture);
            }
            catch (OverflowException ex)
            {
                throw new OverflowException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Conversion of RangeAttribute boundary value of {1} type to {2} type caused the overflow " +
                        "exception. Notice, the RangeAttribute type contains the following constructors taking two " +
                        "arguments only:{0}" +
                        "RangeAttribute(int, int){0}" +
                        "RangeAttribute(double, double){0}" +
                        "{0}" +
                        "When you pass a value of other type (e.g. long, decimal), it is converted to either int or " +
                        "double depending on the type. Some conversion (e.g. long to double) could lead to the value " +
                        "distortion due to the double type precision and conversion back to the original type might " +
                        "fail with OverflowException.{0}" +
                        "{0}" +
                        "To solve the issue rather specify the range value that could be safely converted to double " +
                        "and back without overflow or use the constructor overload taking value as a string.{0}" +
                        "{0}" +
                        "Example:{0}" +
                        "RangeAttribute(typeof(long), \"0\", \"9223372036854775807\")",
                        Environment.NewLine,
                        attributeValue.GetType().FullName,
                        conversionType.FullName
                    ),
                    ex);
            }
        }
    }
}

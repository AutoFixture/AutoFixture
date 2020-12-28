using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.DataAnnotations
{
    /// <summary>
    /// Handles a request for a string that matches an Enum data type.
    /// </summary>
    public class EnumDataTypeAttributeRelay : ISpecimenBuilder
    {
        private IRequestMemberTypeResolver requestMemberTypeResolver = new RequestMemberTypeResolver();
        private readonly EnumGenerator enumGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDataTypeAttributeRelay"/> class.
        /// </summary>
        public EnumDataTypeAttributeRelay()
        {
            this.enumGenerator = new EnumGenerator();
        }

        /// <summary>
        /// Gets or sets the current IRequestMemberTypeResolver.
        /// </summary>
        public IRequestMemberTypeResolver RequestMemberTypeResolver
        {
            get => this.requestMemberTypeResolver;
            set => this.requestMemberTypeResolver = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (request == null)
            {
                return new NoSpecimen();
            }

            var enumDataTypeAttribute = TypeEnvy.GetAttribute<EnumDataTypeAttribute>(request);
            if (enumDataTypeAttribute == null)
            {
                return new NoSpecimen();
            }

            if (!this.RequestMemberTypeResolver.TryGetMemberType(request, out var memberType))
            {
                return new NoSpecimen();
            }

            if (!enumDataTypeAttribute.EnumType.GetTypeInfo().IsEnum)
                return new NoSpecimen();

            var enumType = enumDataTypeAttribute.EnumType;
            var enumValue = this.enumGenerator.Create(enumType, context);

            if (enumValue is NoSpecimen)
                return new NoSpecimen();

            if (memberType == typeof(string))
            {
                return enumValue.ToString();
            }

            if (memberType.IsNumberType())
            {
                return Convert.ChangeType(enumValue, memberType, CultureInfo.CurrentCulture);
            }

            return new NoSpecimen();
        }
    }
}

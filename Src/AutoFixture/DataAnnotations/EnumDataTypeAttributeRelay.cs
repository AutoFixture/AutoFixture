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
        private readonly ISpecimenBuilder enumGenerator;
        private IRequestMemberTypeResolver requestMemberTypeResolver = new RequestMemberTypeResolver();

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDataTypeAttributeRelay" /> class.
        /// </summary>
        public EnumDataTypeAttributeRelay()
            : this(new EnumGenerator())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDataTypeAttributeRelay" /> class.
        /// </summary>
        /// <param name="enumGenerator">The <see langword="enum" /> value builder.</param>
        public EnumDataTypeAttributeRelay(ISpecimenBuilder enumGenerator)
        {
            this.enumGenerator = enumGenerator
                ?? throw new ArgumentNullException(nameof(enumGenerator));
        }

        /// <summary>
        /// Gets or sets the current IRequestMemberTypeResolver.
        /// </summary>
        public IRequestMemberTypeResolver RequestMemberTypeResolver
        {
            get => this.requestMemberTypeResolver;
            set => this.requestMemberTypeResolver = value
                ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise a <see cref="NoSpecimen" /> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (request == null) return new NoSpecimen();

            var attribute = TypeEnvy.GetAttribute<EnumDataTypeAttribute>(request);
            if (attribute == null || !attribute.EnumType.GetTypeInfo().IsEnum) return new NoSpecimen();

            var enumValue = this.enumGenerator.Create(attribute.EnumType, context);
            if (enumValue is NoSpecimen) return enumValue;

            if (!this.RequestMemberTypeResolver.TryGetMemberType(request, out var memberType))
            {
                return new NoSpecimen();
            }

            return memberType switch
            {
                var t when t == attribute.EnumType => enumValue,
                var t when t == typeof(object) => enumValue,
                var t when t.IsNumberType() => Convert.ChangeType(enumValue, memberType, CultureInfo.CurrentCulture),
                var t when t == typeof(string) => enumValue.ToString(),
                _ => new NoSpecimen()
            };
        }
    }
}
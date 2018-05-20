using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.DataAnnotations
{
    /// <summary>
    /// Relays a request for a ranged request to a <see cref="RangedRequest"/>.
    /// </summary>
    public class RangeAttributeRelay : ISpecimenBuilder
    {
        private IRequestMemberTypeResolver requestMemberTypeResolver = new RequestMemberTypeResolver();

        /// <summary>
        /// Gets or sets the current IRequestMemberTypeResolver.
        /// </summary>
        public IRequestMemberTypeResolver RequestMemberTypeResolver
        {
            get => this.requestMemberTypeResolver;
            set => this.requestMemberTypeResolver = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Creates a new specimen based on a requested range.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A container that can be used to create other specimens.</param>
        /// <returns>
        /// A specimen created from a <see cref="RangedRequest"/> encapsulating the operand
        /// type, the minimum and the maximum of the requested value, if possible; otherwise,
        /// a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var rangeAttribute = TypeEnvy.GetAttribute<RangeAttribute>(request);
            if (rangeAttribute == null)
            {
                return new NoSpecimen();
            }

            var memberType = this.GetMemberType(rangeAttribute, request);
            var rangedRequest =
                new RangedRequest(
                    memberType,
                    rangeAttribute.OperandType,
                    rangeAttribute.Minimum,
                    rangeAttribute.Maximum);

            return context.Resolve(rangedRequest);
        }

        private Type GetMemberType(RangeAttribute rangeAttribute, object request)
        {
            if (this.RequestMemberTypeResolver.TryGetMemberType(request, out var memberType))
            {
                return memberType;
            }

            return Nullable.GetUnderlyingType(rangeAttribute.OperandType) ?? rangeAttribute.OperandType;
        }
    }
}
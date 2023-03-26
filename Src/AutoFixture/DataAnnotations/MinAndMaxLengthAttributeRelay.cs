using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using AutoFixture.Kernel;

namespace AutoFixture.DataAnnotations
{
    /// <summary>
    /// Relays a request for a constrained string to a <see cref="ConstrainedStringRequest"/>.
    /// </summary>
    public class MinAndMaxLengthAttributeRelay : ISpecimenBuilder
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
        /// Creates a new specimen based on a specified minimum or maximum length of characters that are allowed.
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
            if (context is null) throw new ArgumentNullException(nameof(context));

            if (!this.RequestMemberTypeResolver.TryGetMemberType(request, out var memberType))
            {
                return new NoSpecimen();
            }

            var range = Range.From(request);
            if (!range.IsConstrained)
            {
                return new NoSpecimen();
            }

            if (memberType == typeof(string))
            {
                return context.Resolve(range.ToStringRequest());
            }

            if (!memberType.IsArray)
            {
                return new NoSpecimen();
            }

            var elementType = memberType.GetElementType();
            var result = context.Resolve(range.ToSequenceRequest(elementType));
            if (result is not IEnumerable seqResult)
            {
                return new NoSpecimen();
            }

            return seqResult.ToTypedArray(elementType);
        }

        private class Range
        {
            private const int FallbackMin = 1;

            public Range(int? min, int? max)
            {
                this.Min = min;
                this.Max = max;
                this.IsConstrained = min.HasValue || max.HasValue;
            }

            public int? Min { get; }

            public int? Max { get; }

            public bool IsConstrained { get; }

            public ConstrainedStringRequest ToStringRequest()
            {
                var min = this.Min ?? 0;
                var max = this.Max ?? min * 2;
                min = min == 0 && max != 0 ? FallbackMin : min;
                max = max < 0 ? int.MaxValue : max;
                return new ConstrainedStringRequest(min, max);
            }

            public RangedSequenceRequest ToSequenceRequest(Type elementType)
            {
                var min = this.Min ?? 0;
                var max = this.Max ?? min * 2;
                min = min == 0 && max != 0 ? FallbackMin : min;
                const int fallbackMax = 3;
                max = max < 0 ? fallbackMax : max;
                return new RangedSequenceRequest(elementType, min, max);
            }

            public static Range From(object request)
            {
                var minLengthAttribute = TypeEnvy.GetAttribute<MinLengthAttribute>(request);
                var maxLengthAttribute = TypeEnvy.GetAttribute<MaxLengthAttribute>(request);

                return new Range(minLengthAttribute?.Length, maxLengthAttribute?.Length);
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (!Range.TryGetFromAttributes(request, out var range))
            {
                return new NoSpecimen();
            }

            if (!this.RequestMemberTypeResolver.TryGetMemberType(request, out var memberType))
            {
                return new NoSpecimen();
            }

            if (memberType == typeof(string))
            {
                return ResolveString(context, range);
            }

            if (memberType.IsArray)
            {
                return ResolveArray(context, memberType, range);
            }

            return new NoSpecimen();
        }

        private static object ResolveString(ISpecimenContext context, Range range)
        {
            return context.Resolve(new ConstrainedStringRequest(range.Min, range.Max));
        }

        private static object ResolveArray(ISpecimenContext context, Type arrayType, Range range)
        {
            var elementType = arrayType.GetElementType();

            var result = context.Resolve(new RangedSequenceRequest(elementType, range.Min, range.Max));
            if (result is IEnumerable seqResult)
                return seqResult.ToTypedArray(elementType);

            return new NoSpecimen();
        }

        private class Range
        {
            public int Min { get; }

            public int Max { get; }

            private Range(int min, int max)
            {
                this.Min = min;
                this.Max = max;
            }

            public static bool TryGetFromAttributes(object request, out Range range)
            {
                var minLengthAttribute = TypeEnvy.GetAttribute<MinLengthAttribute>(request);
                var maxLengthAttribute = TypeEnvy.GetAttribute<MaxLengthAttribute>(request);

                if (minLengthAttribute == null && maxLengthAttribute == null)
                {
                    range = null;
                    return false;
                }

                range = GetRange(minLengthAttribute, maxLengthAttribute);
                return true;
            }

            private static Range GetRange(MinLengthAttribute minLengthAttribute, MaxLengthAttribute maxLengthAttribute)
            {
                var min = minLengthAttribute?.Length ?? 0;
                var max = maxLengthAttribute?.Length ?? min * 2;

                // To avoid creation of empty strings/arrays.
                if (max > 0 && min == 0)
                {
                    min = 1;
                }

                return new Range(min, max);
            }
        }
    }
}
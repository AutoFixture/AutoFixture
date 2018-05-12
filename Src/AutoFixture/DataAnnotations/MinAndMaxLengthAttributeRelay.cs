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
        private readonly IMemberTypeResolver memberTypeResolver;

        /// <summary>
        /// Creates a new instance of MinAndMaxLengthAttributeRelay
        /// </summary>
        /// <param name="memberTypeResolver">MemberTypeResolver to determine member type of requested object</param>
        public MinAndMaxLengthAttributeRelay(IMemberTypeResolver memberTypeResolver)
        {
            this.memberTypeResolver = memberTypeResolver;
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
            if (request == null)
            {
                return new NoSpecimen();
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var range = Range.TryGetFromAttributes(request);
            
            if (range == null)
            {
                return new NoSpecimen();
            }

            var memberType = memberTypeResolver.TryGetMemberType(request);

            if (IsString(memberType))
            {
                return ResolveString(context, range);
            }

            if (IsArray(memberType))
            {
                return ResolveArray(context, memberType, range);
            }

            return new NoSpecimen();
        }

        private static bool IsString(Type memberType)
        {
            return memberType == typeof(string);
        }

        private static bool IsArray(Type memberType)
        {
            return memberType?.IsArray ?? false;
        }
        
       private static object ResolveString(ISpecimenContext context, Range range)
        {
            return context.Resolve(new ConstrainedStringRequest(range.Min, range.Max));
        }

        private static object ResolveArray(ISpecimenContext context, Type memberType, Range range)
        {
            var elementType = memberType.GetElementType();

            var collectionCount = GetRandomNumberWithinRange(context, range);

            if (collectionCount is NoSpecimen)
                return new NoSpecimen();

            var resultCollection = GetRandomCollection(context, elementType, collectionCount);

            return ToArray((IEnumerable)resultCollection, elementType);
        }

        private static object GetRandomNumberWithinRange(ISpecimenContext context, Range range)
        {
            return context.Resolve(new RangedNumberRequest(typeof(int), range.Min, range.Max));
        }

        private static object GetRandomCollection(ISpecimenContext context, Type elementType, object collectionCount)
        {
            return context.Resolve(new FiniteSequenceRequest(elementType, (int)collectionCount));
        }

        private static object ToArray(IEnumerable elements, Type elementType)
        {
            var collection = elements as ICollection;
            var count = (collection != null) ? collection.Count : elements.Cast<object>().Count();
            var array = Array.CreateInstance(elementType, count);
            var index = 0;

            foreach (var element in elements)
            {
                array.SetValue(element, index);
                index++;
            }

            return array;
        }

        private class Range
        {
            public int Min { get; }
            public int Max { get; }

            private Range(int min, int max)
            {
                Min = min;
                Max = max;
            }

            public static Range TryGetFromAttributes(object request)
            {
                var minLengthAttribute = TypeEnvy.GetAttribute<MinLengthAttribute>(request);
                var maxLengthAttribute = TypeEnvy.GetAttribute<MaxLengthAttribute>(request);

                return minLengthAttribute == null && maxLengthAttribute == null
                    ? null
                    : GetRange(minLengthAttribute, maxLengthAttribute);
            }

            private static Range GetRange(MinLengthAttribute minLengthAttribute, MaxLengthAttribute maxLengthAttribute)
            {
                var min = minLengthAttribute?.Length ?? 0;
                var max = maxLengthAttribute?.Length ?? Int32.MaxValue;

                //to avoid creation of empty strings/arrays
                if (max > 0 && min == 0)
                {
                    min = 1;
                }

                return new Range(min, max);
            }
        }
    }
}
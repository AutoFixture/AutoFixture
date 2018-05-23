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
            get => requestMemberTypeResolver;
            set => requestMemberTypeResolver = value ?? throw new ArgumentNullException(nameof(value));
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

            if(!Range.TryGetFromAttributes(request, out var range))
            {
                return new NoSpecimen();
            }

            if (!this.RequestMemberTypeResolver.TryGetMemberType(request, out var memberType))
            {
                return new NoSpecimen();
            }

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

        private static object ResolveArray(ISpecimenContext context, Type arrayType, Range range)
        {
            if (!TryGetRandomNumberWithinRange(context, range, out var collectionCount))
            {
                return new NoSpecimen();
            }

            var elementType = arrayType.GetElementType();

            if (!TryGetRandomCollection(context, elementType, collectionCount, out var randomCollection))
            {
                return new NoSpecimen();
            }
            
            return ToArray(randomCollection, elementType);
        }

        private static bool TryGetRandomNumberWithinRange(ISpecimenContext context, Range range, out int randomNumber)
        {
            var result = context.Resolve(new RangedNumberRequest(typeof(int), range.Min, range.Max));

            if (result is int number)
            {
                randomNumber = number;
                return true;
            }

            randomNumber = default(int);
            return false;
        }

        private static bool TryGetRandomCollection(ISpecimenContext context, Type elementType, int collectionCount, out IEnumerable randomCollection)
        {
            var result = context.Resolve(new FiniteSequenceRequest(elementType, collectionCount));

            if (result is IEnumerable collection)
            {
                randomCollection = collection;
                return true;
            }

            randomCollection = null;
            return false;
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
                var max = maxLengthAttribute?.Length ?? Int32.MaxValue;

                // To avoid creation of empty strings/arrays
                if (max > 0 && min == 0)
                {
                    min = 1;
                }

                return new Range(min, max);
            }
        }
    }
}
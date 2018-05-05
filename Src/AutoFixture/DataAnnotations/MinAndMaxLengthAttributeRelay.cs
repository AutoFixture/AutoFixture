using System;
using System.ComponentModel.DataAnnotations;
using AutoFixture.Kernel;

namespace AutoFixture.DataAnnotations
{
    /// <summary>
    /// Relays a request for a constrained string to a <see cref="ConstrainedStringRequest"/>.
    /// </summary>
    public class MinAndMaxLengthAttributeRelay : ISpecimenBuilder
    {
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

            var minLengthAttribute = TypeEnvy.GetAttribute<MinLengthAttribute>(request);
            var maxLengthAttribute = TypeEnvy.GetAttribute<MaxLengthAttribute>(request);

            if (maxLengthAttribute == null && minLengthAttribute == null)
            {
                return new NoSpecimen();
            }

            var minLength = minLengthAttribute?.Length ?? 0;
            var maxLength = maxLengthAttribute?.Length ?? Int32.MaxValue;

            return context.Resolve(new ConstrainedStringRequest(minLength, maxLength));
        }
    }
}
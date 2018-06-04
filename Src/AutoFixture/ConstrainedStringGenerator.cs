using System;
using System.Text;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Creates a constrained string.
    /// </summary>
    public class ConstrainedStringGenerator : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a constrained string based on a ConstrainedStringRequest.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The requested number if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var constrain = request as ConstrainedStringRequest;
            if (constrain == null)
            {
                return new NoSpecimen();
            }

            return Create(constrain.MinimumLength, constrain.MaximumLength, context);
        }

        private static string Create(int minimumLength, int maximumLength, ISpecimenContext context)
        {
            var sb = new StringBuilder();

            do
            {
                sb.Append(context.Resolve(typeof(string)));
            }
            while (sb.Length < minimumLength);

            if (sb.Length > maximumLength)
            {
                return sb.ToString().Substring(0, maximumLength);
            }

            return sb.ToString();
        }
    }
}
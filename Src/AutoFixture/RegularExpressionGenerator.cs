using System;
using System.Text.RegularExpressions;
using AutoFixture.Kernel;
using Fare;

namespace AutoFixture
{
    /// <summary>
    /// Creates a string that is guaranteed to match a RegularExpressionRequest.
    /// </summary>
    public class RegularExpressionGenerator : ISpecimenBuilder
    {
        private readonly Random random;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegularExpressionGenerator"/> class.
        /// </summary>
        public RegularExpressionGenerator()
        {
            this.random = new Random();
        }

        /// <summary>
        /// Creates a string that is guaranteed to match a RegularExpressionRequest.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (request == null) return new NoSpecimen();

            var regularExpressionRequest = request as RegularExpressionRequest;
            if (regularExpressionRequest == null)
            {
                return new NoSpecimen();
            }

            return this.GenerateRegularExpression(regularExpressionRequest);
        }

        private object GenerateRegularExpression(RegularExpressionRequest request)
        {
            string pattern = request.Pattern;

            try
            {
                // Use the Xeger constructor overload that that takes an instance of Random.
                // Otherwise identically strings can be generated, if regex are generated within short time.
                string regex = new Xeger(pattern, this.random).Generate();
                if (Regex.IsMatch(regex, pattern))
                {
                    return regex;
                }
            }
            catch (InvalidOperationException)
            {
                return new NoSpecimen();
            }
            catch (ArgumentException)
            {
                return new NoSpecimen();
            }

            return new NoSpecimen();
        }
    }
}

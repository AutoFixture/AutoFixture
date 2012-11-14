﻿using System;
using System.Text.RegularExpressions;
using Ploeh.AutoFixture.DataAnnotations;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates a string that is guaranteed to match a RegularExpressionRequest.
    /// </summary>
    public class RegularExpressionGenerator : ISpecimenBuilder
    {
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
            if (request == null)
            {
                return new NoSpecimen();
            }

            var regularExpressionRequest = request as RegularExpressionRequest;
            if (regularExpressionRequest == null)
            {
                return new NoSpecimen(request);
            }

            return RegularExpressionGenerator.CreateAnonymous(regularExpressionRequest);
        }

        private static object CreateAnonymous(RegularExpressionRequest request)
        {
            string pattern = request.Pattern;

            try
            {
                string regex = new Xeger(pattern).Generate();
                if (Regex.IsMatch(regex, pattern, RegexOptions.Singleline))
                {
                    return regex;
                }
            }
            catch (InvalidOperationException)
            {
                return new NoSpecimen(request);
            }
            catch (ArgumentException)
            {
                return new NoSpecimen(request);
            }

            return new NoSpecimen(request);
        }
    }
}
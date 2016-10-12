﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.DataAnnotations
{
    /// <summary>
    /// Relays a request for a constrained string to a <see cref="ConstrainedStringRequest"/>.
    /// </summary>
    public class MinLengthAttributeRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new specimen based on a specified length of characters that are allowed.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A container that can be used to create other specimens.</param>
        /// <returns>
        /// A specimen created from a <see cref="ConstrainedStringRequest"/> encapsulating the 
        /// minimum and the maximum of the requested number, if possible; otherwise,
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

            var customAttributeProvider = request as ICustomAttributeProvider;
            if (customAttributeProvider == null)
            {
                return new NoSpecimen();
            }

            var minLengthAttribute = customAttributeProvider.GetCustomAttributes(typeof(MinLengthAttribute), inherit: true).Cast<MinLengthAttribute>().SingleOrDefault();
            if (minLengthAttribute == null)
            {
                return new NoSpecimen();
            }

            return context.Resolve(new ConstrainedStringRequest(minLengthAttribute.Length, minLengthAttribute.Length));
        }
    }
}

﻿using System;
using AutoFixture.Kernel;

namespace AutoFixture.AutoNSubstitute
{
    /// <summary>
    /// Creates a substitute in response to the <see cref="SubstituteRequest"/>.
    /// </summary>
    /// <remarks>
    /// This class serves as a fixture customization, responding to explicit substitute requests in the form of the
    /// <see cref="SubstituteRequest"/> instances. The actual construction of the substitute is delegated to a
    /// substitute factory, responsible for invoking an appropriate constructor for the target type with
    /// automatically generated arguments.
    /// </remarks>
    public class SubstituteRequestHandler : ISpecimenBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubstituteRequestHandler"/> class with the specified
        /// <paramref name="substituteFactory"/>.
        /// </summary>
        /// <param name="substituteFactory">
        /// An <see cref="ISpecimenBuilder"/> responsible for creating a substitute instance from a requested
        /// <see cref="Type"/>.
        /// </param>
        public SubstituteRequestHandler(ISpecimenBuilder substituteFactory)
        {
            this.SubstituteFactory = substituteFactory ?? throw new ArgumentNullException(nameof(substituteFactory));
        }

        /// <summary>
        /// Returns an <see cref="ISpecimenBuilder"/> responsible for creating a substitute instance based on a target
        /// <see cref="Type"/> type.
        /// </summary>
        public ISpecimenBuilder SubstituteFactory { get; }

        /// <summary>
        /// Creates a substitute when <paramref name="request"/> is an explicit <see cref="SubstituteRequest"/>.
        /// </summary>
        /// <returns>
        /// A substitute created by the <see cref="SubstituteFactory"/> when <paramref name="request"/> is a
        /// <see cref="SubstituteRequest"/> or <see cref="NoSpecimen"/> for all other requests.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            var substituteRequest = request as SubstituteRequest;
            if (substituteRequest == null)
            {
                return new NoSpecimen();
            }

            return this.SubstituteFactory.Create(substituteRequest.TargetType, context);
        }
    }
}

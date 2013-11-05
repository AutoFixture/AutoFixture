﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Decorates an <see cref="ISpecimenBuilder" /> with a
    /// <see cref="RecursionGuard" />.
    /// </summary>
    public class OmitOnRecursionBehavior : ISpecimenBuilderTransformation
    {
        private const int DefaultRecursionDepth = 1;
        private readonly int recursionDepth;
        /// <summary>
        /// Initializes new instance of the <see cref="OmitOnRecursionBehavior" /> class with default recursion depth.
        /// The default recursion depth will omit assignment on first recursion.
        /// </summary>
        public OmitOnRecursionBehavior() : this(DefaultRecursionDepth)
        {            
        }

        /// <summary>
        /// Initializes new instance of the <see cref="OmitOnRecursionBehavior" /> class with specific recursion depth.
        /// </summary>
        /// <param name="recursionDepth">The recursion depth at which the request will be omitted.</param>
        public OmitOnRecursionBehavior(int recursionDepth)
        {
            this.recursionDepth = recursionDepth;
        }

        /// <summary>
        /// Decorates the supplied <see cref="ISpecimenBuilder" /> with an
        /// <see cref="RecursionGuard"/>.
        /// </summary>
        /// <param name="builder">The builder to decorate.</param>
        /// <returns>
        /// <paramref name="builder" /> decorated with an
        /// <see cref="RecursionGuard" />.
        /// </returns>
        public ISpecimenBuilder Transform(ISpecimenBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");

            return new RecursionGuard(builder, new OmitOnRecursionHandler(), recursionDepth);
        }
    }
}

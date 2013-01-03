﻿using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    /// <summary>Provides pre- and post-condition checks for requests for substituted instances.</summary>
    /// <seealso cref="Create(object, ISpecimenContext)"/>
    public class NSubstituteBuilder : ISpecimenBuilder
    {
        private readonly ISpecimenBuilder builder;
        private readonly Func<Type, bool> substitutionSpecification;

        /// <summary>Initializes a new instance of the <see cref="NSubstituteBuilder"/> class with an
        ///     <see cref="ISpecimenBuilder"/> to decorate.</summary>
        /// <param name="builder">The builder which must build mock instances.</param>
        /// <remarks>
        ///     <paramref name="builder"/> is subsequently available through the <see cref="Builder"/> property.
        /// </remarks>
        /// <seealso cref="Builder"/>
        public NSubstituteBuilder(ISpecimenBuilder builder)
            : this(builder, t => t != null && (t.IsInterface || t.IsAbstract))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="NSubstituteBuilder"/> class with an
        ///     <see cref="ISpecimenBuilder"/> to decorate.</summary>
        /// <param name="builder">The builder which must build mock instances.</param>
        /// <param name="substitutionSpecification">A specification that determines whether a substitute should be created for a given type or not.</param>
        /// <remarks>
        ///     <para><paramref name="builder"/> is subsequently available through the <see cref="Builder"/> property.</para>
        ///     <para><paramref name="substitutionSpecification"/> is subsequently available through the <see cref="SubstitutionSpecification"/> property.</para>
        /// </remarks>
        /// <seealso cref="Builder"/>
        /// <seealso cref="SubstitutionSpecification"/>
        public NSubstituteBuilder(ISpecimenBuilder builder, Func<Type, bool> substitutionSpecification)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");
            if (substitutionSpecification == null)
                throw new ArgumentNullException("substitutionSpecification");

            this.builder = builder;
            this.substitutionSpecification = substitutionSpecification;
        }

        /// <summary>Gets the decorated builder supplied through the constructor.</summary>
        /// <seealso cref="NSubstituteBuilder(ISpecimenBuilder)"/>
        public ISpecimenBuilder Builder
        {
            get { return builder; }
        }

        /// <summary>Gets a specification that determines whether a substitute should be created for a given type.</summary>
        /// <remarks>
        ///     <para>
        ///         By default it only returns <see langword="true"/> for interfaces and abstract classes, but a different specification can be supplied by using the
        ///         <see cref="NSubstituteBuilder(ISpecimenBuilder, Func{Type, bool})"/> overloaded constructor that takes a specification as input. In that case, this
        ///         property returns the specification supplied to the constructor.
        ///     </para>
        /// </remarks>
        /// <seealso cref="NSubstituteBuilder(ISpecimenBuilder, Func{Type, bool})"/>
        public Func<Type, bool> SubstitutionSpecification
        {
            get { return substitutionSpecification; }
        }

        /// <summary>Creates a new specimen based on a request.</summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.</returns>
        /// <remarks>
        ///     If the request is not a <see cref="Type"/> or the request does not represent an interface or an abstract class, this method returns a new
        ///     <see cref="NoSpecimen"/>; otherwise, it returns a substitute for the requested type.
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            var type = request as Type;
            if (!ShouldBeSubstituted(type))
                return new NoSpecimen(request);

            var substitute = Builder.Create(request, context);
            if (substitute == null)
                return new NoSpecimen(request);

            return substitute;
        }

        private bool ShouldBeSubstituted(Type type)
        {
            return SubstitutionSpecification(type);
        }
    }
}

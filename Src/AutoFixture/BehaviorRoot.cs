﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// A marker class, used to explicitly identify the root of the
    /// <i>behaviors</i> role in an <see cref="ISpecimenBuilderNode" /> graph.
    /// </summary>
    /// <remarks>
    /// The only purpose of this class is to act as an easily identifiable
    /// container. This makes it easier to find the root of the of
    /// <i>behaviors</i> even if it is buried deep in a larger graph.
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "The main responsibility of this class isn't to be a 'collection' (which, by the way, it isn't - it's just an Iterator).")]
    public class BehaviorRoot : ISpecimenBuilderNode
    {
        private readonly ISpecimenBuilder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="BehaviorRoot" />
        /// class.
        /// </summary>
        /// <param name="builder">The builder to decorate.</param>
        /// <remarks>
        /// <para>
        /// After initialization <paramref name="builder" /> is available via
        /// the <see cref="Builder" /> property.
        /// </para>
        /// </remarks>
        /// <seealso cref="Builder" />
        public BehaviorRoot(ISpecimenBuilder builder)
        {
            this.builder = builder;
        }

        /// <summary>Composes the supplied builders.</summary>
        /// <param name="builders">The builders to compose.</param>
        /// <returns>
        /// A new <see cref="ISpecimenBuilderNode" /> instance containing
        /// <paramref name="builders" /> as child nodes.
        /// </returns>
        public ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            var composedBuilder = 
                CompositeSpecimenBuilder.ComposeIfMultiple(builders);
            return new BehaviorRoot(composedBuilder);
        }

        /// <summary>Creates a new specimen based on a request.</summary>
        /// <param name="request">
        /// The request that describes what to create.
        /// </param>
        /// <param name="context">
        /// A context that can be used to create other specimens.
        /// </param>
        /// <returns>
        /// The requested specimen if possible; otherwise a
        /// <see cref="NoSpecimen" /> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <paramref name="request" /> can be any object, but will often
        /// be a <see cref="Type" /> or other
        /// <see cref="System.Reflection.MemberInfo" /> instances.
        /// </para>
        /// <para>
        /// Note to implementers: Implementations are expected to return a
        /// <see cref="NoSpecimen" /> instance if they can't satisfy the
        /// request.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            return this.builder.Create(request, context);
        }

        /// <summary>Returns the decorated builder as a sequence.</summary>
        /// <returns><see cref="Builder" /> wrappen in an Iterator.</returns>
        /// <seealso cref="Builder" />
        public IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            yield return this.builder;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can
        /// be used to iterate through the collection.
        /// </returns>
        /// <seealso cref="GetEnumerator()" />
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>Gets the builder decorated by this instance.</summary>
        /// <value>The builder originally supplied via the constructor.</value>
        /// <seealso cref="BehaviorRoot(ISpecimenBuilder)" />
        public ISpecimenBuilder Builder
        {
            get { return this.builder; }
        }
    }
}

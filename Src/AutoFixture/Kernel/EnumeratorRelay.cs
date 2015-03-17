﻿using System;
using System.Collections.Generic;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Relays a request for <see cref="IEnumerator{T}" /> to 
    /// <see cref="IEnumerable{T}"/> and converts the result to an enumerator
    /// of a sequence of the requested type.
    /// </summary>
    public class EnumeratorRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new enumerator based on a request.
        /// </summary>
        /// <param name="request">
        /// The request that describes what to create.
        /// </param>
        /// <param name="context">
        /// A context that can be used to create other specimens.
        /// </param>
        /// <returns>
        /// An enumerator of the requested type if possible; otherwise a 
        /// <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If <paramref name="request"/> is a request for an 
        /// <see cref="IEnumerator{T}"/> and <paramref name="context"/> can
        /// satisfy an <see cref="IEnumerable{T}"/> request for the
        /// element type, the return value is an enumerator of the
        /// requested type. If not, the return value is a
        /// <see cref="NoSpecimen"/> instance.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var t = request as Type;
            if (t == null)
                return new NoSpecimen(request);

            var typeArguments = t.GetGenericArguments();
            if (typeArguments.Length != 1 ||
                typeof (IEnumerator<>) != t.GetGenericTypeDefinition())
                return new NoSpecimen(request);

            var specimenBuilder = (ISpecimenBuilder) Activator.CreateInstance(
                typeof (EnumeratorRelay<>).MakeGenericType(typeArguments));
            return specimenBuilder.Create(request, context);
        }
    }

    internal class EnumeratorRelay<T> : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var t = request as Type;
            if (t == null)
                return new NoSpecimen(request);

            if (t != typeof(IEnumerator<T>))
                return new NoSpecimen(request);

            var enumerable =
                context.Resolve(typeof (IEnumerable<T>)) as IEnumerable<T>;

            if (enumerable == null)
                return new NoSpecimen(request);

            return enumerable.GetEnumerator();
        }
    }
}


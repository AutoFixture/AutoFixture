﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Relays requests for enumerable parameters, and returns an empty array
    /// if the object returned from the context is an
    /// <see cref="OmitSpecimen" /> instance.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <strong>OmitEnumerableParameterRequestRelay</strong> class works
    /// like <see cref="ParameterRequestRelay" />, except that it only handles
    /// <see cref="ParameterInfo" /> instances where
    /// <see cref="ParameterInfo.ParameterType" /> is
    /// <see cref="IEnumerable{T}" />. If the value returned from the context
    /// is an <see cref="OmitSpecimen" /> instance, it returns an empty array.
    /// </para>
    /// </remarks>
    /// <seealso cref="OmitEnumerableParameterRequestRelay.Create(object, ISpecimenContext)" />
    /// <seealso cref="ParameterRequestRelay" />
    /// <seealso cref="OmitArrayParameterRequestRelay" />
    /// <seealso cref="OmitSpecimen" />
    public class OmitEnumerableParameterRequestRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a specimen based on a requested enumerable parameter.
        /// </summary>
        /// <param name="request">
        /// The request that describes what to create.
        /// </param>
        /// <param name="context">
        /// A context that can be used to create other specimens.
        /// </param>
        /// <returns>
        /// A specimen created from a <see cref="SeededRequest"/> encapsulating
        /// the parameter type and name of the requested parameter, if
        /// possible; otherwise, a <see cref="NoSpecimen" />.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method only handles <see cref="ParameterInfo" /> instances
        /// where <see cref="ParameterInfo.ParameterType" /> is
        /// <see cref="IEnumerable{T}" />.  If <paramref name="context" />
        /// returns an <see cref="OmitSpecimen" /> instance, an empty array of
        /// the correct type is returned instead.
        /// </para>
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="context" /> is <see langword="null"/>
        /// </exception>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var pi = request as ParameterInfo;
            if (pi == null)
#pragma warning disable 618
                return new NoSpecimen(request);
#pragma warning restore 618

            if (!pi.ParameterType.IsGenericType)
#pragma warning disable 618
                return new NoSpecimen(request);
#pragma warning restore 618

            if (IsNotEnumerable(pi))
#pragma warning disable 618
                return new NoSpecimen(request);
#pragma warning restore 618

            var returnValue = context.Resolve(
                new SeededRequest(
                    pi.ParameterType,
                    pi.Name));

            if (returnValue is OmitSpecimen)
                return Array.CreateInstance(
                    pi.ParameterType.GetGenericArguments().Single(),
                    0);

            return returnValue;
        }

        private static bool IsNotEnumerable(ParameterInfo pi)
        {
            var openGenericType = pi.ParameterType.GetGenericTypeDefinition();
            return openGenericType != typeof(IEnumerable<>);
        }
    }
}

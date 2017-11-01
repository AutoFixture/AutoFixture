using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Creates a new instance of the requested type by invoking the first method it can
    /// satisfy.
    /// </summary>
    public class MethodInvoker : ISpecimenBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodInvoker"/> class with the supplied
        /// <see cref="IMethodQuery" />.
        /// </summary>
        /// <param name="query">
        /// The <see cref="IMethodQuery"/> that defines which methods are attempted.
        /// </param>
        public MethodInvoker(IMethodQuery query)
        {
            this.Query = query ?? throw new ArgumentNullException(nameof(query));
        }

        /// <summary>
        /// Gets the <see cref="IMethodQuery"/> that defines which constructors will be
        /// attempted.
        /// </summary>
        public IMethodQuery Query { get; }

        /// <summary>
        /// Creates a specimen of the requested type by invoking the first constructor or method it
        /// can satisfy.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// A specimen generated from a method of the requested type, if possible;
        /// otherwise, <see langword="null"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method uses the first constructor or method returned by <see cref="Query"/> where
        /// <paramref name="context"/> can create values for all parameters.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            foreach (var ci in this.GetConstructors(request))
            {
                var paramValues = (from pi in ci.Parameters
                                   select context.Resolve(pi)).ToList();

                if (paramValues.All(IsValueValid))
                {
                    return ci.Invoke(paramValues.ToArray());
                }
            }

            return new NoSpecimen();
        }

        private IEnumerable<IMethod> GetConstructors(object request)
        {
            var requestedType = request as Type;
            if (requestedType == null)
            {
                return Enumerable.Empty<IMethod>();
            }

            return this.Query.SelectMethods(requestedType);
        }

        private static bool IsValueValid(object value)
        {
            return !(value is NoSpecimen)
                && !(value is OmitSpecimen);
        }
    }
}

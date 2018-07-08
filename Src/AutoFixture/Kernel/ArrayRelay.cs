using System;
using System.Collections;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Relays a request for an array to a <see cref="MultipleRequest"/> and converts the result
    /// to the desired array type.
    /// </summary>
    public class ArrayRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new array based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// An array of the requested type if possible; otherwise a <see cref="NoSpecimen"/>
        /// instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If <paramref name="request"/> is a request for an array and <paramref name="context"/>
        /// can satisfy a <see cref="MultipleRequest"/> for the element type, the return value is a
        /// populated array of the requested type. If not, the return value is a
        /// <see cref="NoSpecimen"/> instance.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            // This is performance-sensitive code when used repeatedly over many requests.
            // See discussion at https://github.com/AutoFixture/AutoFixture/pull/218
            var type = request as Type;
            if (type == null)
                return new NoSpecimen();

            if (!type.IsArray)
                return new NoSpecimen();

            var elementType = type.GetElementType();
            var specimen = context.Resolve(new MultipleRequest(elementType));
            if (specimen is OmitSpecimen)
                return specimen;

            var elements = specimen as IEnumerable;
            if (elements == null)
                return new NoSpecimen();

            return elements.ToTypedArray(elementType);
        }
    }
}

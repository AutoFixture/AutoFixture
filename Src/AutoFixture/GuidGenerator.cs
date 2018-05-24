using System;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Creates new <see cref="Guid"/> instances.
    /// </summary>
    public class GuidGenerator : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new <see cref="Guid"/> instance.
        /// </summary>
        [Obsolete("Please use the Create(request, context) method as this overload will be removed to make API uniform.")]
        public static Guid Create()
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// Creates a new <see cref="Guid"/> instance.
        /// </summary>
        [Obsolete("Please move over to using Create() as this method will be removed in the next release", true)]
        public static Guid CreateAnonymous()
        {
            return Create();
        }

        /// <inheritdoc />
        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(Guid).Equals(request))
            {
                return new NoSpecimen();
            }

#pragma warning disable 618
            return Create();
#pragma warning restore 618
        }
    }
}

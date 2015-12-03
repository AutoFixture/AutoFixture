using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates new <see langword="struct"/>.
    /// </summary>
    public class MutableValueTypeGenerator : ISpecimenBuilder
    {
        private IRequestSpecification valueTypeWithoutConstructorsSpecification;

        /// <summary>
        /// Creates new instance.
        /// </summary>
        public MutableValueTypeGenerator()
        {
            valueTypeWithoutConstructorsSpecification = new AndRequestSpecification(new ValueTypeSpecification(),
                                                                                    new NoConstructorsSpecification());

        }

        /// <summary>
        /// Creates a new <see langword="struct"/>.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens. Not used</param>
        /// <returns>
        /// The requested struct if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            Type type = request as Type;
            if (type == null || !valueTypeWithoutConstructorsSpecification.IsSatisfiedBy(type))
            {
#pragma warning disable 618
                return new NoSpecimen(request);
#pragma warning restore 618
            }

            return Activator.CreateInstance(type);
        }
    }
}
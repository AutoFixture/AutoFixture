using System;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Creates new <see langword="struct"/>.
    /// </summary>
    public class MutableValueTypeGenerator : ISpecimenBuilder
    {
        private readonly IRequestSpecification valueTypeWithoutConstructorsSpecification;

        /// <summary>
        /// Creates new instance.
        /// </summary>
        public MutableValueTypeGenerator()
        {
            this.valueTypeWithoutConstructorsSpecification = new AndRequestSpecification(new ValueTypeSpecification(),
                                                                                    new NoConstructorsSpecification());
        }

        /// <summary>
        /// Creates a new <see langword="struct"/>.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens. Not used.</param>
        /// <returns>
        /// The requested struct if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            Type type = request as Type;
            if (type == null || !this.valueTypeWithoutConstructorsSpecification.IsSatisfiedBy(type))
            {
                return new NoSpecimen();
            }

            return Activator.CreateInstance(type);
        }
    }
}

using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    public class NSubstituteRelay : ISpecimenBuilder
    {
        private readonly Func<Type, bool> shouldBeMocked; 

        /// <summary>
        /// Initializes a new instance of the <see cref="NSubstituteRelay"/> class.
        /// </summary>
        public NSubstituteRelay()
            : this(NSubstituteType.IsMockable)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NSubstituteRelay"/> class with a specification
        /// that determines whether a type should be mocked.
        /// </summary>
        /// <param name="mockableSpecification">
        /// A specification that determines whether a type should be mocked or not.
        /// </param>
        public NSubstituteRelay(Func<Type, bool> mockableSpecification)
        {
            if (mockableSpecification == null)
            {
                throw new ArgumentNullException("mockableSpecification");
            }

            shouldBeMocked = mockableSpecification;
        }
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var type = request as Type;
            if (type == null || !shouldBeMocked(type))
            {
                return new NoSpecimen(request);
            }

            return context.Resolve(request);
        }

        public Func<Type, bool> MockableSpecification
        {
            get { return shouldBeMocked; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Encapsulates a unit test that verifies that a type which overrides the <see cref="object.Equals(object)"/>
    /// method is implemented correctly with respect to the rule: calling `x.Equals(y)` 3 times returns same value.
    /// </summary>
    public class EqualsSuccessiveAssertion : IdiomaticAssertion
    {
        private readonly ISpecimenBuilder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualsSuccessiveAssertion"/> class.
        /// </summary>
        /// <param name="builder">
        /// A composer which can create instances required to implement the idiomatic unit test,
        /// such as the owner of the property, as well as the value to be assigned and read from
        /// the member.
        /// </param>
        /// <remarks>
        /// <para>
        /// <paramref name="builder" /> will typically be a <see cref="Fixture" /> instance.
        /// </para>
        /// </remarks>
        public EqualsSuccessiveAssertion(ISpecimenBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            this.builder = builder;
        }

        /// <summary>
        /// Gets the builder supplied by the constructor.
        /// </summary>
        public ISpecimenBuilder Builder
        {
            get { return this.builder; }
        }

        /// <summary>
        /// Verifies that calling `x.Equals(y)` 3 times on an instance of the type returns same value.
        /// </summary>
        /// <param name="type">The type.</param>
        public override void Verify(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (!type.GetMethods().Any(m => m.IsObjectEqualsOverrideMethod()))
            {
                // The type has no overrides of the Object.Equals(object) method
                return;
            }

            throw new NotImplementedException();
        }

    }

}

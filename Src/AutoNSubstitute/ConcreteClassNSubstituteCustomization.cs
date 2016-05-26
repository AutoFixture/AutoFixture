using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    /// <summary>
    /// Enables creating substitutes for concrete classes.
    /// </summary>
    public class ConcreteClassNSubstituteCustomization : ICustomization
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConcreteClassNSubstituteCustomization"/> class.
        /// </summary>
        /// <param name="type">The type to create a substityte for.</param>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is null.</exception>
        public ConcreteClassNSubstituteCustomization(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            this.Type = type;
        }

        /// <summary>
        /// Gets the type to create a substityte for.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Customizes the specified fixture by adding a specimen builder for 
        /// creating concrete type substitutes.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            var builder = new FilteringSpecimenBuilder(
                new MethodInvoker(new NSubstituteMethodQuery()),
                new ExactTypeSpecification(this.Type));

            fixture.Customizations.Insert(0, builder);
        }
    }
}
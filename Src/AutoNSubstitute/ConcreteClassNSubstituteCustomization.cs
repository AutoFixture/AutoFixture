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
        /// <param name="typeToProxy">The type to create a substityte for.</param>
        /// <exception cref="ArgumentNullException"><paramref name="typeToProxy"/> is null.</exception>
        public ConcreteClassNSubstituteCustomization(Type typeToProxy)
        {
            if (typeToProxy == null)
            {
                throw new ArgumentNullException(nameof(typeToProxy));
            }

            this.TypeToProxy = typeToProxy;
        }

        /// <summary>
        /// Gets the type to create a substityte for.
        /// </summary>
        public Type TypeToProxy { get; }

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
                new ExactTypeSpecification(this.TypeToProxy));

            fixture.Customizations.Insert(0, builder);
        }
    }
}
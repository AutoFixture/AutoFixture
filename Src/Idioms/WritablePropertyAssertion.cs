using System;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Encapsulates a unit test that verifies that a writable property is correctly implemented.
    /// </summary>
    /// <remarks>
    /// <para>
    /// According to the framework design guidelines, a writable property should always have both a
    /// setter and getter. In order not to violate the Principle of Least Astonishment the getter
    /// should always return the object assigned via the setter. Thus, for every writable property
    /// one should always write a unit test that verifies that the value read from the proeprty is
    /// the same as the value assigned to the property.
    /// </para>
    /// <para>
    /// However, such unit tests become tedious and repetitive to write; together, they form a unit
    /// testing idiom for .NET. This <see cref="IIdiomaticAssertion" /> encapsulates this idiom.
    /// </para>
    /// </remarks>
    public class WritablePropertyAssertion : IdiomaticAssertion
    {
        private readonly ISpecimenBuilderComposer composer;

        /// <summary>
        /// Initializes a new instance of the <see cref="WritablePropertyAssertion"/> class.
        /// </summary>
        /// <param name="composer">
        /// A composer which can create instances required to implement the idiomatic unit test,
        /// such as the owner of the property, as well as the value to be assigned and read from
        /// the property.
        /// </param>
        /// <remarks>
        /// <para>
        /// <paramref name="composer" /> will typically be a <see cref="Fixture" /> instance.
        /// </para>
        /// </remarks>
        public WritablePropertyAssertion(ISpecimenBuilderComposer composer)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }

            this.composer = composer;
        }

        /// <summary>
        /// Gets the composer supplied via the constructor.
        /// </summary>
        public ISpecimenBuilderComposer Composer
        {
            get { return this.composer; }
        }

        /// <summary>
        /// Verifies that a writable property is correctly implemented.
        /// </summary>
        /// <param name="propertyInfo">The property.</param>
        /// <remarks>
        /// <para>
        /// This method verifies that <paramref name="propertyInfo" /> is correctly implemented as
        /// a writable property. It used the <see cref="Composer" /> to create an instance of the
        /// Type on which the property is implemented and then assigns to and reads from the
        /// property. The assertion passes if the value read from the property is the same as the
        /// value assigned to it. If this is not the case, a
        /// <see cref="WritablePropertyException" /> is thrown.
        /// </para>
        /// <para>
        /// This method does nothing if the property is not a writable property.
        /// </para>
        /// </remarks>
        /// <exception cref="WritablePropertyException">The verification fails.</exception>
        public override void Verify(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");

            if (propertyInfo.GetSetMethod() == null)
                return;

            var specimen = this.composer.CreateAnonymous(propertyInfo.ReflectedType);
            var propertyValue = this.composer.CreateAnonymous(propertyInfo.PropertyType);

            propertyInfo.SetValue(specimen, propertyValue, null);
            var result = propertyInfo.GetValue(specimen, null);

            if (!propertyValue.Equals(result))
            {
                throw new WritablePropertyException(propertyInfo);
            }
        }
    }
}

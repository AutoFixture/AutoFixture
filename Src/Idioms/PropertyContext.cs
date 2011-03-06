using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Expresses idiomatic assertions related to a single property.
    /// </summary>
    public class PropertyContext : IPropertyContext
    {
        private readonly ISpecimenBuilderComposer composer;
        private readonly PropertyInfo propertyInfo;
        private readonly bool isPropertyReadOnly;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyContext"/> class.
        /// </summary>
        /// <param name="composer">
        /// The composer which will be used to create instances of the type owning the property, as
        /// well as the property value itself.
        /// </param>
        /// <param name="propertyInfo">The property.</param>
        public PropertyContext(ISpecimenBuilderComposer composer, PropertyInfo propertyInfo)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }
            if (propertyInfo == null)
            {
                throw new ArgumentNullException("propertyInfo");
            }

            this.composer = composer;
            this.propertyInfo = propertyInfo;
            this.isPropertyReadOnly = this.propertyInfo.GetSetMethod() == null;
        }

        /// <summary>
        /// Gets the composer provided to the constructor.
        /// </summary>
        public ISpecimenBuilderComposer Composer
        {
            get { return this.composer; }
        }

        /// <summary>
        /// Gets the property provided to the constructor.
        /// </summary>
        public PropertyInfo PropertyInfo
        {
            get { return this.propertyInfo; }
        }

        #region IPropertyContext Members

        /// <summary>
        /// Verifies that the property or properties encapsulated by the context is or are
        /// writable, and that the value returned is the same as the value which was originally
        /// assigned.
        /// </summary>
        public void VerifyWritable()
        {
            if (this.isPropertyReadOnly)
            {
                throw new PropertyContextException("The supplied PropertyInfo is read-only.");
            }

            var specimen = this.composer.CreateAnonymous(this.PropertyInfo.ReflectedType);
            var propertyValue = this.composer.CreateAnonymous(this.PropertyInfo.PropertyType);

            this.PropertyInfo.SetValue(specimen, propertyValue, null);
            var result = this.PropertyInfo.GetValue(specimen, null);

            if (!propertyValue.Equals(result))
            {
                throw new PropertyContextException("The supplied PropertyInfo does not point out a well-behaved property.");
            }
        }

        #endregion

        #region IMemberContext Members

        /// <summary>
        /// Verifies the boundaries conditions of the type member(s) encapsulated by the context.
        /// </summary>
        /// <param name="convention">The convention to use to verify the boundaries.</param>
        /// <remarks>
        /// An example of a convention could be to verify that all method parameters are protected
        /// by Guard Clauses the protect against null references.
        /// </remarks>
        public void VerifyBoundaries(IBoundaryConvention convention)
        {
            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }

            if (this.isPropertyReadOnly)
            {
                return;
            }

            var specimen = this.composer.CreateAnonymous(this.PropertyInfo.ReflectedType);
            Action<object> setProperty = x => this.PropertyInfo.SetValue(specimen, x, null);

            var behaviors = from b in convention.CreateBoundaryBehaviors(this.PropertyInfo.PropertyType)
                            select b.UnwrapReflectionExceptions();
            foreach (var b in behaviors)
            {
                b.Assert(setProperty, string.Format(CultureInfo.InvariantCulture, "Assigning the property {0}", this.PropertyInfo.Name));
            }
        }

        #endregion
    }
}
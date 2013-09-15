using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Composes an arbitrary number of <see cref="IdiomaticAssertion" /> instances.
    /// </summary>
    public class CompositeIdiomaticAssertion : IdiomaticAssertion
    {
        private readonly IEnumerable<IIdiomaticAssertion> assertions;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeIdiomaticAssertion"/> class with
        /// the supplied <see cref="IIdiomaticAssertion" /> instances.
        /// </summary>
        /// <param name="assertions">The encapsulated assertions.</param>
        public CompositeIdiomaticAssertion(params IIdiomaticAssertion[] assertions)
        {
            this.assertions = assertions;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeIdiomaticAssertion"/> class with
        /// the supplied <see cref="IIdiomaticAssertion" /> instances.
        /// </summary>
        /// <param name="assertions">The encapsulated assertions.</param>
        public CompositeIdiomaticAssertion(IEnumerable<IIdiomaticAssertion> assertions)
            : this(assertions.ToArray())
        {
        }

        /// <summary>
        /// Gets the assertions supplied via the constructor.
        /// </summary>
        public IEnumerable<IIdiomaticAssertion> Assertions
        {
            get { return this.assertions; }
        }

        /// <summary>
        /// Verifies the behavior of the constructor by delegating the implementation to
        /// all <see cref="Assertions" />.
        /// </summary>
        /// <param name="constructorInfo">The constructor whose behavior must be verified.</param>
        public override void Verify(ConstructorInfo constructorInfo)
        {
 	        foreach(var assertion in this.assertions)
            {
                assertion.Verify(constructorInfo);
            }
        }

        /// <summary>
        /// Verifies the behavior of the constructor by delegating the implementation to
        /// all <see cref="Assertions" />.
        /// </summary>
        /// <param name="methodInfo">The method whose behavior must be verified.</param>
        public override void Verify(MethodInfo methodInfo)
        {
 	        foreach(var assertion in this.assertions)
            {
                assertion.Verify(methodInfo);
            }
        }

        /// <summary>
        /// Verifies the behavior of the property by delegating the implementation to
        /// all <see cref="Assertions" />.
        /// </summary>
        /// <param name="propertyInfo">The property whose behavior must be verified.</param>
        public override void Verify(PropertyInfo propertyInfo)
        {
 	        foreach(var assertion in this.assertions)
            {
                assertion.Verify(propertyInfo);
            }
        }

        /// <summary>
        /// Verifies the behavior of the field by delegating the implementation to
        /// all <see cref="Assertions" />.
        /// </summary>
        /// <param name="fieldInfo">The field whose behavior must be verified.</param>
        public override void Verify(FieldInfo fieldInfo)
        {
 	        foreach(var assertion in this.assertions)
            {
                assertion.Verify(fieldInfo);
            }
        }
    }
}

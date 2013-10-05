using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Encapsulates a unit test which verifies a method correctly makes a copy of an
    /// object while updating one or more public properties or fields.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When dealing with immutable types, it can be useful to add a convenience method
    /// to change a single field/property in a more complex object; e.g.
    /// <code>
    /// public Foo WithBar(Bar newBar)
    /// {
    ///     return new Foo(this.baz, newBar, this.qux);
    /// }
    /// </code>
    /// </para>
    /// Testing this not only requires verification that newBar was properly used (and exposed
    /// as a field or Inspection Property), but that all other values contained by Foo are held
    /// constant.
    /// </remarks>
    public class CopyAndUpdateAssertion : IdiomaticAssertion
    {
        private readonly ISpecimenBuilder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyAndUpdateAssertion"/> class.
        /// </summary>
        /// <param name="builder">
        /// A composer which can create instances required to implement the idiomatic unit test.
        /// </param>
        /// <remarks>
        /// <para>
        /// <paramref name="builder" /> will typically be a <see cref="Fixture" /> instance.
        /// </para>
        /// </remarks>
        public CopyAndUpdateAssertion(ISpecimenBuilder builder)
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
        /// Verifies that a method correctly makes a copy of an object while changing
        /// one or more public properties or fields.
        /// </summary>
        /// <param name="methodInfo">The 'copy and update' method to verify</param>
        public override void Verify(MethodInfo methodInfo)
        {
            if (methodInfo == null)
                throw new ArgumentNullException("methodInfo");

            throw new NotImplementedException();
        }

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using System.Globalization;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Encapsulates a unit test that verifies that a type which overrides the
    /// <see cref="object.Equals(object)"/> method is implemented correctly with
    /// respect to the rule: calling `x.Equals(new object())` returns false.
    /// </summary>
    public class EqualsNewObjectAssertion : IdiomaticAssertion
    {
        private readonly ISpecimenBuilder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualsNewObjectAssertion"/> class.
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
        public EqualsNewObjectAssertion(ISpecimenBuilder builder)
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
        /// Verifies that calling `x.Equals(new object())` on an instance of the type 
        /// returns false, if the supplied method is an override of the 
        /// <see cref="object.Equals(object)"/>.
        /// </summary>
        /// <param name="methodInfo">The method to verify</param>
        public override void Verify(MethodInfo methodInfo)
        {
            if (methodInfo == null)
                throw new ArgumentNullException("methodInfo");

            if (methodInfo.ReflectedType == null ||
                !methodInfo.IsObjectEqualsOverrideMethod())
            {
                // The method is not an override of the Object.Equals(object) method
                return;
            }

            var instance = this.builder.CreateAnonymous(methodInfo.ReflectedType);
            var equalsResult = instance.Equals(new object());
            if (equalsResult)
            {
                throw new EqualsOverrideException(string.Format(CultureInfo.CurrentCulture,
                    "The type '{0}' overrides the object.Equals(object) method incorrectly, " +
                    "calling x.Equals(new object()) should return false.",
                    methodInfo.ReflectedType.FullName));
            }
        }
    }
}

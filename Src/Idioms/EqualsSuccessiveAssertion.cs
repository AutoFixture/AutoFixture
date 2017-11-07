using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.Idioms
{
    /// <summary>
    /// Encapsulates a unit test that verifies that a type which overrides the
    /// <see cref="object.Equals(object)"/> method is implemented correctly with
    /// respect to the rule: calling `x.Equals(y)` 3 times returns same value.
    /// </summary>
    public class EqualsSuccessiveAssertion : IdiomaticAssertion
    {
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
            this.Builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Gets the builder supplied by the constructor.
        /// </summary>
        public ISpecimenBuilder Builder { get; }

        /// <summary>
        /// Verifies that `x.Equals(y)` 3 times on an instance of the type returns same
        /// value, if the supplied method is an override of the 
        /// <see cref="object.Equals(object)"/>.
        /// </summary>
        /// <param name="methodInfo">The method to verify</param>
        public override void Verify(MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));

            if (methodInfo.ReflectedType == null ||
                !methodInfo.IsObjectEqualsOverrideMethod())
            {
                // The method is not an override of the Object.Equals(object) method
                return;
            }

            var instance = this.Builder.CreateAnonymous(methodInfo.ReflectedType);
            var other = this.Builder.CreateAnonymous(methodInfo.ReflectedType);

            var results = Enumerable.Range(1, 3)
                .Select(i => instance.Equals(other))
                .ToArray();

            if (results.Any(result => result != results[0]))
            {
                throw new EqualsOverrideException(string.Format(CultureInfo.CurrentCulture,
                    "The type '{0}' overrides the object.Equals(object) method incorrectly, " +
                    "calling x.Equals(y) multiple times should return the same value.",
                    methodInfo.ReflectedType.FullName));
            }
        }
    }
}

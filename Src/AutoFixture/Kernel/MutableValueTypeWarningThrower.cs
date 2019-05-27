using System.Globalization;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Throws an <see cref="ObjectCreationException"/> when one tries to create Structure without explicit parametrized constructor.
    /// Possibly notifying about bad design (mutable value type).
    /// </summary>
    /// <remarks>
    /// <para>
    /// This <see cref="ISpecimenBuilder"/> can be used with proper filtering <see cref="IRequestSpecification"/> to throw exceptions only on
    /// structures without constructors. Will throw an exception instead of letting the
    /// containing builder return a <see cref="NoSpecimen"/> instance when it can't satisfy a
    /// request or generic exception being thrown.
    /// </para>
    /// </remarks>
    public class MutableValueTypeWarningThrower : ISpecimenBuilder
    {
        /// <summary>
        /// Throws an <see cref="ObjectCreationException"/>.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">
        /// A context that can be used to create other specimens. Not used.
        /// </param>
        /// <returns>
        /// This method never returns. It always throws an <see cref="ObjectCreationException"/>.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            throw new ObjectCreationException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    @"AutoFixture was unwilling to create an instance from {0}, since it is a value type with no explicit, parameterized constructors. Are you attempting to create an instance of a mutable value type? If so, you should strongly consider changing the design of the value type. However, if you are unable to do so, you can add the {1} customizations to your Fixture instance:
var fixture = new Fixture();
var customization = new {1}();
customization.Customize(fixture);

For more information about mutable value types please refer to: http://tinyurl.com/pegtw57",
                    request, typeof(SupportMutableValueTypesCustomization).Name));
        }
    }
}
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;

// This class is placed in the AutoFixture namespace to improve discoverability.
namespace AutoFixture
{
    public static class FixtureExtensions
    {
        /// <summary>
        /// Applies the AutoMoqCustomization on the <see cref="IFixture"/>.
        /// </summary>
        /// <param name="fixture">The fixture upon which to enable auto-mocking.</param>
        /// <param name="configureMembers">Specifies whether members of a mock will be automatically setup to retrieve the return values from a fixture.</param>
        /// <param name="generateDelegates">Specifies whether delegate requests are intercepted and handled by Moq.</param>
        /// <param name="relay">The relay that will be added to <see cref="IFixture.ResidueCollectors"/> when the customization is applied on the <see cref="fixture"/>.</param>
        /// <returns>The <see cref="fixture"/> with the customization applied.</returns>
        public static IFixture AddAutoMoq(this IFixture fixture, bool configureMembers = false, bool generateDelegates = false, ISpecimenBuilder relay = null)
        {
            var customization = new AutoMoqCustomization
            {
                ConfigureMembers = configureMembers,
                GenerateDelegates = generateDelegates
            };

            if (relay != null)
            {
                customization.Relay = relay;
            }

            fixture.Customize(customization);

            return fixture;
        }
    }
}

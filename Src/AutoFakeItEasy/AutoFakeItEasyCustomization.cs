using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoFakeItEasy
{
    /// <summary>
    /// Enables auto-mocking with FakeItEasy.
    /// </summary>
    public class AutoFakeItEasyCustomization : ICustomization
    {
        /// <summary>
        /// Customizes an <see cref="IFixture"/> to enable auto-mocking with Rhino Mocks.
        /// </summary>
        /// <param name="fixture">The fixture upon which to enable auto-mocking.</param>
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            fixture.ResidueCollectors.Add(
                new FakeItEasyRelay(
                    new MethodInvoker(
                        new FakeItEasyMethodQuery())));
        }
    }
}

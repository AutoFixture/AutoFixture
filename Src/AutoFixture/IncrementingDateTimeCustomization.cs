using System;

namespace Ploeh.AutoFixture
{
    public class IncrementingDateTimeCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            fixture.Customizations.Add(new StrictlyMonotonicallyIncreasingDateTimeGenerator(DateTime.Now));
        }
    }
}

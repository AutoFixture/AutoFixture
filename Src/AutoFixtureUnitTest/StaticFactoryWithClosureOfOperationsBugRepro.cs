using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureUnitTest
{
    /* This class contains regression tests against this bug:
     * http://autofixture.codeplex.com/workitem/4256 */
    public class StaticFactoryWithClosureOfOperationsBugRepro
    {
        [Fact]
        public void CreateWithOmitOnRecursionThrowsAppropriateException()
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Exercise system and verify outcome
            Assert.Throws<ObjectCreationException>(() =>
                fixture.Create<ClosureOfOperationsHost>());
            // Teardown
        }

        private class ClosureOfOperationsHost
        {
            private ClosureOfOperationsHost()
            {
            }

            public static ClosureOfOperationsHost CloseAround(
                ClosureOfOperationsHost thing)
            {
                return thing;
            }
        }
    }
}

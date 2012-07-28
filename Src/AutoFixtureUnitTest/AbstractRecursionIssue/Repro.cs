using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture;
using System.Collections;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.AbstractRecursionIssue
{
    public class Repro
    {
        /// <summary>
        /// This test reproduces the issue exactly as reported at
        /// https://github.com/AutoFixture/AutoFixture/issues/3
        /// </summary>
        [Fact(Skip = "Waiting for fix. Most likely this is best fixed in AutoFixture 3.0 - at least no clear fix for AutoFixture 2.x is immediately apparent.")]
        public void IssueAsReported()
        {
            var fixture = new Fixture().Customize(new MultipleCustomization());
            fixture.RepeatCount = 3;
            fixture.Behaviors.Clear();
            fixture.Behaviors.Add(new NullRecursionBehavior());

            fixture.Register<ItemBase>(() => fixture.CreateAnonymous<FunkyItem>());

            var funkyItem = fixture.CreateAnonymous<FunkyItem>();

            /* No assertion is in place because the above code is a verbatim repro of the issue as
             * reported. When reported, the last line of code (CreateAnonymous<FunkyItem>()) threw
             * a StackOverflowException in CompositeSpecimenBuilder.Create. */
        }

        /// <summary>
        /// This test reduces the issue to essentials. Despite the fewer lines of code, it exhibits
        /// the same behavior as the test above.
        /// </summary>
        [Fact(Skip = "Waiting for fix. Most likely this is best fixed in AutoFixture 3.0 - at least no clear fix for AutoFixture 2.x is immediately apparent.")]
        public void IssueReduced()
        {
            var fixture = new Fixture().Customize(new MultipleCustomization());
            fixture.Register<ItemBase>(fixture.CreateAnonymous<FunkyItem>);

            Assert.DoesNotThrow(() => fixture.CreateAnonymous<FunkyItem>());
        }

        [Fact]
        public void Workaround()
        {
            var fixture = new Fixture().Customize(new MultipleCustomization());
            fixture.RepeatCount = 3;
            fixture.Behaviors.Clear();
            fixture.Behaviors.Add(new NullRecursionBehavior());

            fixture.Customizations.Add(
                new TypeRelay(
                    typeof(ItemBase),
                    typeof(FunkyItem)));

            Assert.DoesNotThrow(() => fixture.CreateAnonymous<FunkyItem>());
        }
    }
}

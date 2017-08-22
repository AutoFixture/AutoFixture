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
        [Fact]
        public void IssueAsReported()
        {
            var fixture = new Fixture().Customize(new MultipleCustomization());
            fixture.RepeatCount = 3;
            fixture.Behaviors.Clear();
            fixture.Behaviors.Add(new NullRecursionBehavior());

            fixture.Register<ItemBase>(() => fixture.Create<FunkyItem>());

            var funkyItem = fixture.Create<FunkyItem>();

            /* No assertion is in place because the above code is a verbatim repro of the issue as
             * reported. When reported, the last line of code (Create<FunkyItem>()) threw
             * a StackOverflowException in CompositeSpecimenBuilder.Create. */
        }

        /// <summary>
        /// This test reduces the issue to essentials. Despite the fewer lines of code, it exhibits
        /// the same behavior as the test above.
        /// </summary>
        [Fact]
        public void IssueReduced()
        {
            var fixture = new Fixture().Customize(new MultipleCustomization());
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new NullRecursionBehavior());
            fixture.Register<ItemBase>(fixture.Create<FunkyItem>);

            Assert.Null(Record.Exception(() => fixture.Create<FunkyItem>()));
        }

        /// <summary>
        /// This test presents one possible workaround to the issue reported above.
        /// </summary>
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

            Assert.Null(Record.Exception(() => fixture.Create<FunkyItem>()));
        }
    }
}

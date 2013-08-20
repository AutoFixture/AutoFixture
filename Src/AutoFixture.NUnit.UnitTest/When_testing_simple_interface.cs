using AutoSpecificationFor.Tests.Models;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.NUnit;
using Ploeh.AutoFixture.NUnit.Specifications;

namespace AutoSpecificationFor.Tests
{
    public class When_testing_simple_interface : SpecificationFor<ISimple>
    {
        protected string ExpectedValue { get; set; }

        public When_testing_simple_interface()
            : base(new Fixture().Customize(new AutoNSubstituteCustomization()))
        {
        }

        protected override ISimple Given()
        {
            return Fixture.Create<ISimple>();
        }

        protected override void When()
        {
            ExpectedValue = Fixture.Create<string>();

            Subject.PropertyExample = ExpectedValue;
        }

        [Then]
        public void Should_be_able_to_set_property_value()
        {
            Assert.AreEqual(Subject.PropertyExample, ExpectedValue);
        }
    }
}
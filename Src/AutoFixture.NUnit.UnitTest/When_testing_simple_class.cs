using AutoSpecificationFor.Tests.Models;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.NUnit;
using Ploeh.AutoFixture.NUnit.Specifications;

namespace AutoSpecificationFor.Tests
{
    public class When_testing_simple_class : SpecificationFor<Simple>
    {
        protected string ExpectedValue { get; set; }
        
        protected override Simple Given()
        {
            ExpectedValue = Fixture.Create<string>();

            return Fixture.Create<Simple>();
        }

        protected override void When()
        {
            Subject.PropertyExample = ExpectedValue;
        }

        [Then]
        public void Should_be_able_to_set_property_value()
        {
            Assert.AreEqual(Subject.PropertyExample, ExpectedValue);
        }
    }
}
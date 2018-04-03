using NUnit.Framework;
using TestTypeFoundation;

namespace AutoFixture.NUnit3.UnitTest {
    [TestFixture]
    public class AutoValueAttributeUsageTest
    {
        [Test]
        public void AutoValueAttribute_should_provide_a_fixed_Model(
            [Values] bool flag,
            [Range(1, 5)] int statusCode,
            [AutoValue] DoubleParameterType<int, string> model
        )
        {
            Assert.That(model, Is.Not.Null);
        }

        [Test]
        public void AutoValueAttribute_should_provide_a_simple_type([AutoValue] int value)
        {
            Assert.Pass();
        }

        [Test]
        public void AutoValueAttribute_should_provide_a_complex_type([AutoValue] DoubleParameterType<int, string> value)
        {
            Assert.Pass();
        }

    }
}
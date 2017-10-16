using AutoFixture.NUnit2.Addins;
using NUnit.Framework;

namespace AutoFixture.NUnit2.UnitTest
{
    [TestFixture]
    public class ConstantTest
    {
        [Test]
        public void AutoDataAttributeShouldPointToValidType()
        {
            // Fixture setup
            var expectedName = typeof(AutoDataAttribute).FullName;

            // Exercise system and verify outcome
            Assert.AreEqual(expectedName, Constants.AutoDataAttribute);

            // Teardown
        }
    }
}
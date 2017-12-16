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
            // Arrange
            var expectedName = typeof(AutoDataAttribute).FullName;

            // Act & Assert
            Assert.AreEqual(expectedName, Constants.AutoDataAttribute);
        }
    }
}
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit.org;

namespace AutoFixture.NUnit.org.Addins.UnitTest
{
    public class SampleTest
    {
        [Test, AutoData]
        public void SampleMethod(int number)
        {
            Assert.IsTrue(true);
        }
    }
}
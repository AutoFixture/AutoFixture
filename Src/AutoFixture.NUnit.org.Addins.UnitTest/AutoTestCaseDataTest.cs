using NUnit.Framework;
using Ploeh.AutoFixture.NUnit.org.Addins;

namespace AutoFixture.NUnit.org.Addins.UnitTest
{
    [TestFixture]
    public class AutoTestCaseDataTest
    {


        [Test]
        public void Test()
        {
            AutoTestCaseData.FromAutoData();
        }
    }
}
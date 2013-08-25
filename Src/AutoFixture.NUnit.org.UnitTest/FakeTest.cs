using NUnit.Framework;
using Ploeh.AutoFixture.NUnit.org;

namespace Ploe.AutoFixture.NUnit.org.UnitTest
{
    public class FakeTest
    {
        [AutoTestCase]
        public void DoSomething(int number)
        {
            Assert.IsTrue(number > 0);
        }
    }
}
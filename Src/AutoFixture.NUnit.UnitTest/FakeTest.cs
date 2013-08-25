using NUnit.Framework;
using Ploeh.AutoFixture.NUnit;

namespace Ploe.AutoFixture.NUnit.UnitTest
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
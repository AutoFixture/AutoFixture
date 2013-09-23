using Ploeh.AutoFixture.NUnit2;

namespace Ploeh.AutoFixture.NUnit2.Addins.UnitTest
{
    public class FakeAutoDataFixture
    {
        [AutoData]
        public void DoSomething(int number)
        {
        }
    }
}
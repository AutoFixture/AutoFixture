using NUnit.Framework;

namespace Ploeh.AutoFixture.NUnit.UnitTest.TestCases
{
    public class InitializationCountTests
    {
        [InitializationCountTestCase(typeof(InitializationCountTests), "CreationOfParameterOnlyHappensOnce")]
        public void CreationOfParameterOnlyHappensOnce(InitializationCounter counter)
        {
            Assert.That(counter.TotalCount, Is.EqualTo(1));
        }
    }
}

using Ploeh.AutoFixture.AutoMoq;

namespace Ploeh.AutoFixture.NUnit3.UnitTest
{
    public class TestBase
    {
        protected IFixture Fixture;

        protected TestBase()
        {
            this.Fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        protected T Any<T>()
        {
            return this.Fixture.Create<T>();
        }
    }
}
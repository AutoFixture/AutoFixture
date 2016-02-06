using Ploeh.AutoFixture.AutoMoq;

namespace Ploeh.AutoFixture.NUnit3.UnitTest
{
    public class TestBase
    {
        protected IFixture Fixture;

        protected TestBase()
        {
            this.Fixture = new Fixture();

            this.Fixture.Customize(new AutoMoqCustomization());
            this.Fixture.Customize(new MultipleCustomization());
        }

        protected T Any<T>()
        {
            return this.Fixture.Create<T>();
        }
    }
}
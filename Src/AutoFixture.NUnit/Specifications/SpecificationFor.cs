using System.Linq;
using NUnit.Framework;

namespace Ploeh.AutoFixture.NUnit.Specifications
{
    [TestFixture]
    public abstract class SpecificationFor<T>
    {
        private readonly IFixture _fixture;

        protected SpecificationFor()
        {
            var attributes = GetType().GetCustomAttributes(typeof(TestConventionsAttribute), false).OfType<TestConventionsAttribute>();
            var attribute = attributes.FirstOrDefault();
            
            if (attribute != null)
            {
                _fixture = attribute.Fixture ?? new Fixture();
            }
            else
            {
                _fixture = new Fixture();
            }
        }

        protected SpecificationFor(IFixture fixture)
        {
            _fixture = fixture;
        }

        protected abstract T Given();

        protected abstract void When();

        protected T Subject { get; private set; }

        protected IFixture Fixture
        {
            get { return _fixture; }
        }

        [TestFixtureSetUp]
        public void SetUp()
        {
            Subject = Given();
            When();
        }
    }
}

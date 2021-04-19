using System;

namespace AutoFixture.NUnit3.UnitTest
{
    internal class DelegatingFixtureFactory
    {
        public DelegatingFixtureFactory()
        {
        }

        public DelegatingFixtureFactory(Func<IFixture> factory)
        {
            this.OnFixtureCreated = factory;
        }

        public Func<IFixture> OnFixtureCreated { get; set; }

        public bool Invoked { get; private set; }

        public void Reset()
        {
            this.Invoked = false;
        }

        public IFixture Invoke()
        {
            this.Invoked = true;
            return this.OnFixtureCreated?.Invoke();
        }

        public static implicit operator Func<IFixture>(DelegatingFixtureFactory factory)
        {
            return factory.Invoke;
        }
    }
}
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureDocumentationTest.Greedy
{
    public class Scenario
    {
        [Fact]
        public void BastardIsCreatedWithModestConstructorByDefault()
        {
            var fixture = new Fixture();
            var b = fixture.Create<Bastard>();
            Assert.IsAssignableFrom<DefaultFoo>(b.Foo);
        }

        [Fact]
        public void FooIsDefaultEvenWhenFooIsFrozen()
        {
            var fixture = new Fixture();
            fixture.Register<IFoo>(
                fixture.Create<DummyFoo>);
            var b = fixture.Create<Bastard>();
            Assert.IsAssignableFrom<DefaultFoo>(b.Foo);
        }

        [Fact]
        public void FooIsDummyWithSpecializedCustomization()
        {
            var fixture = new Fixture();
            fixture.Customize<Bastard>(c => c.FromFactory(
                new MethodInvoker(
                    new GreedyConstructorQuery())));
            fixture.Register<IFoo>(
                fixture.Create<DummyFoo>);
            var b = fixture.Create<Bastard>();
            Assert.IsAssignableFrom<DummyFoo>(b.Foo);
        }

        [Fact]
        public void FooIsDummyWithGeneralCustomization()
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(
                new MethodInvoker(
                    new GreedyConstructorQuery()));
            fixture.Register<IFoo>(
                fixture.Create<DummyFoo>);
            var b = fixture.Create<Bastard>();
            Assert.IsAssignableFrom<DummyFoo>(b.Foo);
        }
    }
}

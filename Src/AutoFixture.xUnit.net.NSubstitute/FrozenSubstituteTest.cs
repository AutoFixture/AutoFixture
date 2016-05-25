using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Xunit;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace AutoFixture.xUnit.net.NSubstitute
{
    public class FrozenSubstituteTest
    {
        [Theory, AutoNSubstituteData]
        public void CreateConcreteTypeSubstituteSubstitute([Substitute] ConcreteType sut)
        {
            Assert.IsAssignableFrom<ConcreteType>(sut);
            Assert.True(sut.GetType().BaseType == typeof(ConcreteType));
        }

        [Theory, AutoNSubstituteData]
        public void CreateFrozenConcreteTypeSubstituteSubstitute([Frozen, Substitute] ConcreteType sut)
        {
            Assert.IsAssignableFrom<ConcreteType>(sut);
            Assert.True(sut.GetType().BaseType == typeof(ConcreteType));
        }

        private class AutoNSubstituteDataAttribute : AutoDataAttribute
        {
            public AutoNSubstituteDataAttribute()
              : base(new Fixture().Customize(new AutoNSubstituteCustomization()))
            {
            }
        }
    }
}
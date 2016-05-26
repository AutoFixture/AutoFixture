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
        public void CreateConcreteTypeSubstitute([Substitute] ConcreteType sut)
        {
            Assert.True(sut.GetType().BaseType == typeof(ConcreteType));
        }

        [Theory, AutoNSubstituteData]
        public void FreezeConcreteTypeSubstitute([Frozen, Substitute] ConcreteType ct1, ConcreteType ct2)
        {
            Assert.True(ct1.GetType().BaseType == typeof(ConcreteType));
            Assert.Same(ct1, ct2);
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
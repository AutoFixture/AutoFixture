using System;
using System.Reflection;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Kernel;
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
        public void FreezeConcreteTypeSubstitute([Frozen, Substitute2] ConcreteType ct1, ConcreteType ct2)
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

        private class Substitute2Attribute : CustomizeAttribute
        {
            public override ICustomization GetCustomization(ParameterInfo parameter)
            {
                var type = typeof(ConcreteType);
                return new ConcreteSubstituteCustomization(type);
            }
        }

        public class ConcreteSubstituteCustomization : ICustomization
        {
            private readonly Type type;

            public ConcreteSubstituteCustomization(Type type)
            {
                this.type = type;
            }

            public void Customize(IFixture fixture)
            {
                var builder = new FilteringSpecimenBuilder(
                    new MethodInvoker(new NSubstituteMethodQuery()),
                    new ExactTypeSpecification(type));

                fixture.Customizations.Insert(0, builder);
            }
        }
    }
}
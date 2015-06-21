using NSubstitute;
using NSubstitute.Exceptions;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public class Scenario
    {
        [Theory, AutoDataWithAutoSubstituteCustomization]
        public void InterfacesAreSubstitutedByDefault(IInterface i, object obj)
        {
            i.Method(obj);
            i.Received().Method(obj);
        }

        [Theory, AutoDataWithAutoSubstituteCustomization]
        public void AbstractTypesAreSubstitutedByDefault(AbstractType t, object obj)
        {
            t.AbstractMethod(obj);
            t.Received().AbstractMethod(obj);
        }

        [Theory, AutoDataWithAutoSubstituteCustomization]
        public void ConcreteTypesAreNotSubstitutedByDefault(ConcreteType t, object obj)
        {
            t.VirtualMethod(obj);
            Assert.Throws<NotASubstituteException>(() => t.Received().VirtualMethod(obj));
        }

        [Theory, AutoDataWithAutoSubstituteCustomization]
        public void ConcreteTypesAreSubstitutedWhenParameterIsMarkedWithSubstituteAttribute(
            [Substitute]ConcreteType t, object obj)
        {
            t.VirtualMethod(obj);
            t.Received().VirtualMethod(obj);
        }

        [Theory, AutoDataWithAutoSubstituteCustomization]
        public void SubstituteAttributeAppliesToSingleParameterAndNotItsEntireType(
            [Substitute]ConcreteType t1, ConcreteType t2, object obj)
        {
            t2.VirtualMethod(obj);
            Assert.Throws<NotASubstituteException>(() => t2.Received().VirtualMethod(obj));
        }

        // [Theory] // This test always fails. Uncomment when you need to observe its behavior.
        [AutoDataWithAutoSubstituteCustomization]
        public void AttemptingToSubstituteSealedTypeFailsTestBeforeItStarts(
            [Substitute]SealedType t, object obj)
        {
            t.VirtualMethod(obj);
            t.Received().VirtualMethod(obj);
        }
        
        // [Theory] // This test always fails. Uncomment when you need to observe its behavior.
        [AutoData]
        public void SubstituteAttributeShouldWorkWithPlainAutoDataAttribute(
            [Substitute]ConcreteType t, object obj)
        {
            t.VirtualMethod(obj);
            // The following line throws NotASubstituteExeption, which is confusing because the 
            // Substitute attribute is clearly used.
            t.Received().VirtualMethod(obj);
        }

        public class AutoDataWithAutoSubstituteCustomizationAttribute : AutoDataAttribute
        {
            public AutoDataWithAutoSubstituteCustomizationAttribute()
            {
                this.Fixture.Customize(new AutoSubstituteCustomization());
            }
        }

        public interface IInterface
        {
            void Method(object obj);
        }

        public abstract class AbstractType
        {
            public abstract void AbstractMethod(object obj);
        }

        public class ConcreteType
        {
            public virtual void VirtualMethod(object obj)
            {
            }
        }

        public sealed class SealedType : ConcreteType
        {
        }
    }
}

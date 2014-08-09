using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Moq;
using Ploeh.AutoFixture.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.AutoMoq.UnitTest
{
    public class SealedPropertyInitializerTest
    {
        [Fact]
        public void SetupThrowsWhenMockIsNull()
        {
            // Fixture setup
            var context = new Mock<ISpecimenContext>();
            var sut = new SealedPropertyInitializer();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => sut.Setup(null, context.Object));
            // Teardown
        }

        [Fact]
        public void SetupThrowsWhenContextIsNull()
        {
            // Fixture setup
            var mock = new Mock<object>();
            var sut = new SealedPropertyInitializer();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => sut.Setup(mock, null));
            // Teardown
        }

        [Fact]
        public void InitializesSealedPropertyUsingContext()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<ClassWithSealedProperty>();

            var sut = new SealedPropertyInitializer();
            // Exercise system
            sut.Setup(mock, new SpecimenContext(fixture));
            // Verify outcome
            Assert.Equal(frozenString, mock.Object.SealedProperty);
            Assert.Equal(frozenString, mock.Object.ImplicitlySealedProperty);
            // Teardown
        }

        [Fact]
        public void IgnoresGetOnlyProperties()
        {
            // Fixture setup
            var fixture = new Fixture();
            var mock = new Mock<ClassWithReadOnlyProperty>();

            var sut = new SealedPropertyInitializer();
            //Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Setup(mock, new SpecimenContext(fixture)));
        }

        [Fact]
        public void IgnoresVirtualProperties()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<ClassWithVirtualProperty>();

            var sut = new SealedPropertyInitializer();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Setup(mock, new SpecimenContext(fixture)));
            Assert.NotEqual(frozenString, mock.Object.VirtualProperty);
        }

        [Fact]
        public void IgnoresPropertiesWithPrivateSetter()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<ClassWithPropertyWithPrivateSetter>();

            var sut = new SealedPropertyInitializer();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Setup(mock, new SpecimenContext(fixture)));
            Assert.NotEqual(frozenString, mock.Object.PropertyWithPrivateSetter);
        }

        [Fact]
        public void IgnoresPrivateProperties()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<ClassWithPrivateProperty>();
            var privateProperty = typeof (ClassWithPrivateProperty)
                .GetProperty("PrivateProperty",
                             BindingFlags.Instance | BindingFlags.NonPublic);

            var sut = new SealedPropertyInitializer();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Setup(mock, new SpecimenContext(fixture)));
            Assert.NotEqual(frozenString, privateProperty.GetValue(mock.Object, null));
        }

        [Fact]
        public void IgnoresInterfaceProperties()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<IInterfaceWithProperty>();

            var sut = new SealedPropertyInitializer();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Setup(mock, new SpecimenContext(fixture)));
            Assert.NotEqual(frozenString, mock.Object.Property);
        }

        public abstract class TempClass
        {
            public abstract string SealedProperty { get; set; }
        }

        public class ClassWithSealedProperty : TempClass
        {
            public override sealed string SealedProperty { get; set; }
            public string ImplicitlySealedProperty { get; set; }
        }

        public class ClassWithReadOnlyProperty
        {
            public string ReadOnlyProperty
            {
                get { return ""; }
            }
        }

        public class ClassWithVirtualProperty
        {
            public virtual string VirtualProperty { get; set; }
        }

        public class ClassWithPropertyWithPrivateSetter
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            public string PropertyWithPrivateSetter { get; private set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }

        public class ClassWithPrivateProperty
        {
            // ReSharper disable UnusedMember.Local
            private string PrivateProperty { get; set; }
            // ReSharper restore UnusedMember.Local
        }

        public interface IInterfaceWithProperty
        {
            string Property { get; set; }
        }
    }
}
